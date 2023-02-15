using AnvilCloud.Kubernetes.Probes.Tcp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

//Placed in the "root" namespace to make discovery easier.
namespace AnvilCloud.Kubernetes.Probes
{
    /// <summary>
    /// Creates a TcpProbe.
    /// </summary>
    public class TcpProbeFactory : IProbeFactory
    {
        /// <summary>
        /// Creates an instance of the <see cref="TcpProbeFactory"/> class.
        /// </summary>
        /// <param name="port">The port on which to listen to.</param>
        public TcpProbeFactory(int port, HealthStatus threshold = HealthStatus.Healthy)
        {
            Port = port;
            Threshold = threshold;
        }

        /// <summary>
        /// The port on which to listen to.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// The threshold at which to consider the health status "good".
        /// </summary>
        public HealthStatus Threshold { get; }

        /// <inheritdoc/>
        public Task<IProbe> CreateProbeAsync(
            IServiceProvider serviceProvider,
            IProbeRegistration registration,
            CancellationToken cancellationToken = default)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            var logger = loggerFactory.CreateLogger<TcpProbe>();

            var probe = new TcpProbe(logger, registration, this);

            return Task.FromResult<IProbe>(probe);
        }
    }
}
