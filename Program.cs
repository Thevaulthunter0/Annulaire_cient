using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Annulaire_Client.Paquet;

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

                Paquet paquetDec = new Paquet(0, 0, TypePaquet.Deconnexion, new List<List<String>>(), false);
                byte[] buffer = paquetDec.bytes();
                await socketClient.SendAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

                // Close the socket and cleanup
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
                        HandleReceivedMessage(p, menu);
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

        private static void HandleReceivedMessage(Paquet paquet, Menu menu)
        {
            ReceiveController rContr = new ReceiveController();
            switch (paquet.type)
            {
                case TypePaquet.Connexion:
                    if (paquet.boolInfo == true)
                    {
                        rContr.PrintConnexionSucces();
                        menu.setIsAdmin(true);
                    }
                    else
                    {
                        rContr.PrintConnexionFailed(); 
                        menu.setIsAdmin(false);
                    }
                    break;

                case TypePaquet.Demande:
                    switch (paquet.intInfo)
                    {
                        case 1:
                            if (paquet.boolInfo == true)
                            {
                                rContr.PrintMembre(paquet.donnee);
                            }
                            else { rContr.PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 2:
                            if (paquet.boolInfo == true)
                            {
                                rContr.PrintMembre(paquet.donnee);
                            }
                            else { rContr.PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 3:
                            if (paquet.boolInfo == true)
                            {
                                rContr.PrintMembre(paquet.donnee);
                            }
                            else { rContr.PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 4:
                            if (paquet.boolInfo == true)
                            {
                                rContr.PrintDemandeSucces("Succes, le membre a bien été ajouté");
                            }
                            else { rContr.PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 5:
                            if (paquet.boolInfo == true)
                            {
                                rContr.PrintDemandeSucces("Succes, le membre a bien été supprimé.");
                            }
                            else { rContr.PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 6:
                            if (paquet.boolInfo == true)
                            {
                                rContr.PrintDemandeSucces("Succes, le membre a bien été modifié.");
                            }
                            else { rContr.PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 7:
                            if (paquet.boolInfo == true)
                            {
                                rContr.PrintDemandeSucces("Succes, si le numéro existe le membre a bien été mis sur la liste rouge.");
                            }
                            else { rContr.PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                        case 8:
                            if (paquet.boolInfo == true)
                            {
                                rContr.PrintDemandeSucces("Succes, si le numéro existe le membre a bien été enlever de la liste rouge.");
                            }
                            else { rContr.PrintDemandeFailed(paquet.donnee[0][0]); }
                            break;
                    }
                    break;
            }
        }
    }
}
