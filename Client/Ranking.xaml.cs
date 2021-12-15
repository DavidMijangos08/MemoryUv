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
    /// Lógica de interacción para Ranking.xaml
    /// </summary>
    public partial class Ranking : Window
    {

        UserGame userGame = new UserGame();
        MemoryServer service;

        /// <summary>
        /// Constructor de la clase Ranking en donde se inicializan los diversos componentes
        /// </summary>
        public Ranking(UserGame _user)
        {
            InitializeComponent();
            try
            {
                InitializeListRank();
                userGame = _user;
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(userGame.id))));
                
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que inicializa la tabla de clasificacion
        /// </summary>
        private void InitializeListRank()
        {
            try
            {
                service = new MemoryServer();
                List<StatisticUser> itemListView = new List<StatisticUser>();
                List<StatisticUser> itemStatic = service.GetBetterUser();

                for (int i = 0; i < itemStatic.Count(); i++)
                {
                    itemListView.Add(new StatisticUser() { id = i + 1, nameTag = itemStatic[i].nameTag, totalWins = itemStatic[i].totalWins });
                }

                listRank.ItemsSource = itemListView;
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que regresa a la ventana anterior
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
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
            MessageBox.Show("Ocurrió un error en el sistema, intente más tarde.");
            this.Close();
        }
    }
}
