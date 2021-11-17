using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Lógica de interacción para PreGame.xaml
    /// </summary>
    public partial class PreGame : Window, PreGameService.IPreGameServiceCallback
    {
        UserGame userConnected = new UserGame();
        UserGame userAdmin = new UserGame();
        UserGame userInvited = new UserGame();
        public PreGameService.PreGameServiceClient client;
        List<String> users = new List<String>();
        public PreGame(UserGame userGameConnected, UserGame userGameInvited, UserGame userGameAdmin)
        {
            InitializeComponent();
            userConnected = userGameConnected;
            userAdmin = userGameAdmin;
            userInvited = userGameInvited;
            InstanceContext context = new InstanceContext(this);
            client = new PreGameService.PreGameServiceClient(context);
            client.ConnectPlayer(userConnected.nametag, userAdmin.nametag);
            if (userGameConnected.nametag.Equals(userGameAdmin.nametag))
            {
                btnStartGame.IsEnabled = true;
            }
            else
            {
                btnStartGame.IsEnabled = false;
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            if (userConnected.nametag.Equals(userInvited.nametag))
            {
                client.DisconnectPlayer(userConnected.nametag, userAdmin.nametag);
            }
            else
            {
                client.DisconnectPlayer(userConnected.nametag, userInvited.nametag);
            }
            this.Close();
            Home home = new Home(userConnected);
            home.Show();
        }

        private void ClicStartGame(object sender, RoutedEventArgs e)
        {
            client.SendAccessGame(userAdmin.nametag, userConnected.nametag);
            this.Close();
            Game game = new Game(userConnected);
            game.Show(); 
        }

        public void UpdateUsersRoom(Dictionary<object, string>.ValueCollection usersPreGame)
        {
            lbUsergame1.Text = "";
            lbUsergame2.Text = "";
            users.Clear();
            foreach (string user in usersPreGame)
            {
                users.Add(user);
            }

            lbUsergame1.Text = users[0];
            if(users.Count() > 1)
            {
                lbUsergame2.Text = users[1];
            }
        }

        public void RecieveAccessGame()
        {
            this.Close();
            Game game = new Game(userConnected);
            game.Show();
        }

        public void RecieveExitNotification(string userDisconnected)
        {
            string message = "La sala fue cancelada. El jugador " + userDisconnected + " se desconectó";
            gridNotification.Visibility = Visibility.Visible;
            lbNotification.Text = message;
        }

        private void ClickAccept(object sender, RoutedEventArgs e)
        {
            this.Close();
            Home home = new Home(userConnected);
            home.Show();
        }
    }
}
