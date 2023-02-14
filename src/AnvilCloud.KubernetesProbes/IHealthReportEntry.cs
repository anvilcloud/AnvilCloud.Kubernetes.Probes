using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AnvilCloud.KubernetesProbes
{
    /// <summary>
    /// Represents an entry in a <see cref="HealthReport"/>. Corresponds to the result of a single <see cref="IHealthCheck"/>.
    /// </summary>
    public interface IHealthReportEntry
    {
        /// <summary>
        /// Gets additional key-value pairs describing the health of the component.
        /// </summary>
        public IReadOnlyDictionary<string, object> Data { get; }

        /// <summary>
        /// Gets a human-readable description of the status of the component that was checked.
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// Gets the health status of the component that was checked.
        /// </summary>
        HealthStatus Status { get; }

        /// <summary>
        /// Gets the tags associated with the health check.
        /// </summary>
        IEnumerable<string> Tags { get; }
    }
}
