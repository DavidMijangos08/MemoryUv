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
        public Settings(UserGame _user)
        {
            InitializeComponent();
            userGame = _user;

            service = new MemoryServer();
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(_user.id))));

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

        

        private void FiltroVista(object sender, RoutedEventArgs e)
        {
            if (chbVista.IsEnabled)
            {
                filtro.IsEnabled = true;
            }
            else
            {
                filtro.IsEnabled = false;
            }
            
        }

        
    }

    
}
