using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para FriendRequests.xaml
    /// </summary>
    public partial class FriendRequests : Window
    {
        UserGame usergame;
        MemoryServer service;
        string language;

        /// <summary>
        /// Constructor de la clase FriendRequests en donde se inicializan los diversos componentes
        /// </summary>
        public FriendRequests(UserGame _user)
        {
            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
            usergame = _user;
            try
            {
                InitializeListRequests();
                service = new MemoryServer();
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(_user.id))));
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }          
        }

        /// <summary>
        /// Método que inicializa la lista de solicitudes
        /// </summary>
        private void InitializeListRequests()
        {
            try
            {
                service = new MemoryServer();
                List<UserGame> usersRequesting = service.GetUsersRequesting(usergame.id);
                for (int i = 0; i < usersRequesting.Count(); i++)
                {
                    string nametag = usersRequesting[i].nametag;
                    listRequests.Items.Add(nametag);
                }
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que acepta la solicitud de amistad
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ClicAcceptFriend(object sender, RoutedEventArgs e)
        {
            object itemSelected = listRequests.SelectedItem;
            if(itemSelected != null)
            {
                try
                {
                    service = new MemoryServer();
                    string nametagApplicant = itemSelected.ToString();
                    List<UserGame> userApplicant = service.GetUsersByInitialesOfNametag(nametagApplicant);
                    bool accepted = service.AcceptFriendRequest(userApplicant[0].id, usergame.id);
                    if (accepted)
                    {
                        service.AddFriend(userApplicant[0].id, usergame.id);
                        if (language.Equals("es-MX"))
                        {
                            MessageBox.Show("Tienes un nuevo amigo");
                        }
                        else
                        {
                            MessageBox.Show("You have a new friend");
                        }
                        listRequests.Items.Clear();
                        InitializeListRequests();
                    }
                }
                catch (SystemException)
                {
                    ShowExceptionAlert();
                }
            }
            else
            {
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Debes seleccionar una solicitud");
                }
                else
                {
                    MessageBox.Show("You must select an application");
                }
            }

        }

        /// <summary>
        /// Método que rechaza la solicitud de amistad
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ClicRejectFriend(object sender, RoutedEventArgs e)
        {
            object itemSelected = listRequests.SelectedItem;
            if (itemSelected != null)
            {
                try
                {
                    service = new MemoryServer();
                    string nametagApplicant = itemSelected.ToString();
                    List<UserGame> userApplicant = service.GetUsersByInitialesOfNametag(nametagApplicant);
                    bool rejected = service.RejectFriendRequest(userApplicant[0].id, usergame.id);
                    if (rejected)
                    {
                        if (language.Equals("es-MX"))
                        {
                            MessageBox.Show("Solicitud rechazada con exito");
                        }
                        else
                        {
                            MessageBox.Show("Request rejected successfully");
                        }
                        listRequests.Items.Clear();
                        InitializeListRequests();
                    }
                }
                catch (SystemException)
                {
                    ShowExceptionAlert();
                }
            }
            else
            {
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Debes seleccionar una solicitud");
                }
                else
                {
                    MessageBox.Show("You must select an application");
                }
            }
        }

        /// <summary>
        /// Método que regresa a la ventana anterior
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void ClicExit(object sender, RoutedEventArgs e)
        {
            Friends windowFriends = new Friends(usergame);
            windowFriends.Show();
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
