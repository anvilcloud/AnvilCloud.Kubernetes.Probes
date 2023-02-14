using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnvilCloud.KubernetesProbes.Tcp
{
    internal class TcpProbeRegistration : IProbeRegistration
    {
        public TcpProbeRegistration(string name, int port)
        {
            Name = name;
            Port = port;
        }

        public string Name { get; }

        public int Port { get; }

        public Task<IProbe> CreateProbeAsync(IServiceProvider serviceProvider,  CancellationToken cancellationToken = default)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            var logger = loggerFactory.CreateLogger<TcpProbe>();

            return Task.FromResult<IProbe>(new TcpProbe(logger, this));
        }
    }
}
