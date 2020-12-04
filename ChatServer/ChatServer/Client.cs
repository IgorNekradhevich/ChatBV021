using System.Net;
using System.Net.Sockets;

namespace chat
{
    class Client
    {
       // public  IPAddress IPAddress { get; set; }
        //public int Port { get; set; }
        public string Name{ get; set; }
        public int Id{ get; set; }
        public TcpClient tcpClient { get; set; }
       // Color color = Color.FromRgb(0, 0, 0);

        public Client (string name, int id, TcpClient tcpClient)
        {
            Id = id;
            this.Name = name;
            this.tcpClient = tcpClient;
        }

    }
}
