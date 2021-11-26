using Client.Domain;
using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        string section;

        private Dictionary<string, Card> cards = new Dictionary<string, Card>();
        int flippedCards;
        private List<string> cardNumbers = new List<string>();
        private int[] messyCards = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15,
                                     16, 16, 17, 17, 18, 18};
        private int remainingPairs = 18;
        string statusTurn;
        public Game(List<UserGame> users, string section, string difficulty)
        {
            InitializeComponent();
            InstanceContext context = new InstanceContext(this);
            client = new GameService.GameServiceClient(context);
            userConnected = users[0];
            userOpponent = users[1];
            UserGame userAdmin = users[2];
            this.section = section;
            canvaGame.IsHitTestVisible = false;
            client.ConnectToGame(userConnected.nametag, userOpponent.nametag);
            if (userConnected.nametag.Equals(userAdmin.nametag))
            {
                InitializeTimer();
                canvaGame.IsHitTestVisible = true;
                flippedCards = 0;
                statusTurn = "En ejecución";
            }
            lbUserTurn.Text = userAdmin.nametag;
            LoadCardsDictionary();
            cardNumbers.Clear();
            LoadCardsToGame();
            service = new MemoryServer();
            //this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(users[0].id))));
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
            if (lbTimer.Text.Equals("0") || statusTurn.Equals("Completado"))
            {
                dispatcherTimer.Stop();
                lbTimer.Text = "";
                CleanBoard();
            }
        }

        private void CleanBoard()
        {
            if(flippedCards == 1)
            {
                cards[cardNumbers[0]].status = StatusCard.NotFlipped;
                LoadCardsToGame();
                cardNumbers.Clear();
                SendTurn();
            }
            else
            {
                SendTurn();
            }
        }

        private void SendTurn()
        {
            client.SendGameTurn(userOpponent.nametag);
            lbUserTurn.Text = userOpponent.nametag;
            canvaGame.IsHitTestVisible = false;
        }

        public void ReceiveExitNotification(string userDisconnected)
        {
            string message = "El jugador " + userDisconnected + " se desconectó. Tu ganas esta partida!!!";
            gridNotification.Visibility = Visibility.Visible;
            lbNotification.Text = message;
        }

        public void ReceiveGameTurn()
        {
            flippedCards = 0;
            statusTurn = "En ejecución";
            canvaGame.IsHitTestVisible = true;
            InitializeTimer();
        }

        private void BackingOutClick(object sender, RoutedEventArgs e)
        {
            gridConfirmation.Visibility = Visibility.Visible;
        }

        private void ClickAcceptNotification(object sender, RoutedEventArgs e)
        {
            client.DisconnectTheGame("Finalizado", userConnected.nametag, userOpponent.nametag);
            service = new MemoryServer();
            bool addedGameWon = service.AddOneWinGame(userConnected.id);
            if (addedGameWon)
            {
                this.Close();
                Home home = new Home(userConnected);
                home.Show();
            }
        }

        private void ClickYes(object sender, RoutedEventArgs e)
        {
            client.DisconnectTheGame("Abandonado", userConnected.nametag, userOpponent.nametag);
            service = new MemoryServer();
            bool addedGameLosing = service.AddOneLoseGame(userConnected.id);
            if (addedGameLosing) {
                this.Close();
                Home home = new Home(userConnected);
                home.Show();
            }
        }

        private void ClickNO(object sender, RoutedEventArgs e)
        {
            gridConfirmation.Visibility = Visibility.Hidden;
        }

        private void LoadCardsDictionary()
        {
            cards["btn1"] = new Card(StatusCard.NotFlipped, 0, btn1);
            cards["btn2"] = new Card(StatusCard.NotFlipped, 0, btn2);
            cards["btn3"] = new Card(StatusCard.NotFlipped, 0, btn3);
            cards["btn4"] = new Card(StatusCard.NotFlipped, 0, btn4);
            cards["btn5"] = new Card(StatusCard.NotFlipped, 0, btn5);
            cards["btn6"] = new Card(StatusCard.NotFlipped, 0, btn6);
            cards["btn7"] = new Card(StatusCard.NotFlipped, 0, btn7);
            cards["btn8"] = new Card(StatusCard.NotFlipped, 0, btn8);
            cards["btn9"] = new Card(StatusCard.NotFlipped, 0, btn9);
            cards["btn10"] = new Card(StatusCard.NotFlipped, 0, btn10);
            cards["btn11"] = new Card(StatusCard.NotFlipped, 0, btn11);
            cards["btn12"] = new Card(StatusCard.NotFlipped, 0, btn12);
            cards["btn13"] = new Card(StatusCard.NotFlipped, 0, btn13);
            cards["btn14"] = new Card(StatusCard.NotFlipped, 0, btn14);
            cards["btn15"] = new Card(StatusCard.NotFlipped, 0, btn15);
            cards["btn16"] = new Card(StatusCard.NotFlipped, 0, btn16);
            cards["btn17"] = new Card(StatusCard.NotFlipped, 0, btn17);
            cards["btn18"] = new Card(StatusCard.NotFlipped, 0, btn18);
            cards["btn19"] = new Card(StatusCard.NotFlipped, 0, btn19);
            cards["btn20"] = new Card(StatusCard.NotFlipped, 0, btn20);
            cards["btn21"] = new Card(StatusCard.NotFlipped, 0, btn21);
            cards["btn22"] = new Card(StatusCard.NotFlipped, 0, btn22);
            cards["btn23"] = new Card(StatusCard.NotFlipped, 0, btn23);
            cards["btn24"] = new Card(StatusCard.NotFlipped, 0, btn24);
            cards["btn25"] = new Card(StatusCard.NotFlipped, 0, btn25);
            cards["btn26"] = new Card(StatusCard.NotFlipped, 0, btn26);
            cards["btn27"] = new Card(StatusCard.NotFlipped, 0, btn27);
            cards["btn28"] = new Card(StatusCard.NotFlipped, 0, btn28);
            cards["btn29"] = new Card(StatusCard.NotFlipped, 0, btn29);
            cards["btn30"] = new Card(StatusCard.NotFlipped, 0, btn30);
            cards["btn31"] = new Card(StatusCard.NotFlipped, 0, btn31);
            cards["btn32"] = new Card(StatusCard.NotFlipped, 0, btn32);
            cards["btn33"] = new Card(StatusCard.NotFlipped, 0, btn33);
            cards["btn34"] = new Card(StatusCard.NotFlipped, 0, btn34);
            cards["btn35"] = new Card(StatusCard.NotFlipped, 0, btn35);
            cards["btn36"] = new Card(StatusCard.NotFlipped, 0, btn36);
            MessUpCards();
        }

        private void MessUpCards()
        {
            Random random = new Random();
            messyCards = messyCards.OrderBy(x => random.Next()).ToArray();
            int i = 0;
            foreach(string key in cards.Keys)
            {
                cards[key].numberCard = messyCards[i];
                i++;
            }
        }

        private void LoadCardsToGame()
        {
            foreach (string key in cards.Keys)
            {
                Card card = cards[key];
                Button button = card.button;
                int numberCard;
                if (card.status == StatusCard.NotFlipped)
                {
                    button.Content = "";
                    button.Background = Brushes.Black;
                }
                else
                {
                    numberCard = card.numberCard;
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("Resources/" + section + "/" + numberCard + ".png", UriKind.Relative));
                    image.Stretch = Stretch.Fill;
                    button.Content = image;
                }
            }
            CompareCards();
        }

        private void CompareCards()
        {
            if(flippedCards == 2)
            {
                if (cards[cardNumbers[0]].numberCard == cards[cardNumbers[1]].numberCard)
                {
                    remainingPairs--;
                    MessageBox.Show("Son iguales");
                }
                else
                {
                    cards[cardNumbers[0]].status = StatusCard.NotFlipped;
                    cards[cardNumbers[1]].status = StatusCard.NotFlipped;
                    MessageBox.Show("No son iguales");
                }
                flippedCards = 0;
                LoadCardsToGame();
                cardNumbers.Clear();
                CheckEndGame();
                statusTurn = "Completado";
            }
        }

        private void CheckEndGame()
        {
            if(remainingPairs == 0)
            {
                MessageBox.Show("El juego ha terminado");
            }
        }

        private void NewClic(string btn)
        {
            if(flippedCards < 2)
            {
                Card card = cards[btn];
                if(card.status == StatusCard.NotFlipped)
                {
                    cardNumbers.Add(btn);
                    cards[btn].status = StatusCard.TurnedAround;
                    flippedCards++;
                    LoadCardsToGame();
                    return;
                }
            }
        }

        private void Btn1Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn1");
        }

        private void Btn2Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn2");
        }

        private void Btn3Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn3");
        }

        private void Btn4Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn4");
        }

        private void Btn5Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn5");
        }

        private void Btn6Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn6");
        }

        private void Btn7Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn7");
        }

        private void Btn13Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn13");
        }

        private void Btn19Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn19");
        }

        private void Btn25Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn25");
        }

        private void Btn31Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn31");
        }

        private void Btn8Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn8");
        }

        private void Btn9Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn9");
        }

        private void Btn10Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn10");
        }

        private void Btn11Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn11");
        }

        private void Btn12Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn12");
        }

        private void Btn14Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn14");
        }

        private void Btn15Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn15");
        }

        private void Btn16Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn16");
        }

        private void Btn17Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn17");
        }

        private void Btn18Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn18");
        }

        private void Btn20Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn20");
        }

        private void Btn21Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn21");
        }

        private void Btn22Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn22");
        }

        private void Btn23Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn23");
        }

        private void Btn24Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn24");
        }

        private void Btn26Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn26");
        }

        private void Btn27Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn27");
        }

        private void Btn28Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn28");
        }

        private void Btn29Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn29");
        }

        private void Btn30Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn30");
        }

        private void Btn32Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn32");
        }

        private void Btn33Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn33");
        }

        private void Btn34Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn34");
        }

        private void Btn35Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn35");
        }

        private void Btn36Click(object sender, RoutedEventArgs e)
        {
            NewClic("btn36");
        }
    }
}
