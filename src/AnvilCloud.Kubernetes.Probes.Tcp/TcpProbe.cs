using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace AnvilCloud.Kubernetes.Probes.Tcp
{
    internal class TcpProbe : IProbe
    {
        private readonly ILogger<TcpProbe> logger;
        private readonly IProbeRegistration registration;
        private readonly TcpProbeFactory factory;
        private readonly TcpListener server;

        private RunState runState;

        public TcpProbe(
            ILogger<TcpProbe> logger,
            IProbeRegistration registration,
            TcpProbeFactory factory)
        {
            this.logger = logger;
            this.registration = registration;
            this.factory = factory;
            server = new TcpListener(IPAddress.Any, factory.Port);
        }

        public IProbeRegistration Registration => registration;

        public async Task SetHealthStatusAsync(HealthStatus status, CancellationToken cancellationToken = default)
        {
            if (status >= factory.Threshold)
            {
                await StartAsync();
            }
            else
            {
                await StopAsync();
            }
        }

        private Task StartAsync()
        {
            //Check to see if we're already running
            if (runState != null)
                return Task.CompletedTask;

            logger.LogInformation("Starting TcpProbe '{ProbeName}' on port {Port}", registration.Name, factory.Port);

            runState = new RunState(RunAsync);

            //We're done for now
            return Task.CompletedTask;
        }

        private async Task StopAsync()
        {
            var runStateCopy = runState;

            if (runStateCopy == null)
                return;

            runState = null;

            logger.LogInformation("Stopping TcpProbe '{ProbeName}' on port {Port}", registration.Name, factory.Port);

            server.Stop();

            await runStateCopy.DisposeAsync();
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            //Force this to execute async
            await Task.Yield();

            server.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (TcpClient client = server.AcceptTcpClient())
                    {
                        logger.LogInformation("TcpProbe '{ProbeName}' accepted a probe request on port {Port}", registration.Name, factory.Port);

                        //Nothing to do
                        client.Close();
                    }
                }
                catch (SocketException ex)
                {
                    //These errors are likely transient and/or unimportant.
                    logger.LogTrace(ex, "A SocketError occurred during a probe attempt for '{ProbeName}'.", registration.Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during a probe attempt for '{ProbeName}'.", registration.Name);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            await StopAsync();
        }

        /// <summary>
        /// The thought here is to couple of the lifetimes of the <see cref="Task"/> and <see cref="CancellationToken"/> that are
        /// running the TcpListener accept cycle.
        /// </summary>
        private class RunState : IAsyncDisposable
        {
            private readonly CancellationTokenSource cts = new CancellationTokenSource();
            private readonly Task runTask;

            public RunState(Func<CancellationToken, Task> run)
            {
                runTask = run(cts.Token);
            }

            /// <summary>
            /// Cancels the <see cref="CancellationTokenSource"/> and waits for <see cref="Task"/> to complete.
            /// </summary>
            /// <returns></returns>
            public async ValueTask DisposeAsync()
            {
                cts.Cancel();

                await runTask
                    .ConfigureAwait(false);
            }
        }
    }
}
