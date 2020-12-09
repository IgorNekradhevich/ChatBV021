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
 
        static void SendMessage(NetworkStream networkStream, string message)
        {
            byte[] buffer  = Encoding.UTF8.GetBytes(message); 
            networkStream.Write(buffer, 0, buffer.Length);
        }
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
                    int bufferLength = networkStream.Read(buffer, 0, 1024);
                    if (bufferLength > 0)
                    {
                        string clientMessage = Encoding.UTF8.GetString(buffer);
                        clientMessage = clientMessage.Remove(clientMessage.IndexOf("\0"));


                        if (clientMessage.IndexOf("<myName>") != -1)
                        {
                            client.Name = clientMessage.Remove(0, 8);
                            if (client.Name.IndexOf("<myID>") != -1)
                            {
                                string myID = client.Name.Remove(0, client.Name.IndexOf("<"));
                                client.Name = client.Name.Remove(client.Name.IndexOf("<"));
                                int index = clientsInfo.IndexOf(client);
                                client.Id = Convert.ToInt32(myID.Remove(0, 6));
                                clientsInfo[index].Id = Convert.ToInt32(myID.Remove(0, 6));
                            }
                            RefreshOnlineList();
                        }
                        else if (clientMessage.IndexOf("<private id:") != -1)
                        {

                            string privateMessage = clientMessage.Remove(0, 12);
                            privateMessage = privateMessage.Remove(privateMessage.IndexOf(">"));
                            int private_id = Convert.ToInt32(privateMessage);
                            foreach (Client tcpClient in clientsInfo)
                            {
                                if (tcpClient.Id == private_id)
                                {
                                    privateMessage = clientMessage.Remove(0, clientMessage.IndexOf(">") + 1);
                                    dataBaseConnection.MessageToDB(client.Id, private_id, client.Name, privateMessage);
                                    SendMessage(tcpClient.tcpClient.GetStream(), privateMessage);
                                }
                            }
                            // Console.WriteLine(privateMessage + "ПРИВАТНОЕ");
                        }
                        else
                        {
                            dataBaseConnection.MessageToDB(client.Id, 0, client.Name, clientMessage);

                            foreach (Client tcpClient in clientsInfo)
                            {
                                if (tcpClient.tcpClient.Connected)
                                {
                                    SendMessage(tcpClient.tcpClient.GetStream(), client.Name + " : " + clientMessage);
                                }
                            }
                            //Console.WriteLine(Encoding.UTF8.GetString(buffer));
                        }
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
           
            foreach (Client tcpClient in clientsInfo)
            {
                SendMessage(tcpClient.tcpClient.GetStream(), onlineList);
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
                //Console.WriteLine("Ура у нас Посетитель номер "+count.ToString());
                clientsInfo.Add(client);
                Task.Factory.StartNew(()=> { ListenClient(client); });
                count++;
            }
        }
    }
}
