using chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ChatServer
{
    class Program
    {
       static List<Client> clientsInfo;
       static void ListenClient(Client client)
        {
            bool isOnine = true;
            DataBaseConnection dataBaseConnection = new DataBaseConnection("localhost", "chatdb", "root", "");
            while (isOnine)
            {
                
                NetworkStream networkStream = client.tcpClient.GetStream();
                byte[] buffer = new byte[1024];
                try
                {
                    int buffer_int = networkStream.Read(buffer, 0, 1024);
                    if (buffer_int > 0)
                    {
                        string temp = Encoding.UTF8.GetString(buffer);
                       temp= temp.Remove(temp.IndexOf("\0"));
                        if (temp.IndexOf("<myName>") != -1)
                        {
                            //IGOR<myID>2
                            client.Name = temp.Remove(0, 8);
                            if (client.Name.IndexOf("<myID>") != -1)
                            {
                                string myID = client.Name.Remove(0, client.Name.IndexOf("<"));
                                client.Name = client.Name.Remove(client.Name.IndexOf("<"));
                                int num = clientsInfo.IndexOf(client);
                                client.Id = Convert.ToInt32(myID.Remove(0, 6));
                                clientsInfo[num].Id = Convert.ToInt32(myID.Remove(0, 6));
                            }
                            // client.Name = client.Name.Remove(client.Name.IndexOf("\0"));
                            RefreshOnlineList();
                            continue;
                        }
                      
                        if (temp.IndexOf("<private id:") != -1)
                        {
                            
                            string buffer_private = temp.Remove(0, 12);
                            buffer_private = buffer_private.Remove(buffer_private.IndexOf(">"));
                            int private_id = Convert.ToInt32(buffer_private);
                            
                            foreach (Client tcpClient in clientsInfo)
                            {
                                if ( tcpClient.Id== private_id)
                                {
                                    NetworkStream ClientNetworkStream = tcpClient.tcpClient.GetStream();
                                    buffer_private = temp.Remove(0,temp.IndexOf(">") + 1);

                                    dataBaseConnection.MessageToDB(client.Id, private_id, client.Name, buffer_private);
                                    
                                    buffer = Encoding.UTF8.GetBytes(buffer_private);
                                    ClientNetworkStream.Write(buffer, 0, buffer.Length);
                                }
                            }
                            Console.WriteLine(buffer_private + "ПРИВАТНОЕ");
                            continue;
                        }

                        buffer = Encoding.UTF8.GetBytes(client.Name + " : " + temp);
                        
                        dataBaseConnection.MessageToDB(client.Id, 0, client.Name, temp);
                        
                        foreach (Client tcpClient in clientsInfo)
                        {
                            if (tcpClient.tcpClient.Connected)
                            {
                                NetworkStream ClientNetworkStream = tcpClient.tcpClient.GetStream();
                                ClientNetworkStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        Console.WriteLine(Encoding.UTF8.GetString(buffer));
                    }
                }
                catch (Exception e)
                {
                    isOnine = false;
                    clientsInfo.Remove(client);
                    RefreshOnlineList();
                    Console.WriteLine(e.Message);
                }

            }
        }
        static void RefreshOnlineList()
        {
            string onlineList = "<OnlineList>";
            foreach(Client client in clientsInfo)
            {
                onlineList+=client.Name+$"|{client.Id.ToString()}|";
            }
            byte[] buffer = new byte[1024];
            buffer = Encoding.UTF8.GetBytes(onlineList);

            foreach (Client tcpClient in clientsInfo)
            {
                NetworkStream ClientNetworkStream = tcpClient.tcpClient.GetStream();
                ClientNetworkStream.Write(buffer, 0, buffer.Length);
                //<OnlineList>Игорь| Ольга| Ваня|
            }
        }
        static void Main(string[] args)
        {
            int count=1;
            clientsInfo = new List<Client>();
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);
            tcpListener.Start();

            while(true)
            {

                Client client = new Client("User", count, tcpListener.AcceptTcpClient());
                Console.WriteLine("Ура у нас Посетитель номер "+count.ToString());
                
                clientsInfo.Add(client);
               
                Task.Factory.StartNew(()=> { ListenClient(client); });
                count++;
            }
        
        }

      

    }
}
