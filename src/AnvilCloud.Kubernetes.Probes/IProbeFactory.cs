namespace AnvilCloud.Kubernetes.Probes
{
    /// <summary>
    /// Responsible for creating an <see cref="IProbe"/> instance.
    /// </summary>
    public interface IProbeFactory
    {
        /// <summary>
        /// Creates the probe. This will be called once per lifetime of this object.
        /// </summary>
        /// <param name="serviceProvider">Passed in to allow dependency injection.</param>
        /// <param name="registration">The registration that is creating the probe.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IProbe> CreateProbeAsync(
            IServiceProvider serviceProvider,
            IProbeRegistration registration,
            CancellationToken cancellationToken = default);
    }
}
