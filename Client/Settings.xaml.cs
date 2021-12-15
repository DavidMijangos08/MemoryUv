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
using System.Data;
using Data;
using Host;
using System.Threading;
using System.Windows.Navigation;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        
        UserGame userGame = new UserGame();
        MemoryServer service;
        string language = "es-MX";

        public Settings(UserGame _user)
        {
            InitializeComponent();
            userGame = _user;
            language = Properties.Settings.Default.languageCode;

            try
            {
                service = new MemoryServer();
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(_user.id))));
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }      
        }


        /// <summary>
        /// Método que regresa a la ventana Home
        /// </summary>
        /// <param name="sender"> Identificador del usuario que desea enviar la solicitud </param>
        /// <param name="e"> Identificador del usuario que recibe la solicitud </param>
        /// <returns> No retorna </returns>
      
        private void CerrarSesionClick(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        /// <summary>
        /// Método que regresa a la ventana Home
        /// </summary>
        /// <param name="sender"> Identificador del usuario que desea enviar la solicitud </param>
        /// <param name="e"> Identificador del usuario que recibe la solicitud </param>
        /// <returns> No retorna </returns>
        
        private void RegresarClick(object sender, RoutedEventArgs e)
        {
            Home windowHome = new Home(userGame);
            windowHome.Show();
            this.Close();
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
        /// Método que permite cambiar el idioma del programa
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void IdiomsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbLanguage.SelectedIndex == 0)
            {
                Properties.Settings.Default.languageCode = "en-US";
                Properties.Settings.Default.Save();
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

                Settings settings = new Settings(userGame);
                settings.Show();
                this.Close();
            }
            else
            {
                Properties.Settings.Default.languageCode = "es-MX";
                Properties.Settings.Default.Save();
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-MX");

                Settings settings = new Settings(userGame);
                settings.Show();
                this.Close();
            }
        }
    }

    
}
