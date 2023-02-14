using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AnvilCloud.KubernetesProbes
{
    /// <summary>
    /// Responsible for hosting <see cref="IProbe"/> implementations at runtime.
    /// </summary>
    internal class ProbeHost : IHostedService
    {
        private readonly ILogger<ProbeHost> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IProbeRegistration[] probeRegistrations;
        private readonly IList<IProbe> probes = new List<IProbe>();

        public ProbeHost(
            ILogger<ProbeHost> logger, 
            IEnumerable<IProbeRegistration> probeRegistrations,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.probeRegistrations = probeRegistrations.ToArray();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating {ProbeCount} probes...", probeRegistrations.Length);

            foreach(var registration in probeRegistrations)
            {
                try
                {

                    logger.LogInformation("Creating probe '{ProbeName}'...", registration.Name);

                    var probe = await registration.CreateProbeAsync(serviceProvider, cancellationToken);

                    await probe.StartAsync(cancellationToken);

                    probes.Add(probe);
                }
                catch(Exception ex)
                {
                    logger.LogCritical(ex, "Failed to create and start probe '{ProbeName}'", registration.Name);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Disposing probes...");

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
