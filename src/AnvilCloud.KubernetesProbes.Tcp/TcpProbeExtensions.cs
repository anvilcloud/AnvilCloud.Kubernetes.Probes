using AnvilCloud.KubernetesProbes.Tcp;
using Microsoft.Extensions.DependencyInjection;

namespace AnvilCloud.KubernetesProbes
{
    public static class TcpProbeExtensions
    {
        /// <summary>
        /// Adds a TCP Probe.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IServiceCollection AddTcpProbe(this IServiceCollection services,  string name, int port)
        {
            services.RegisterProbe(new TcpProbeRegistration(name, port));

            return services;
        }
    }
}
