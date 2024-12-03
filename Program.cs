using System.Net;
using System.Net.Sockets;

namespace Annulaire_Client
{
    class Client
    {
        private static Socket socketClient;
        private static bool keepRunning = true;

        static async Task Main(string[] args)
        {
            // Define the server IP and port to connect to
            string serverAddress = "127.0.0.1"; // Change this to your server's IP address
            int port = 3434;

            // Create the socket client
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Connect to the server
                await socketClient.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverAddress), port));
                Console.WriteLine("Connected to the server.");
                
                Menu menu = new Menu(socketClient);
                
                // Start receiving data from the server in a separate task
                Task receiveTask = Task.Run(() => ReceiveData(menu));
                
                menu.MenuPrincipal();

                // Close the socket
                socketClient.Close();
                Console.WriteLine("Connection closed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadLine();
        }

        // This task handles receiving messages from the server
        private static void ReceiveData(Menu menu)
        {
            ReceiveController rContr = new ReceiveController();
            byte[] buffer;
            int bytesReceived;
            while (keepRunning)
            {
                try
                {
                    buffer = new byte[socketClient.ReceiveBufferSize];
                    bytesReceived = socketClient.Receive(buffer);
                    if (bytesReceived > 0)
                    {
                        Paquet p = new Paquet(buffer);
                        rContr.HandleReceivedMessage(p, menu);
                    }
                }
                catch (SocketException)
                {
                    if (!keepRunning) break;  // Exit the loop if socket is closed
                    Console.WriteLine("Connection lost.");
                    break;
                }
            }
        }
    }
}
