using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();

             reg= new Registration();
            reg.ShowDialog();
            OnlineClients = new List<Client>();
            IPAddress ip = IPAddress.Parse(reg.IpAddres.Text);
            myName = reg.myName.Text;
            int port = 8888;
            client = new TcpClient();
            client.Connect(ip, port);

            networkStream = client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes("<myName>"+myName);
            networkStream.Write(buffer, 0, buffer.Length);
            Thread.Sleep(300);
            buffer = Encoding.UTF8.GetBytes("<myID>" + reg.userId.ToString());
            networkStream.Write(buffer, 0, buffer.Length);
           
            task = new Task(ServerListner);
            task.Start();

        }

        

        void ServerListner()
        {
            //OnlineList.Items.Clear();
            //OnlineList.Items.Add()
            while (client.Connected)
            if (client.Available > 0)
            {

                byte[] buffer = new byte[1024];
                    networkStream = client.GetStream();

                    int buffer_int = networkStream.Read(buffer, 0, 1024);

                    if (buffer_int > 0)
                    {
                        
                        string message = Encoding.UTF8.GetString(buffer);
                    if(message.IndexOf("<OnlineList>")!=-1)
                        {
                            message = message.Remove(0, 12);
                            online = message.Split('|');

                            Dispatcher.Invoke(new Action(() =>
                            {
                                OnlineList.Items.Clear();
                            }));

                            for(int i=0; i<online.Length-1;i+=2)
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
            networkStream = client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(MyMessage.Text);
            networkStream.Write(buffer,0,buffer.Length);
            MyMessage.Text = "";
            //Console.WriteLine("Работает?");
        }

        private void FormClosed(object sender, EventArgs e)
        {

            reg.Close();
         /*   client.Client.Disconnect(false);
            if (client != null)
            {
                client.Dispose(); //Освободим ресурсы использованные клиентом
            }
            else
            {
                client.Close(); //Закрыть соединение
            }*/

        }

        private void SendMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key== Key.Enter)
            {
                networkStream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(MyMessage.Text);
                networkStream.Write(buffer, 0, buffer.Length);
                MyMessage.Text = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OnlineList.SelectedIndex > -1)
            {
                networkStream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes("<private id:" +
                    OnlineClients[OnlineList.SelectedIndex].Id.ToString() + ">" + MyMessage.Text);
                networkStream.Write(buffer, 0, buffer.Length);
                MyMessage.Text = "";
            }
            else
            {
                MessageBox.Show("Укажите кому хотите отправить приватное сообщение");
            }
        }
    }
}
