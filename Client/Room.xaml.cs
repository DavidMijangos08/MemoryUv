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
    /// Lógica de interacción para Room.xaml
    /// </summary>
    public partial class Room : Window, RoomService.IRoomServiceCallback
    { 
        UserGame usergame = new UserGame();
        MemoryServer service;
        public RoomService.RoomServiceClient client;
        string usergameApplicant;
        string usergameInvited;
        string section;
        string difficulty;
        string language = "es-MX";

        public Room(UserGame _user)
        {
            usergame = _user;
            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
            InitializeListFriends();
            try
            {
                InstanceContext context = new InstanceContext(this);
                client = new RoomService.RoomServiceClient(context);
                client.ConnectWaitingRoom(usergame.nametag);
                service = new MemoryServer();
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

        private void InitializeListFriends()
        {
            try
            {
                service = new MemoryServer();
                List<UserGame> friendsConnected = service.GetConnectedFriends(usergame.id);
                for (int i = 0; i < friendsConnected.Count(); i++)
                {
                    string nametag = friendsConnected[i].nametag;
                    listFriends.Items.Add(nametag);
                }
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        private void ClicExit(object sender, RoutedEventArgs e)
        {
            try
            {
                client.DisconnectRoom(usergame.nametag);
                Home windowHome = new Home(usergame);
                windowHome.Show();
                this.Close();
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        private void ClicAdd(object sender, RoutedEventArgs e)
        {
            if(cbSection.SelectedItem != null && cbDifficulty.SelectedItem != null)
            {
                section = cbSection.Text;
                difficulty = cbDifficulty.Text;
                object itemSelected = listFriends.SelectedItem;
                usergameInvited = itemSelected.ToString();
                try
                {
                    client.SendInvitation(usergame.nametag, usergameInvited);
                }
                catch (CommunicationException)
                {
                    ShowExceptionAlert();
                }
            }
            else
            {
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Debes configurar la partida");
                }
                else
                {
                    MessageBox.Show("You must configure the game");
                }
            }
           
        }

        private void ClickAccept(object sender, RoutedEventArgs e)
        {
            try
            {
                gridInvitation.Visibility = Visibility.Collapsed;
                client.SendAcceptance(this.usergameApplicant, usergame.nametag);
                client.DisconnectRoom(usergame.nametag);
                service = new MemoryServer();
                List<UserGame> userAdmin = service.GetUsersByInitialesOfNametag(this.usergameApplicant);
                List<UserGame> usersToSend = new List<UserGame>(4);
                usersToSend.Add(usergame);
                usersToSend.Add(usergame);
                usersToSend.Add(userAdmin[0]);
                PreGame pregame = new PreGame(usersToSend, "", "");
                pregame.Show();
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

        public void RecieveAnswer()
        {
            try
            {
                service = new MemoryServer();
                List<UserGame> userInvited = service.GetUsersByInitialesOfNametag(this.usergameInvited);
                List<UserGame> usersToSend = new List<UserGame>(4);
                usersToSend.Add(usergame);
                usersToSend.Add(userInvited[0]);
                usersToSend.Add(usergame);
                PreGame preGame = new PreGame(usersToSend, section, difficulty);
                preGame.Show();
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

        private void RecharchClick(object sender, RoutedEventArgs e)
        {
            listFriends.Items.Clear();
            InitializeListFriends();
        }

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
