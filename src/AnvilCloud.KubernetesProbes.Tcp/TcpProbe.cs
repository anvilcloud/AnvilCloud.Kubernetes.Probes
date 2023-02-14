using System.Net;
using System.Net.Sockets;

namespace AnvilCloud.KubernetesProbes.Tcp
{
    internal class TcpProbe : IProbe
    {
        private TcpListener server;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly int port;
        private Task runTask;

        public TcpProbe(int port)
        {
            this.port = port;
        }

        public ValueTask DisposeAsync()
        {
            server.Stop();

            return ValueTask.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            //Create the server
            server = new TcpListener(IPAddress.Any, port);

            //Start the server
            server.Start();

            //Start it up!
            runTask = RunAsync(cts.Token);

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
                        //logger.LogInformation("Accepted a probe request");

                        //Nothing to do
                        client.Close();
                    }
                }
                catch (Exception)
                {
                    //logger.LogError(ex, "An error occurred during a probe attempt.");
                }
            }

            return Task.CompletedTask;
        }

        //public void Test()
        //{
        //    logger.LogInformation("Starting probe...");

        //    server = new TcpListener(System.Net.IPAddress.Any, 9000);

        //    server.Start();

        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            using (TcpClient client = server.AcceptTcpClient())
        //            {
        //                logger.LogInformation("Accepted a probe request");

        //                //Nothing to do
        //                client.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.LogError(ex, "An error occurred during a probe attempt.");
        //        }
        //    }

        //    logger.LogInformation("Probe complete.");

        //}
    }
}
