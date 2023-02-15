using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AnvilCloud.Kubernetes.Probes
{
    /// <summary>
    /// Responsible for hosting <see cref="IProbe"/> implementations at runtime.
    /// </summary>
    internal class ProbeHost : IHostedService
    {
        private readonly ILogger<ProbeHost> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IProbeRegistration[] probeRegistrations;
        private readonly IList<ProbeOwner> probes = new List<ProbeOwner>();

        public ProbeHost(
            ILogger<ProbeHost> logger,
            IEnumerable<IProbeRegistration> probeRegistrations,
            IServiceProvider serviceProvider,
            HealthReportMessenger messenger)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.probeRegistrations = probeRegistrations.ToArray();

            messenger.SetProbeHost(this);
        }

        internal async Task OnHealthReportAsync(HealthReport report, CancellationToken cancellationToken)
        {
            logger.LogTrace("ProbeHost received health report: {Status}", report.Status);

            foreach (var probe in probes)
            {
                try
                {
                    var healthStatusForProbe = report.GetEffectiveHealthStatus(probe.Registration.Predicate);

                    await probe.Probe.SetHealthStatusAsync(healthStatusForProbe, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Problem sending health report status to probe '{ProbeName}'", probe.Registration.Name);
                }
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating {ProbeCount} probes...", probeRegistrations.Length);

            foreach (var registration in probeRegistrations)
            {
                try
                {
                    logger.LogInformation("Creating probe '{ProbeName}'...", registration.Name);

                    var probe = await registration.Factory.CreateProbeAsync(serviceProvider, registration, cancellationToken);

                    var probeOwner = new ProbeOwner(registration, probe);

                    probes.Add(probeOwner);
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Failed to create probe '{ProbeName}'", registration.Name);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Disposing {ProbeCount} probes...", probes.Count);

            foreach (var probe in probes)
            {
                try
                {
                    await probe.DisposeAsync();
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "An error ocurred while disposing probe '{ProbeName}' of type {ProbeType}", probe.Registration.Name, probe.GetType().Name);
                }
            }
        }
    }
}
