namespace AnvilCloud.KubernetesProbes
{
    /// <summary>
    /// An instance of a probe.
    /// </summary>
    public interface IProbe : IAsyncDisposable
    {
        Task StartAsync(CancellationToken cancellationToken = default);
    }
}
