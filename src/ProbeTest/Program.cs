﻿using AnvilCloud.KubernetesProbes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await Host.CreateDefaultBuilder()
    .ConfigureLogging(loggerBuilder =>
    {
        loggerBuilder.AddConsole();
    })
    .ConfigureServices((hostContext, services) => {
        services.AddHostedService<ProbeHost>();
    }).RunConsoleAsync();
