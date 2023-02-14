using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnvilCloud.KubernetesProbes.Tcp
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
        public TcpProbeFactory(int port)
        {
            Port = port;
        }

        /// <summary>
        /// The port on which to listen to.
        /// </summary>
        public int Port { get; }

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
