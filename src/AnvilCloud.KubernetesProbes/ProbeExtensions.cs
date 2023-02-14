using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace AnvilCloud.KubernetesProbes
{
    public static class ProbeExtensions
    {
        /// <summary>
        /// This is typically only called by implementors of  <see cref="IProbe"/> and <see cref="IProbeRegistration"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterProbe(this IServiceCollection services, IProbeRegistration probeRegistration)
        {
            //This might get called a number of times, so make it idemopotent
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, ProbeHost>());

            services.Add(ServiceDescriptor.Singleton<IProbeRegistration>(probeRegistration));

            return services;
        }
    }
}
