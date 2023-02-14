using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace AnvilCloud.KubernetesProbes
{
    public class ProbeHost : IHostedService
    {
        private readonly ILogger<ProbeHost> logger;

        private readonly IProbeRegistration[] probeRegistrations;
        private readonly IList<IProbe> probes = new List<IProbe>();

        public ProbeHost(ILogger<ProbeHost> logger, IEnumerable<IProbeRegistration> probeRegistrations)
        {
            this.logger = logger;

            probeRegistrations = probeRegistrations.ToArray();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach(var registration in probeRegistrations)
            {
                probes.Add(await registration.CreateProbeAsync(cancellationToken));
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var probe in probes)
            {
                try
                {
                    await probe.DisposeAsync();
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "An error ocurred while disposing probe {ProbeType}", probe.GetType().Name);
                }
            }
        }
    }
}
