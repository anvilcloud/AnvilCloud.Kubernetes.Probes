# AnvilCloud.KubernetesProbes
Probes for headless .NET applications

# What does it do?
Kubernetes supports [liveness, readiness and startup probes](https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/) to know that a pod is ready to acceot requests, still alive, etc. ASP.NET core has first class support for readiness probes via [health checks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks). This doesn't apply to "headless" applications (i.e. applications with an API).

This library allows .NET applications to directly support these probes without needing to spin up [Kestrel](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel).


# How to use it

# How to build it

# How to test it

# Resources
- https://stackoverflow.com/a/68069909/232566