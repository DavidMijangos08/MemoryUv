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
        public Home(UserGame _user)
        {
            InitializeComponent();
            username = _user.nametag;
            lbUsername.Text = "" + username;
            usergame = _user;
            try
            {
                InstanceContext context = new InstanceContext(this);
                client = new RoomService.RoomServiceClient(context);
                client.ConnectWaitingRoom(usergame.nametag);
                service = new MemoryServer();
                service.UpdateUserStatus(usergame.id, "Activo");
                //this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(_user.id))));
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

        private void ClicRejeact(object sender, RoutedEventArgs e)
        {
            gridInvitation.Visibility = Visibility.Collapsed;
        }

        public void RecieveInvitation(string usergameApplicant)
        {
            this.usergameApplicant = usergameApplicant;
            string messageInvitation = "El usuario " + usergameApplicant + " te está invitando a su sala";
            gridInvitation.Visibility = Visibility.Visible;
            lbInvitation.Text = messageInvitation;
        }

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

        private void ShowExceptionAlert()
        {
            MessageBox.Show("Ocurrió un error en el sistema, intente más tarde.");
            this.Close();
        }
    }
}
