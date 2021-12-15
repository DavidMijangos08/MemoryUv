using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Data;
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
        MemoryServer service;

        /// <summary>
        /// Constructor de la clase PreGame en donde se inicializan los diversos componentes
        /// </summary>
        /// <param name="receivedUsers" >Corresponde a la lista de usuarios de la sala</param>
        /// <param name="section"> Corresponde a la seleccion del juego</param>
        /// <param name="difficulty">Corresponde a la dificultad del juego</param>
        string language = "es-MX";
        public PreGame(List<UserGame> receivedUsers, string section, string difficulty)
        {
            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
            this.receivedUsers = receivedUsers;
            userConnected = receivedUsers[0];
            userInvited = receivedUsers[1];
            userAdmin = receivedUsers[2];
            this.section = section;
            this.difficulty = difficulty;
            try
            {
                InstanceContext context = new InstanceContext(this);
                client = new PreGameService.PreGameServiceClient(context);
                client.ConnectPlayer(userConnected.nametag, userAdmin.nametag);
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }

            if (userConnected.nametag.Equals(userAdmin.nametag))
            {
                btnStartGame.IsEnabled = true;
                usersGame.Add(userConnected);
                usersGame.Add(userInvited);
                usersGame.Add(userAdmin);
                try
                {
                    service = new MemoryServer();
                    service.UpdateUserStatus(userInvited.id, "En partida");
                    service.UpdateUserStatus(userAdmin.id, "En partida");
                }
                catch (SystemException)
                {
                    ShowExceptionAlert();
                }
            }
            else
            {
                btnStartGame.IsEnabled = false;
                usersGame.Add(userConnected);
                usersGame.Add(userAdmin);
                usersGame.Add(userAdmin);
            }
        }

        /// <summary>
        /// Método que regresa a la ventana anterior
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            try
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
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que inicia la ventana de juego
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ClicStartGame(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendConfigurationGame(userInvited.nametag, section, difficulty);
                client.SendAccessGame(userAdmin.nametag, userInvited.nametag);
                client.DisconnectPlayer("Correcto", userConnected.nametag, userInvited.nametag);
                this.Close();
                Game game = new Game(usersGame, section, difficulty);
                game.Show();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que actualiza los estados de los usuarios en sala
        /// </summary>
        /// <param name="userPreGame"> Corresponde al dicionario donde se coleccionan los usuarios de la sala preGame </param>
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

        /// <summary>
        /// Método que recibe el acceso al juego
        /// </summary>
        public void RecieveAccessGame()
        {
            try
            {
                client.DisconnectPlayer("Correcto", userConnected.nametag, userAdmin.nametag);
                Game game = new Game(usersGame, section, difficulty);
                game.Show();
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que recibe la notificacion de sala cancelada
        /// </summary>
        /// <param name="userDisconnected"> Corresponde al usuario desconectado </param>
        public void RecieveExitNotification(string userDisconnected)
        {
            string message;
            if (language.Equals("es-MX"))
            {
                message = "La sala fue cancelada. El jugador " + userDisconnected + " se desconectó";
            }
            else
            {
                message = "The room was canceled. The player " + userDisconnected + " was disconnected";
            }
            gridNotification.Visibility = Visibility.Visible;
            lbNotification.Text = message;
        }

        /// <summary>
        /// Método que acepta y avanza a la sala
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ClickAccept(object sender, RoutedEventArgs e)
        {
            try
            {
                client.DisconnectPlayer("Correcto", userConnected.nametag, userAdmin.nametag);
                this.Close();
                Home home = new Home(userConnected);
                home.Show();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que recibe la configuracion del juego
        /// </summary>
        /// <param name="section"> Corresponde a la seccion seleccionada </param>
        /// <param name="difficulty"> Corresponde a la dificultad seleccionada </param>
        public void ReceiveConfigurationGame(string section, string difficulty)
        {
            this.section = section;
            this.difficulty = difficulty;
        }

        /// <summary>
        /// Método que muestra la alerta en caso de excepción
        /// </summary>
        private void ShowExceptionAlert()
        {
            if (language.Equals("es-MX"))
            {
                MessageBox.Show("Ocurrió un error en el sistema, intente más tarde.");
            }
            else
            {
                MessageBox.Show("A system error occurred, please try again later.");
            }
            this.Close();
        }
    }
}
