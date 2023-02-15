using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AnvilCloud.Kubernetes.Probes
{
    internal class ProbeRegistration : IProbeRegistration
    {
        public ProbeRegistration(
            string name,
            IProbeFactory factory,
            Func<HealthReportEntry, bool>? predicate)
        {
            Name = name;
            Factory = factory;
            Predicate = predicate;
        }

        public IProbeFactory Factory { get; }

        public string Name { get; }

        public Func<HealthReportEntry, bool>? Predicate { get; }
    }
}
