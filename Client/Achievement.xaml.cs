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
    /// Lógica de interacción para Logros.xaml
    /// </summary>
    public partial class Logros : Window
    {
        UserGame userGame = new UserGame();
        MemoryServer service;
        string language = "es-MX";

        /// <summary>
        /// Constructor de la clase Logros en donde se inicializan los diversos componentes
        /// </summary>
        /// <param name="_user"> Corresponde al usuario conectado al juego</param>
        public Logros(UserGame _user)
        {
            userGame = _user;
            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
            InitializeArchievement();
            try
            {
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(userGame.id))));
                
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que inicializa la verificacion de los logros del usuario
        /// </summary>
        private void InitializeArchievement()
        {
            service = new MemoryServer();
            try
            {
                if (service.GetStatisticUser(userGame.id, 1) == true)
                {
                    lbAchievement1.IsEnabled = true;
                    imgAchievement1.Opacity = 100;
                }
                if (service.GetStatisticUser(userGame.id, 2))
                {
                    lbAchievement2.IsEnabled = true;
                    imgAchievement2.Opacity = 100;
                }
                if (service.GetStatisticUser(userGame.id, 3))
                {
                    lbAchievement3.IsEnabled = true;
                    imgAchievement3.Opacity = 100;
                }
                if (service.GetStatisticUser(userGame.id, 4))
                {
                    lbAchievement4.IsEnabled = true;
                    imgAchievement4.Opacity = 100;
                }
                if (service.GetStatisticUser(userGame.id, 5))
                {
                    lbAchievement5.IsEnabled = true;
                    imgAchievement5.Opacity = 100;
                }
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que permite cerrar la ventana y regresar a Home
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ExitClick(object sender, RoutedEventArgs e)
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
    }
}
