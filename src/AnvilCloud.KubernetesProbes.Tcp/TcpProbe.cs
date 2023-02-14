using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace AnvilCloud.KubernetesProbes.Tcp
{
    internal class TcpProbe : IProbe
    {
        private readonly ILogger<TcpProbe> logger;
        private readonly IProbeRegistration registration;
        private readonly TcpListener server;

        private Task runTask;
        private CancellationTokenSource cts;

        public TcpProbe(
            ILogger<TcpProbe> logger,
            IProbeRegistration registration,
            TcpProbeFactory factory)
        {
            this.logger = logger;
            this.registration = registration;

            server = new TcpListener(IPAddress.Any, factory.Port);
        }

        public IProbeRegistration Registration => registration;

        public Task EnableAsync(CancellationToken cancellationToken = default)
        {
            //Check to see if we're already running
            if (runTask != null)
                return Task.CompletedTask;

            cts = new CancellationTokenSource();

            runTask = RunAsync(cancellationToken);
          
            //We're done for now
            return Task.CompletedTask;
        }

        public async Task DisableAsync(CancellationToken cancellationToken = default)
        {
            var runTaskCopy = runTask;

            if (runTaskCopy == null)
                return;

            server.Stop();

            runTask = null;

            cts?.Cancel();

            await runTaskCopy
                .ConfigureAwait(false);
        }


        private async Task RunAsync(CancellationToken cancellationToken = default)
        {
            await Task.Yield();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (TcpClient client = server.AcceptTcpClient())
                    {
                        logger.LogInformation("Probe '{ProbeName}' accepted a probe request", registration.Name);

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
            await DisableAsync();
        }

        ///// <summary>
        ///// This represents a Tcp Probe that is up and running.
        ///// </summary>
        //private class TcpProbeExecution
        //{
        //    private readonly CancellationTokenSource cts = new CancellationTokenSource();
        //    private readonly Task runTask;
        //    private readonly IProbeRegistration registration;
        //    private readonly TcpProbeFactory factory;
        //    private readonly ILogger logger;
        //    private readonly TcpListener server;

        //    public TcpProbeExecution(
        //        IProbeRegistration registration,
        //        TcpProbeFactory factory,
        //        ILogger logger)
        //    {
        //        logger.LogInformation("Starting probe '{ProbeName}'", registration.Name);

        //        //Create the server
        //        server = new TcpListener(IPAddress.Any, factory.Port);

        //        //Start the server
        //        server.Start();

        //        //Start it up!
        //        runTask = RunAsync(cts.Token);
        //        this.registration = registration;
        //        this.factory = factory;
        //        this.logger = logger;
        //    }

        //    private async Task RunAsync(CancellationToken cancellationToken = default)
        //    {
        //        await Task.Yield();

        //        while (!cancellationToken.IsCancellationRequested)
        //        {
        //            try
        //            {
        //                using (TcpClient client = server.AcceptTcpClient())
        //                {
        //                    logger.LogInformation("Probe '{ProbeName}' accepted a probe request", registration.Name);

        //                    //Nothing to do
        //                    client.Close();
        //                }
        //            }
        //            catch (SocketException ex)
        //            {
        //                //These errors are likely transient and/or unimportant.
        //                logger.LogTrace(ex, "A SocketError occurred during a probe attempt for '{ProbeName}'.", registration.Name);
        //            }
        //            catch (Exception ex)
        //            {
        //                logger.LogError(ex, "An error occurred during a probe attempt for '{ProbeName}'.", registration.Name);
        //            }
        //        }
        //    }

        //    public async ValueTask DisposeAsync()
        //    {
        //        //Cancel the run token so the loop doesn't try to continue.
        //        cts.Cancel();

        //        //Tell the server to stop
        //        server.Stop();

        //        //Wait for the run task to  complete.
        //        await runTask;
        //    }
        //}
    }
}
