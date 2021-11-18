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
        List<UserGame> receivedUsers = new List<UserGame>();
        List<UserGame> usersGame = new List<UserGame>();
        string section;
        string difficulty;
        public PreGame(List<UserGame> receivedUsers, string section, string difficulty)
        {
            InitializeComponent();
            this.receivedUsers = receivedUsers;
            userConnected = receivedUsers[0];
            userAdmin = receivedUsers[1];
            userInvited = receivedUsers[2];
            this.section = section;
            this.difficulty = difficulty;
            InstanceContext context = new InstanceContext(this);
            client = new PreGameService.PreGameServiceClient(context);
            client.ConnectPlayer(userConnected.nametag, userAdmin.nametag);
            if (userConnected.nametag.Equals(userAdmin.nametag))
            {
                btnStartGame.IsEnabled = true;
                usersGame.Add(userConnected);
                usersGame.Add(userInvited);
            }
            else
            {
                btnStartGame.IsEnabled = false;
                usersGame.Add(userConnected);
                usersGame.Add(userAdmin);
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            if (userConnected.nametag.Equals(userInvited.nametag))
            {
                client.DisconnectPlayer("Abandonado", userConnected.nametag, userAdmin.nametag);
            }
            else
            {
                client.DisconnectPlayer("Abandonado", userConnected.nametag, userInvited.nametag);
            }
            this.Close();
            Home home = new Home(userConnected);
            home.Show();
        }

        private void ClicStartGame(object sender, RoutedEventArgs e)
        {
            client.SendConfigurationGame(userInvited.nametag, section, difficulty);
            client.SendAccessGame(userAdmin.nametag, userInvited.nametag);
            client.DisconnectPlayer("Correcto", userConnected.nametag, userInvited.nametag);
            this.Close();
            Game game = new Game(usersGame, section, difficulty);
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
            client.DisconnectPlayer("Correcto", userConnected.nametag, userAdmin.nametag);
            Game game = new Game(usersGame, section, difficulty);
            game.Show();
            this.Close();
        }

        public void RecieveExitNotification(string userDisconnected)
        {
            string message = "La sala fue cancelada. El jugador " + userDisconnected + " se desconectó";
            gridNotification.Visibility = Visibility.Visible;
            lbNotification.Text = message;
        }

        private void ClickAccept(object sender, RoutedEventArgs e)
        {
            client.DisconnectPlayer("Correcto", userConnected.nametag, userAdmin.nametag);
            this.Close();
            Home home = new Home(userConnected);
            home.Show();
        }

        public void ReceiveConfigurationGame(string section, string difficulty)
        {
            this.section = section;
            this.difficulty = difficulty;
        }
    }
}
