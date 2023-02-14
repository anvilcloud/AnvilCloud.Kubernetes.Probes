namespace AnvilCloud.KubernetesProbes
{
    /// <summary>
    /// Registration information for a probe.
    /// </summary>
    public interface IProbeRegistration
    {
        /// <summary>
        /// Creates the probe. This will be called once per lifetime of this object.
        /// </summary>
        /// <param name="serviceProvider">Passed in to allow dependency injection.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IProbe> CreateProbeAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the name of the registration.
        /// </summary>
        string Name { get; }
    }
}
