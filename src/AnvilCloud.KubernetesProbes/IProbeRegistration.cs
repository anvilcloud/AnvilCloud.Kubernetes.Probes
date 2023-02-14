namespace AnvilCloud.KubernetesProbes
{
    public interface IProbeRegistration
    {
        Task<IProbe> CreateProbeAsync(CancellationToken cancellationToken = default);
    }
}
