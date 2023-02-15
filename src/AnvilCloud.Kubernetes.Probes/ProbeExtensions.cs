using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace AnvilCloud.Kubernetes.Probes
{
    /// <summary>
    /// Provides extension methods for adding Kubernetes probes to in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ProbeExtensions
    {
        /// <summary>
        /// Called to add a probe to the dependency injection environment.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name">The name of the probe.</param>
        /// <param name="probeFactory">Specify null to consider all health reports.</param>
        /// <param name="predicate">Used to filter which <see cref="HealthReportEntry"/> to consider for a given probe.</param>
        /// <returns></returns>
        public static IServiceCollection AddProbe(
            this IServiceCollection services,
            string name,
            IProbeFactory probeFactory,
            Func<HealthReportEntry, bool>? predicate = null)
        {
            //Add the necessary services to make probes work.
            //This might get called a number of times, so make it idemopotent
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, ProbeHost>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHealthCheckPublisher, ProbeHealthCheckPublisher>());
            services.TryAdd(ServiceDescriptor.Singleton<HealthReportMessenger, HealthReportMessenger>());

            //Create the probe registration
            var probeRegistration = new ProbeRegistration(name, probeFactory, predicate);

            //Add it to the sevices
            services.Add(ServiceDescriptor.Singleton<IProbeRegistration>(probeRegistration));

            return services;
        }
    }
}
