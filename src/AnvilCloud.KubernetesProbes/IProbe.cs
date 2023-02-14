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
        /// Starts the probe listening to probe requests.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task EnableAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops the probe from listening to probe requests.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DisableAsync(CancellationToken cancellationToken = default);
    }
}
