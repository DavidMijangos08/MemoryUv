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

        public Room(UserGame _user)
        {
            usergame = _user;
            InitializeComponent();
            initializeListFriends();
            InstanceContext context = new InstanceContext(this);
            client = new RoomService.RoomServiceClient(context);
            client.ConnectWaitingRoom(usergame.nametag);
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
            Home windowHome = new Home(usergame);
            windowHome.Show();
            this.Close();
        }

        private void ClicAdd(object sender, RoutedEventArgs e)
        {
            object itemSelected = listFriends.SelectedItem;
            usergameInvited = itemSelected.ToString();
            client.SendInvitation(usergame.nametag, usergameInvited);
        }

        private void ClickAccept(object sender, RoutedEventArgs e)
        {
            gridInvitation.Visibility = Visibility.Collapsed;
            client.SendAcceptance(this.usergameApplicant, usergame.nametag);
            service = new MemoryServer();
            List<UserGame> userAdmin = service.GetUsersByInitialesOfNametag(this.usergameApplicant);
            PreGame pregame = new PreGame(usergame, usergame, userAdmin[0]);
            pregame.Show();
            this.Close();
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
            service = new MemoryServer();
            List<UserGame> userInvited = service.GetUsersByInitialesOfNametag(this.usergameInvited);
            PreGame preGame = new PreGame(usergame, userInvited[0], usergame);
            preGame.Show();
            this.Close();
        }

        private void RecharchClick(object sender, RoutedEventArgs e)
        {
            listFriends.Items.Clear();
            initializeListFriends();
        }
    }
}
