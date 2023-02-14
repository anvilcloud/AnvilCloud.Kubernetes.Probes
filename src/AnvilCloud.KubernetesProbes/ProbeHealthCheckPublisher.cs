using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace AnvilCloud.KubernetesProbes.HealthChecks
{
    public class ProbeHealthCheckPublisher : IHealthCheckPublisher
    {
        private readonly ILogger<ProbeHealthCheckPublisher> logger;

        public ProbeHealthCheckPublisher(ILogger<ProbeHealthCheckPublisher> logger)
        {
            this.logger = logger;
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            logger.LogInformation("HealthReport: {Status}", report.Status);

            return Task.CompletedTask;
        }
    }

    //public interface IProbeHealthCheckSubscriber
    //{
    //    event EventHandler<EventArgs> Healthy;

    //    event EventHandler<EventArgs> Unhealthy;
    //}
}