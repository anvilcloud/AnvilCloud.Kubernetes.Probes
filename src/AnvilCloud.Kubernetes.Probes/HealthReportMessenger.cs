using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AnvilCloud.Kubernetes.Probes
{
    /// <summary>
    /// This exists to resolve a circular dependency between <see cref="ProbeHost"/> and <see cref="ProbeHealthCheckPublisher"/>.
    /// They depend on each other, so this skirts that issue in an ugly way.
    /// </summary>
    internal class HealthReportMessenger
    {
        private ProbeHost probeHost;

        internal void SetProbeHost(ProbeHost probeHost)
        {
            if (this.probeHost != null)
                throw new InvalidOperationException($"The {nameof(ProbeHost)} has already been set.");

            this.probeHost = probeHost;
        }

        internal async Task PublishHealthReportAsync(HealthReport report, CancellationToken cancellationToken)
        {
            if (probeHost == null)
                throw new InvalidOperationException($"The {nameof(ProbeHost)} has not been set.");

            await probeHost.OnHealthReportAsync(report, cancellationToken);
        }
    }
}
