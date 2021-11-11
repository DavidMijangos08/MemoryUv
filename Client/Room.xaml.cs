using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Room.xaml
    /// </summary>
    public partial class Room : Window
    { 
        UserGame usergame = new UserGame();
        MemoryServer service;
        public Room(UserGame _user)
        {
            usergame = _user;
            InitializeComponent();
            initializeListFriends();
        }

        private void initializeListFriends()
        {
            service = new MemoryServer();
            List<UserGame> friendsConnected = service.GetConnectedFriends(usergame.id);
            for (int i = 0; i < friendsConnected.Count(); i++)
            {
                string nametag = friendsConnected[i].nametag;
                listFriends.Items.Add(nametag);
            }
        }

        private void ClicExit(object sender, RoutedEventArgs e)
        {

        }

        private void ClicStartGame(object sender, RoutedEventArgs e)
        {

        }

        private void ClicAdd(object sender, RoutedEventArgs e)
        {

        }
    }
}
