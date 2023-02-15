using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace AnvilCloud.Kubernetes.Probes
{
    internal class ProbeHealthCheckPublisher : IHealthCheckPublisher
    {
        private readonly ILogger<ProbeHealthCheckPublisher> logger;
        private readonly HealthReportMessenger messenger;

        public ProbeHealthCheckPublisher(
            ILogger<ProbeHealthCheckPublisher> logger,
            HealthReportMessenger messenger)
        {
            this.logger = logger;
            this.messenger = messenger;
        }

        public async Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            logger.LogTrace("HealthReport: {Status}", report.Status);

            await messenger.PublishHealthReportAsync(report, cancellationToken);
        }
    }
}