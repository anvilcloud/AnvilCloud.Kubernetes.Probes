namespace AnvilCloud.KubernetesProbes
{
    internal class ProbeRegistration : IProbeRegistration
    {
        public ProbeRegistration(
            string name, 
            IProbeFactory factory, 
            Func<IHealthReportEntry, bool> healthReportFilter)
        {
            Name = name;
            Factory = factory;
            HealthReportFilter = healthReportFilter ?? (_ => true);
        }

        public IProbeFactory Factory { get; }

        public string Name { get; }

        public Func<IHealthReportEntry, bool> HealthReportFilter { get; }
    }
}
