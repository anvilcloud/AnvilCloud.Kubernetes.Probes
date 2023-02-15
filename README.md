# AnvilCloud.KubernetesProbes
Probes for headless .NET applications

# What does it do?
Kubernetes supports [liveness, readiness and startup probes](https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/) to know that a pod is ready to acceot requests, still alive, etc. ASP.NET core has first class support for readiness probes via [health checks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks). This doesn't apply to "headless" applications (i.e. applications with an API).

This library allows .NET applications to directly support these probes without needing to spin up [Kestrel](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel).


[![.github/workflows/github-publish-nuget.yaml](https://github.com/anvilcloud/AnvilCloud.Kubernetes.Probes/actions/workflows/github-publish-nuget.yaml/badge.svg?branch=release)](https://github.com/anvilcloud/AnvilCloud.Kubernetes.Probes/actions/workflows/github-publish-nuget.yaml)

# How to use it

Install the package: 

```
AnvilCloud.Kubernetes.Probes.Tcp
```

Add the following code:

```c#
await Host.CreateDefaultBuilder()
    .ConfigureLogging(loggerBuilder =>
    {
        loggerBuilder.AddConsole();
    })
    .ConfigureServices((hostContext, services) => {

        services.AddHealthChecks()
            //TODO: Add meaningful checks
            .AddCheck("liveness-check", () => HealthCheckResult.Healthy());
            
        //Adds a TCP probe on port 9000.
        services.AddProbe("liveness-probe", new TcpProbeFactory(9000));

        //This speeds up the health checks from the default values. 
        services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromSeconds(2);
            options.Period = TimeSpan.FromSeconds(10);
        });

    }).RunConsoleAsync();
```

# Advanced Usage
Note that probes can be configured to only consider certain health reports. Here is an example that does just that:

```c#
await Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) => {

        services.AddHealthChecks()
            .AddCheck("liveness-check", () => HealthCheckResult.Healthy(), new[] { "liveness-tag" });
            .AddCheck("readiness-check", () => HealthCheckResult.Healthy(), new[] { "readiness-tag" });
            
        //Adds a TCP probe on port 9000.
        services.AddProbe("liveness-probe", new TcpProbeFactory(9000), e => e.Tags.Contains("liveness-tag"));
        services.AddProbe("readiness-probe", new TcpProbeFactory(9001), e => e.Tags.Contains("readingless-tag"));

    }).RunConsoleAsync();
```

# Kubernetes Example

Assumptions: 
- You are running kubernetes locally

Example yaml: [probe-test.yaml](kubernetes/probe-test.yaml)

## Building the example container

From the [./scripts](./scripts) directory

```
./build.ps1
```

## Running the example pod

From the [./kubernetes](./kubernetes) directory

```
kubectl apply -f ./probe-test.yaml
```

**NOTE:** The example container is designed to stop responding to the liveness probe after approximately 30 seconds. This will cause the pod to exit and get recreated after the probe has failed enough times.

To watch the output:

```
 kubectl logs -f probe-test
```

To delete the test pod:

```
 kubectl delete -f ./probe-test.yaml
```
# Further Reading
- More information on health checks: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks
- Kubernetes probes: https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/