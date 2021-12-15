using Client.Domain;
using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Game.xaml
    /// </summary>
    public partial class Game : Window, GameService.IGameServiceCallback
    {
        MemoryServer service;
        string language = "es-MX";
        public GameService.GameServiceClient client;
        UserGame userConnected = new UserGame();
        UserGame userOpponent = new UserGame();
        UserGame userAdmin = new UserGame();
        DispatcherTimer dispatcherTimer;
        int decrement;
        string section;
        string userInTurn;

        private Dictionary<string, Card> cards = new Dictionary<string, Card>();
        int flippedCards;
        private List<string> cardNumbers = new List<string>();
        private int[] messyCards = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15,
                                     16, 16, 17, 17, 18, 18};
        private int remainingPairs = 18;
        string statusTurn;
        int pointsAdmin = 0;
        int pointsOpponent = 0;

        /// <summary>
        /// Constructor de la clase Game en donde se inicializan los diversos componentes
        /// </summary>
        /// <param name="users"> Corresponde a una lista con los usuarios que se conectan al juego </param>
        /// <param name="section"> Corresponde a la sección del juego </param>
        /// <param name="difficulty"> Corresponde a la dificultad del juego </param>

        public Game(List<UserGame> users, string section, string difficulty)
        {
            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
            try
            {
                InstanceContext context = new InstanceContext(this);
                client = new GameService.GameServiceClient(context);
                userConnected = users[0];
                userOpponent = users[1];
                userAdmin = users[2];
                client.ConnectToGame(userConnected.nametag, userOpponent.nametag);
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
            this.section = section;
            lbUserAdminPoints.Text = pointsAdmin.ToString();
            lbUserOpponentPoints.Text = pointsOpponent.ToString();
            canvaGame.IsHitTestVisible = false;
            LoadCardsDictionary();
            if (userConnected.nametag.Equals(userAdmin.nametag))
            {
                lbUserAdmin.Text = userConnected.nametag;
                lbUserOpponent.Text = userOpponent.nametag;
                userInTurn = userConnected.nametag;
                flippedCards = 0;
                InitializeTimer();
                canvaGame.IsHitTestVisible = true;
                statusTurn = "En ejecución";
                LoadCardsToGame();
            }
            else
            {
                lbUserAdmin.Text = userOpponent.nametag;
                lbUserOpponent.Text = userConnected.nametag;
            }
            cardNumbers.Clear();
            lbUserTurn.Text = userAdmin.nametag;     
        }

        /// <summary>
        /// Método que permite inicializar el temporizador de turnos del juego
        /// </summary>

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

        /// <summary>
        /// Método que permite generar el evento del temporizador y verificar si terminó el tiempo
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del temporizador </param>

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

        /// <summary>
        /// Método que permite limpiar el tablero con las cartas volteadas 
        /// </summary>

        private void CleanBoard()
        {
            if(flippedCards == 1)
            {
                try
                {
                    cards[cardNumbers[0]].status = StatusCard.NotFlipped;
                    LoadCardsToGame();
                    cardNumbers.Clear();
                    client.SendCleanBoard(userOpponent.nametag);
                    SendTurn();
                }
                catch (CommunicationException)
                {
                    ShowExceptionAlert();
                }
            }
            else
            {
                SendTurn();
            }
            return;
        }

        /// <summary>
        /// Método que permite mandar el turno al jugador oponente
        /// </summary>

        private void SendTurn()
        {
            try
            {
                client.SendGameTurn(userOpponent.nametag);
                lbUserTurn.Text = userOpponent.nametag;
                canvaGame.IsHitTestVisible = false;
                flippedCards = 0;
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que permite recibir si un jugador se desconectó del juego 
        /// </summary>
        /// <param name="userDisconnected"> Corresponde al usuario que se desconectó </param>

        public void ReceiveExitNotification(string userDisconnected)
        {
            string message;
            if (language.Equals("es-MX"))
            {
                message = "El jugador " + userDisconnected + " se desconectó. Tu ganas esta partida!!!";
            }
            else
            {
                message = "The player " + userDisconnected + " got disconnected. You win this game!!!";
            }
            gridNotification.Visibility = Visibility.Visible;
            lbNotification.Text = message;
        }

        /// <summary>
        /// Método que permite recibir el turno de juego
        /// </summary>

        public void ReceiveGameTurn()
        {
            userInTurn = userConnected.nametag;
            statusTurn = "En ejecución";
            canvaGame.IsHitTestVisible = true;
            InitializeTimer();
            flippedCards = 0;
        }

        /// <summary>
        /// Método que permite habilitar la notificación de salida
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>

        private void BackingOutClick(object sender, RoutedEventArgs e)
        {
            gridConfirmation.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Método que controla la aceptación de la notificación de salida
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>

        private void ClickAcceptNotification(object sender, RoutedEventArgs e)
        {
            try
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
        /// Método que permite controlar la confirmación de salida
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>

        private void ClickYes(object sender, RoutedEventArgs e)
        {
            try
            {
                client.DisconnectTheGame("Abandonado", userConnected.nametag, userOpponent.nametag);
                service = new MemoryServer();
                bool addedGameLosing = service.AddOneLoseGame(userConnected.id);
                if (addedGameLosing)
                {
                    this.Close();
                    Home home = new Home(userConnected);
                    home.Show();
                }
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
        /// Método que permite controlar la negación de la notificación de salida
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e">  Corresponde al evento del método </param>

        private void ClickNO(object sender, RoutedEventArgs e)
        {
            gridConfirmation.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Método que genera las cartas del juego
        /// </summary>

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
            if (userConnected.nametag.Equals(userAdmin.nametag))
            {
                MessUpCards();
            } 
        }

        /// <summary>
        /// Mètodo que permite desordenar las cartas del juego
        /// </summary>

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

            try
            {
                client.SendGameBoard(messyCards, userOpponent.nametag);
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que permite llenar el tablero de juego del invitado
        /// </summary>
        /// <param name="messyCards"> Corresponde a las cartas revueltas </param>

        private void FillGuestBoard(int[] messyCards)
        {
            this.messyCards = messyCards;
            int i = 0;
            foreach (string key in cards.Keys)
            {
                cards[key].numberCard = messyCards[i];
                i++;
            }
            LoadCardsToGame();
        }

        /// <summary>
        /// Método que permite voltear las cartas del juego o regresar a su posición original
        /// </summary>

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

        /// <summary>
        /// Método que permite comparar las cartas volteadas
        /// </summary>

        private void CompareCards()
        {
            if(flippedCards == 2)
            { 
                if (cards[cardNumbers[0]].numberCard == cards[cardNumbers[1]].numberCard)
                {
                    remainingPairs--;
                    lbPairs.Text = remainingPairs.ToString();
                    if (userInTurn.Equals(userAdmin.nametag))
                    {
                        pointsAdmin++;
                    }
                    else
                    {
                        pointsOpponent++;
                    }
                    lbUserAdminPoints.Text = pointsAdmin.ToString();
                    lbUserOpponentPoints.Text = pointsOpponent.ToString();
                    if (language.Equals("es-MX"))
                    {
                        MessageBox.Show("Las cartas son iguales");
                    }
                    else
                    {
                        MessageBox.Show("The cards are the same");
                    }
                }
                else
                {
                    cards[cardNumbers[0]].status = StatusCard.NotFlipped;
                    cards[cardNumbers[1]].status = StatusCard.NotFlipped;
                    if (language.Equals("es-MX"))
                    {
                        MessageBox.Show("Las cartas no son iguales");
                    }
                    else
                    {
                        MessageBox.Show("The cards are not the same");
                    }
                }
                flippedCards = 0;
                LoadCardsToGame();
                cardNumbers.Clear();
                CheckEndGame();
                statusTurn = "Completado";
            }
        }

        /// <summary>
        /// Método que permite verificar si el juego llegó a su fin
        /// </summary>

        private void CheckEndGame()
        {
            if(remainingPairs == 0)
            {
                string winner;
                if (pointsAdmin > pointsOpponent)
                {
                    winner = userAdmin.nametag;
                }
                else
                {
                    winner = lbUserOpponent.Text;
                }

                string message;

                if (language.Equals("es-MX"))
                {
                    message = "El juego ha terminado. El ganador es: " + winner;
                }
                else
                {
                    message = "The game is over. The winner is: " + winner;
                }

                gridFinishGame.Visibility = Visibility.Visible;
                lbWinner.Text = message;

                if (userConnected.nametag.Equals(userAdmin.nametag))
                {
                    try
                    {
                        service = new MemoryServer();
                        if (winner.Equals(userConnected.nametag))
                        {
                            service.AddOneWinGame(userConnected.id);
                            service.AddOneLoseGame(userOpponent.id);
                        }
                        else
                        {
                            service.AddOneWinGame(userOpponent.id);
                            service.AddOneLoseGame(userConnected.id);
                        }
                    }
                    catch (SystemException)
                    {
                        ShowExceptionAlert();
                    }
                }
            }
        }

        /// <summary>
        /// Método que permite mandar a voltear una carta trás un clic en el tablero
        /// </summary>
        /// <param name="btn"></param>

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
                }
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 1
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn1Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn1", userOpponent.nametag);
                NewClic("btn1");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 2
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn2Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn2", userOpponent.nametag);
                NewClic("btn2");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 3
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn3Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn3", userOpponent.nametag);
                NewClic("btn3");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 4
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn4Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn4", userOpponent.nametag);
                NewClic("btn4");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 5
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn5Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn5", userOpponent.nametag);
                NewClic("btn5");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 6
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn6Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn6", userOpponent.nametag);
                NewClic("btn6");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 7
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn7Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn7", userOpponent.nametag);
                NewClic("btn7");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 13
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn13Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn13", userOpponent.nametag);
                NewClic("btn13");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 19
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn19Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn19", userOpponent.nametag);
                NewClic("btn19");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 25
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn25Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn25", userOpponent.nametag);
                NewClic("btn25");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 31
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn31Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn31", userOpponent.nametag);
                NewClic("btn31");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 8
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn8Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn8", userOpponent.nametag);
                NewClic("btn8");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 9
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn9Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn9", userOpponent.nametag);
                NewClic("btn9");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 10
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn10Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn10", userOpponent.nametag);
                NewClic("btn10");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 11
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn11Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn11", userOpponent.nametag);
                NewClic("btn11");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 12
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn12Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn12", userOpponent.nametag);
                NewClic("btn12");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 14
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn14Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn14", userOpponent.nametag);
                NewClic("btn14");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 15
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn15Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn15", userOpponent.nametag);
                NewClic("btn15");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 16
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn16Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn16", userOpponent.nametag);
                NewClic("btn16");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 17
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn17Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn17", userOpponent.nametag);
                NewClic("btn17");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 18
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn18Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn18", userOpponent.nametag);
                NewClic("btn18");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 20
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn20Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn20", userOpponent.nametag);
                NewClic("btn20");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 21
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn21Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn21", userOpponent.nametag);
                NewClic("btn21");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 22
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn22Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn22", userOpponent.nametag);
                NewClic("btn22");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 23
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn23Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn23", userOpponent.nametag);
                NewClic("btn23");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 24
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn24Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn24", userOpponent.nametag);
                NewClic("btn24");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 26
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn26Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn26", userOpponent.nametag);
                NewClic("btn26");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 27
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn27Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn27", userOpponent.nametag);
                NewClic("btn27");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 28
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn28Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn28", userOpponent.nametag);
                NewClic("btn28");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 29
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn29Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn29", userOpponent.nametag);
                NewClic("btn29");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 30
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn30Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn30", userOpponent.nametag);
                NewClic("btn30");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 32
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn32Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn32", userOpponent.nametag);
                NewClic("btn32");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 33
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn33Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn33", userOpponent.nametag);
                NewClic("btn33");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 34
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn34Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn34", userOpponent.nametag);
                NewClic("btn34");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 35
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn35Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn35", userOpponent.nametag);
                NewClic("btn35");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que representa al evento del clic en el botón 36
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del botón </param>

        private void Btn36Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client.SendMove(userConnected.nametag, "btn36", userOpponent.nametag);
                NewClic("btn36");
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que recibe las cartas revueltas del juego
        /// </summary>
        /// <param name="messyCards"> Corresponde a las cartas revultas </param>

        public void ReceiveGameBoard(int[] messyCards)
        {
            FillGuestBoard(messyCards);
        }

        /// <summary>
        /// Método que recibe el movimiento del jugador oponente
        /// </summary>
        /// <param name="user"> Corresponde al usuario que hizo el movimiento </param>
        /// <param name="btn"> Corresponde al botón en el que dio clic </param>

        public void ReceiveMove(string user, string btn)
        {
            userInTurn = user;
            NewClic(btn);
        }

        /// <summary>
        /// Método que recibe la limpieza del tablero
        /// </summary>

        public void ReceiveCleanBoard()
        {
            cards[cardNumbers[0]].status = StatusCard.NotFlipped;
            LoadCardsToGame();
            cardNumbers.Clear();
        }

        /// <summary>
        /// Método que permite controlar la aceptación de la notificación de finalización
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>

        private void ClicAcceptFinish(object sender, RoutedEventArgs e)
        {
            this.Close();
            Home home = new Home(userConnected);
            home.Show();
        }

        /// <summary>
        /// Método que permite recibir un mensaje del oponente en el juego
        /// </summary>
        /// <param name="user"> Corresponde al usuario que envía el mensaje </param>
        /// <param name="message"> Corresponde al mensaje que enviaron </param>

        public void RecieveMessageInGame(string user, string message)
        {
            string newMessage = $"{user} : {message}";
            messageDisplay.Items.Add(newMessage);
        }

        /// <summary>
        /// Método que controla el envío de mensajes en el juego
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>

        private void ClicSend(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtChat.Text))
            {
                try
                {
                    client.SendMessageInGame(userConnected.nametag, userOpponent.nametag, txtChat.Text);
                    string user = userConnected.nametag;
                    string message = txtChat.Text;
                    string newMessage = $"{user} : {message}";
                    messageDisplay.Items.Add(newMessage);
                    txtChat.Text = "";
                }
                catch (CommunicationException)
                {
                    ShowExceptionAlert();
                }
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

        /// <summary>
        /// Método que controla la salida del juego desde la X de la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            gridConfirmation.Visibility = Visibility.Visible;
        }
    }
}
