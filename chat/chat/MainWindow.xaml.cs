using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace chat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient client;
        NetworkStream networkStream;
        Registration reg;
        Task task;
        string[] online;
        List<Client> OnlineClients;
        string myName;

        void MessageToServer(NetworkStream networkStream, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            networkStream.Write(buffer, 0, buffer.Length);
        }
        void LoadHistoty()
        {
            DataBaseConnection dataBaseConnection = new DataBaseConnection("localhost", "chatdb", "root", "");
            DataTable table = dataBaseConnection.MessagesFromDB(reg.userId, 30);
            int len = table.Rows.Count;
            for (int i = len - 1; i >= 0; i--)
            {
                ChatHistory.Items.Add(table.Rows[i].ItemArray[3] + ":" + table.Rows[i].ItemArray[4]);

            }
        }

        public MainWindow()
        {
            InitializeComponent();
            
            reg= new Registration();
            reg.ShowDialog();
            if (reg.userId <=0)
                Close();
            else
            {
                OnlineClients = new List<Client>();
                IPAddress ip = IPAddress.Parse(reg.IpAddres.Text);
                myName = reg.myName.Text;
                int port = 8888;
                client = new TcpClient();
                client.Connect(ip, port);
                networkStream = client.GetStream();
                MessageToServer(networkStream, "<myName>" + myName + "<myID>" + reg.userId.ToString());
                LoadHistoty();
                task = new Task(ServerListner);
                task.Start();
            }
        }

        void ServerListner()
        {
            while (client.Connected)
                if (client.Available > 0)
                {
                    byte[] buffer = new byte[1024];
                    networkStream = client.GetStream();
                    int bufferLength = networkStream.Read(buffer, 0, 1024);
                    if (bufferLength > 0)
                    {

                        string message = Encoding.UTF8.GetString(buffer);
                        if (message.IndexOf("<OnlineList>") != -1)
                        {
                            message = message.Remove(0, 12);
                            online = message.Split('|');

                            Dispatcher.Invoke(new Action(() =>
                            {
                                OnlineList.Items.Clear();
                            }));

                            for (int i = 0; i < online.Length - 1; i += 2)
                            {

                                OnlineClients.Add(new Client(Convert.ToInt32(online[i + 1]), online[i]));
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    OnlineList.Items.Add(online[i]);
                                }));
                            }
                            continue;
                        }
                        Dispatcher.Invoke(new Action(() =>
                        {
                            ChatHistory.Items.Add(message);
                        }));


                    }
                }
        }
        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            MessageToServer(networkStream, MyMessage.Text);
            MyMessage.Text = "";
        }

        private void FormClosed(object sender, EventArgs e)
        {
           reg.Close();
        }

        private void SendMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key== Key.Enter)
            {
                MessageToServer(networkStream, MyMessage.Text);
                MyMessage.Text = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OnlineList.SelectedIndex > -1)
            {
              
                MessageToServer(networkStream, "<private id:" +
                    OnlineClients[OnlineList.SelectedIndex].Id.ToString() + ">" + MyMessage.Text);
                MyMessage.Text = "";
            }
            else
            {
                MessageBox.Show("Укажите кому хотите отправить приватное сообщение");
            }
        }
    }
}
