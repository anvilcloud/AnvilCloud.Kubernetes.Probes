using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AnvilCloud.Kubernetes.Probes
{
    /// <summary>
    /// Registration information for a probe.
    /// </summary>
    public interface IProbeRegistration
    {
        /// <summary>
        /// The factory that will create the probe at runtime.
        /// </summary>
        IProbeFactory Factory { get; }

        /// <summary>
        /// Gets the name of the registration.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Selects which health report items to consider for this probe.
        /// </summary>
        Func<HealthReportEntry, bool> Predicate { get; }
    }
}
