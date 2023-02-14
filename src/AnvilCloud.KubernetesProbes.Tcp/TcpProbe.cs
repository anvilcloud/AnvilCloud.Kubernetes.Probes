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

        public async ValueTask DisposeAsync()
        {
            var serverCopy = server;

            if (serverCopy != null)
            {
                server.Stop();

                await runTask;
            }
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

        private Task RunAsync(CancellationToken cancellationToken)
        {
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
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during a probe attempt for '{ProbeName}'.", Registration.Name);
                }
            }

            return Task.CompletedTask;
        }
    }
}
