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
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Window, RoomService.IRoomServiceCallback
    {
        public string username;
        UserGame usergame = new UserGame();
        MemoryServer service;
        public RoomService.RoomServiceClient client;
        string usergameApplicant;
        string usergameInvited;
        public Home(UserGame _user)
        {
            InitializeComponent();
            username = _user.nametag;
            lbUsername.Text = "" + username;
            usergame = _user;
            InstanceContext context = new InstanceContext(this);
            client = new RoomService.RoomServiceClient(context);
            client.ConnectWaitingRoom(usergame.nametag);
        }

        private void ChatClick(object sender, RoutedEventArgs e)
        {
            Chat windowChat = new Chat(username);
            windowChat.Show();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            service = new MemoryServer();
            service.UpdateUserStatus(usergame.id, "Inactivo");
            this.Close();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings(usergame);
            settings.Show();
            this.Close();
        }

        private void ArchievementClick(object sender, RoutedEventArgs e)
        {
            Logros logro = new Logros(usergame);
            logro.Show();
            this.Close();
        }

        private void FriendsClick(object sender, RoutedEventArgs e)
        {
            Friends windowFriends = new Friends(usergame);
            windowFriends.Show();
            this.Close();
        }

        private void RankingClick(object sender, RoutedEventArgs e)
        {
            Ranking ranking = new Ranking(usergame);
            ranking.Show();
            this.Close();
        }

        private void RoomClick(object sender, RoutedEventArgs e)
        {
            Room room = new Room(usergame);
            room.Show();
            this.Close();
        }

        private void PersonalizeClick(object sender, RoutedEventArgs e)
        {
            Game game = new Game(usergame);
            game.Show();
            this.Close();

            Personalize personalize = new Personalize();
            personalize.Show();
            this.Close();
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
    }
}
