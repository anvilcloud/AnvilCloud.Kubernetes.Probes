using AnvilCloud.Kubernetes.Probes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

var stopwatch = Stopwatch.StartNew();

await Host.CreateDefaultBuilder()
    .ConfigureLogging(loggerBuilder =>
    {
        loggerBuilder.AddConsole();
    })
    .ConfigureServices((hostContext, services) => {

        //Add a simple check that becomes unhealthy after 30 seconds.
        services.AddHealthChecks()
            .AddCheck("liveness-check", 
                      () => stopwatch.Elapsed < TimeSpan.FromSeconds(30) ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy() , 
                      tags: new[] { "liveness-tag" });

        services.AddProbe("liveness-probe", new TcpProbeFactory(9000), e => e.Tags.Contains("liveness-tag"));

        services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromSeconds(2);
            options.Period = TimeSpan.FromSeconds(10);
        });

    }).RunConsoleAsync();
