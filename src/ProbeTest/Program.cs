using AnvilCloud.Kubernetes.Probes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await Host.CreateDefaultBuilder()
    .ConfigureLogging(loggerBuilder =>
    {
        loggerBuilder.AddConsole();
    })
    .ConfigureServices((hostContext, services) => {
        services.AddProbe("startup", new TcpProbeFactory(9000));
        services.AddProbe("readiness", new TcpProbeFactory(9001));
        services.AddProbe("liveness", new TcpProbeFactory(9002));
        services.AddHealthChecks();
        services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromSeconds(2);
            options.Period = TimeSpan.FromSeconds(10);
        });
    }).RunConsoleAsync();
