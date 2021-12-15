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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Window, RoomService.IRoomServiceCallback
    {
        public string username;
        UserGame usergame = new UserGame();
        MemoryServer service;
        public RoomService.RoomServiceClient client;
        string usergameApplicant;
        string language = "es-MX";

        /// <summary>
        /// Constructor de la clase Home en donde se inicializan los diversos componentes
        /// </summary>
        public Home(UserGame _user)
        {
            InitializeComponent();
            username = _user.nametag;
            lbUsername.Text = "" + username;
            usergame = _user;
            language = Properties.Settings.Default.languageCode;
            try
            {
                InstanceContext context = new InstanceContext(this);
                client = new RoomService.RoomServiceClient(context);
                client.ConnectWaitingRoom(usergame.nametag);
                service = new MemoryServer();
                service.UpdateUserStatus(usergame.id, "Activo");
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(_user.id))));
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }          
        }

        /// <summary>
        /// Método que abre la ventana de chat
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ChatClick(object sender, RoutedEventArgs e)
        {
            try
            {
                client.DisconnectRoom(usergame.nametag);
                Chat windowChat = new Chat(username);
                windowChat.Show();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que finaliza del programa
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            try
            {
                service = new MemoryServer();
                service.UpdateUserStatus(usergame.id, "Inactivo");
                client.DisconnectRoom(usergame.nametag);
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que abre la ventana de configracion
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                client.DisconnectRoom(usergame.nametag);
                Settings settings = new Settings(usergame);
                settings.Show();
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que abre la ventana de logros
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ArchievementClick(object sender, RoutedEventArgs e)
        {
            try
            {
                client.DisconnectRoom(usergame.nametag);
                Logros logro = new Logros(usergame);
                logro.Show();
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que abre la ventana de amigos
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void FriendsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                client.DisconnectRoom(usergame.nametag);
                Friends windowFriends = new Friends(usergame);
                windowFriends.Show();
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que abre la ventana de ranking
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void RankingClick(object sender, RoutedEventArgs e)
        {
            try
            {
                client.DisconnectRoom(usergame.nametag);
                Ranking ranking = new Ranking(usergame);
                ranking.Show();
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que abre la ventana de Sala
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void RoomClick(object sender, RoutedEventArgs e)
        {
            try
            {
                client.DisconnectRoom(usergame.nametag);
                Room room = new Room(usergame);
                room.Show();
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que abre la ventana de personalizar
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void PersonalizeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                client.DisconnectRoom(usergame.nametag);
                Personalize personalize = new Personalize(usergame);
                personalize.Show();
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que acepta la invitacion para unierse a una sala
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ClickAccept(object sender, RoutedEventArgs e)
        {
            try
            {
                gridInvitation.Visibility = Visibility.Collapsed;
                client.SendAcceptance(this.usergameApplicant, usergame.nametag);
                service = new MemoryServer();
                List<UserGame> userAdmin = service.GetUsersByInitialesOfNametag(this.usergameApplicant);
                List<UserGame> usersToSend = new List<UserGame>(4);
                usersToSend.Add(usergame);
                usersToSend.Add(usergame);
                usersToSend.Add(userAdmin[0]);
                PreGame pregame = new PreGame(usersToSend, "", "");
                pregame.Show();
                client.DisconnectRoom(usergame.nametag);
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que rechaza la solicitud de uniersea una sala
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ClicRejeact(object sender, RoutedEventArgs e)
        {
            gridInvitation.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Método que recibe la invitacion para unirse a una sala
        /// </summary>
        /// <param name="usergameApplicant"> Corresponde al nombre del usuario </param>
        public void RecieveInvitation(string usergameApplicant)
        {
            this.usergameApplicant = usergameApplicant;
            string messageInvitation;
            if (language.Equals("es-MX"))
            {
                messageInvitation = "El usuario " + usergameApplicant + " te está invitando a su sala";
            }
            else
            {
                messageInvitation = "The user " + usergameApplicant + " is inviting you to his room";
            }
            gridInvitation.Visibility = Visibility.Visible;
            lbInvitation.Text = messageInvitation;
        }

        /// <summary>
        /// Método que recibe la respuesta del jugador 
        /// </summary>
        public void RecieveAnswer()
        {
            try
            {
                List<UserGame> usersToSend = new List<UserGame>(4);
                usersToSend.Add(usergame);
                usersToSend.Add(usergame);
                usersToSend.Add(usergame);
                PreGame preGame = new PreGame(usersToSend, "", "");
                preGame.Show();
                client.DisconnectRoom(usergame.nametag);
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
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
