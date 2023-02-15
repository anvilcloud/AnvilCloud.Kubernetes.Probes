using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AnvilCloud.Kubernetes.Probes
{
    internal static class HealthReportExtensions
    {
        internal static HealthStatus GetEffectiveHealthStatus(this HealthReport report, Func<HealthReportEntry, bool>? predicate)
        {
            //If there is no filter, just return the overall status.
            if (predicate == null)
                return report.Status;

            var filtered = report.Entries.Values
                .Where(predicate);

            return CalculateAggregateStatus(filtered);
        }

        /// <summary>
        /// This is copied directly from <see cref="HealthReport"/>.
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        private static HealthStatus CalculateAggregateStatus(IEnumerable<HealthReportEntry> entries)
        {
            // This is basically a Min() check, but we know the possible range, so we don't need to walk the whole list
            var currentValue = HealthStatus.Healthy;
            foreach (var entry in entries)
            {
                if (currentValue > entry.Status)
                {
                    currentValue = entry.Status;
                }

                if (currentValue == HealthStatus.Unhealthy)
                {
                    // Game over, man! Game over!
                    // (We hit the worst possible status, so there's no need to keep iterating)
                    return currentValue;
                }
            }

            return currentValue;
        }
    }
}
