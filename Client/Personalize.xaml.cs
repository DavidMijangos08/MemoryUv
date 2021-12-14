using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Personalize.xaml
    /// </summary>
    public partial class Personalize : Window
    {
        UserGame userGame = new UserGame();
        MemoryServer service = new MemoryServer();
        string language = "es-MX";

        public Personalize(UserGame _user)
        {
            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
            userGame = _user;
            try
            {
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(userGame.id))));
                //this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/Background/backgroundX.jpg")));
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Home windowHome = new Home(userGame);
            windowHome.Show();
            this.Close();
        }

        private void DefaultClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/Background/backgroundDefault.png")));
                service.SetBackgroundUser(userGame.id, 0);

                //this.Background = Brushes.Gray;
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        private void GreenClick(object sender, RoutedEventArgs e)
        {
            //Personalize per = new Personalize(userGame);
            //  ImageBrush test = service.GetConfigUser(per);
            //ImageBrush imageBrushed = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/Background/backgroundGreen.png")));
            //Background = test;
            try
            {
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/Background/backgroundGreen.png")));
                service.SetBackgroundUser(userGame.id, 1);
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        private void IceClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/Background/backgroundIce.png")));
                service.SetBackgroundUser(userGame.id, 2);
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        private void PinkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/Background/backgroundPink.jpg")));
                service.SetBackgroundUser(userGame.id, 3);
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        private void RedClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/Background/backgroundRed.jpg")));
                service.SetBackgroundUser(userGame.id, 4);
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        private void XClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources/Background/backgroundX.jpg")));
                service.SetBackgroundUser(userGame.id, 5);
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
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
