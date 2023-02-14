namespace AnvilCloud.KubernetesProbes.Tcp
{
    internal class TcpProbeRegistration : IProbeRegistration
    {
        public Task<IProbe> CreateProbeAsync(CancellationToken cancellationToken = default)
        {
            return new TcpProbe();
        }
    }
}
