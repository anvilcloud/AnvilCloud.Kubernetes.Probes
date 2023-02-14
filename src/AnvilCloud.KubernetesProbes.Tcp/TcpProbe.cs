using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace AnvilCloud.KubernetesProbes.Tcp
{
    internal class TcpProbe : IProbe
    {
        private TcpListener server;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly ILogger<TcpProbe> logger;
        private readonly TcpProbeRegistration registration;
        private Task runTask;

        public string Name { get; }

        public IProbeRegistration Registration => registration;

        public TcpProbe(
            ILogger<TcpProbe> logger, 
            TcpProbeRegistration registration)
        {
            this.logger = logger;
            this.registration = registration;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Starting probe '{ProbeName}'", Registration.Name);

            //Create the server
            server = new TcpListener(IPAddress.Any, registration.Port);

            //Start the server
            server.Start();

            //Start it up!
            runTask = RunAsync(cts.Token);

            //We're done for now
            return Task.CompletedTask;
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            while(!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (TcpClient client = server.AcceptTcpClient())
                    {
                        logger.LogInformation("Probe '{ProbeName}' accepted a probe request", Registration.Name);

                        //Nothing to do
                        client.Close();
                    }
                }
                catch(SocketException ex)
                {
                    //These errors are likely transient and/or unimportant.
                    logger.LogTrace(ex, "A SocketError occurred during a probe attempt for '{ProbeName}'.", Registration.Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during a probe attempt for '{ProbeName}'.", Registration.Name);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            //Cancel the run token so the loop doesn't try to continue.
            cts.Cancel();

            var serverCopy = server;

            if (serverCopy != null)
            {
                //Tell the server to stop
                server.Stop();

                //Wait for the run task to  complete.
                await runTask;
            }
        }
    }
}
