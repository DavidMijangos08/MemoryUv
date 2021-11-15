﻿using Data;
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
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public string username;
        UserGame usergame = new UserGame();
        MemoryServer service;
        public Home(UserGame _user)
        {
            InitializeComponent();
            username = _user.nametag;
            lbUsername.Text = "" + username;
            usergame = _user;

            
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
    }
}
