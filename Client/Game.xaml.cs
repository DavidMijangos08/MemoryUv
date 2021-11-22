using Data;
using Host;
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
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Game.xaml
    /// </summary>
    public partial class Game : Window, GameService.IGameServiceCallback
    {
        UserGame usergame = new UserGame();
        MemoryServer service;
        public GameService.GameServiceClient client;
        UserGame userConnected = new UserGame();
        UserGame userOpponent = new UserGame();
        DispatcherTimer dispatcherTimer;
        int decrement;

        public Game(List<UserGame> users, string section, string difficulty)
        {
            InitializeComponent();
            InstanceContext context = new InstanceContext(this);
            client = new GameService.GameServiceClient(context);
            userConnected = users[0];
            userOpponent = users[1];
            UserGame userAdmin = users[2];
            client.ConnectToGame(userConnected.nametag, userOpponent.nametag);
            if (userConnected.nametag.Equals(userAdmin.nametag))
            {
                InitializeTimer();
            }
            lbUserTurn.Text = userAdmin.nametag;
        }
        
        private void InitializeTimer()
        {
            lbUserTurn.Text = userConnected.nametag;
            decrement = 10;
            dispatcherTimer = new DispatcherTimer();
            lbTimer.Text = "10";
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimerTicker;
            dispatcherTimer.Start();    
        }

        

        private void DispatcherTimerTicker(object sender, EventArgs e)
        {
            decrement--;
            lbTimer.Text = decrement.ToString();
            if (lbTimer.Text.Equals("0"))
            {
                dispatcherTimer.Stop();
                client.SendGameTurn(userOpponent.nametag);
                lbUserTurn.Text = userOpponent.nametag;
            }
        }

        private void changeTextBlock(int lb)
        {
            if (lb == 0)
            {
                //lbUserTurn.Text = userSelected.nametag;
            }
            if (lb == 1)
            {
                lbUserTurn.Text = usergame.nametag;
            }
        }

        public void ReceiveExitNotification(string userDisconnected)
        {
            string message = "El jugador " + userDisconnected + " se desconectó. Tu ganas la partida";
            gridNotification.Visibility = Visibility.Visible;
            lbNotification.Text = message;
        }

        public void ReceiveGameTurn()
        {
            InitializeTimer();
        }

        private void BackingOutClick(object sender, RoutedEventArgs e)
        {
            client.DisconnectTheGame("Abandonado", userConnected.nametag, userOpponent.nametag);
            this.Close();
            Home home = new Home(userConnected);
            home.Show();
        }

        private void ClickAcceptNotification(object sender, RoutedEventArgs e)
        {
            client.DisconnectTheGame("Finalizado", userConnected.nametag, userOpponent.nametag);
            this.Close();
            Home home = new Home(userConnected);
            home.Show();
        }
    }
}
