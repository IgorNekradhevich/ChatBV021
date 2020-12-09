using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace chat
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public int userId { get; set; }
     
        public Registration()
        {
            InitializeComponent();
            userId = 0;
        }
      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataBaseConnection dataBaseConnection = new DataBaseConnection("localhost", "chatdb", "root", "");
            userId= dataBaseConnection.GetUserId(myLogin.Text, myPass.Password);
            if (userId > 0)
            {
                Close();
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль");
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NewUser newUser = new NewUser();
            newUser.ShowDialog();
        }
    }
}
