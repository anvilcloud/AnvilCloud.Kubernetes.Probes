using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AnvilCloud.Kubernetes.Probes
{
    /// <summary>
    /// An instance of a probe.
    /// </summary>
    public interface IProbe : IAsyncDisposable
    {
        /// <summary>
        /// Gets the registration that was used to create this probe.
        /// </summary>
        IProbeRegistration Registration { get; }

        /// <summary>
        /// Sets the health status of the probe given its configured predicate.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SetHealthStatusAsync(HealthStatus status, CancellationToken cancellationToken = default);
    }
}
