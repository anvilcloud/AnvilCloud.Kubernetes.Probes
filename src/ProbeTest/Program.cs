using AnvilCloud.KubernetesProbes;
using AnvilCloud.KubernetesProbes.HealthChecks;
using AnvilCloud.KubernetesProbes.Tcp;
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
    }).RunConsoleAsync();
