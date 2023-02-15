namespace AnvilCloud.Kubernetes.Probes
{
    /// <summary>
    /// Responsible for coupling the lifetime and relationship between an <see cref="IProbe"/> instance 
    /// and the <see cref="IProbeRegistration"/> that was used to create it.
    /// </summary>
    internal class ProbeOwner : IAsyncDisposable
    {
        public ProbeOwner(IProbeRegistration registration, IProbe probe)
        {
            Registration = registration;
            Probe = probe;
        }

        public IProbeRegistration Registration { get; }

        public IProbe Probe { get; }

        public async ValueTask DisposeAsync()
        {
            await Probe.DisposeAsync();
        }
    }
}
