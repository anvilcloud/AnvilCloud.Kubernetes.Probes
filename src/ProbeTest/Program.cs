using AnvilCloud.KubernetesProbes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await Host.CreateDefaultBuilder()
    .ConfigureLogging(loggerBuilder =>
    {
        loggerBuilder.AddConsole();
    })
    .ConfigureServices((hostContext, services) => {
        services.AddTcpProbe("startup", 9000);
        services.AddTcpProbe("readiness", 9001);
        services.AddTcpProbe("liveness", 9002);
    }).RunConsoleAsync();
