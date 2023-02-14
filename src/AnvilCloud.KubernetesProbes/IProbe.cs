namespace AnvilCloud.KubernetesProbes
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
        /// Starts the probe.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task StartAsync(CancellationToken cancellationToken = default);
    }
}
