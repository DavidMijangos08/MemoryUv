using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para Friends.xaml
    /// </summary>
    public partial class Friends : Window
    {
        UserGame userGame = new UserGame();
        MemoryServer service;
        string language = "es-MX";

        public Friends(UserGame _user)
        {
            userGame = _user;
            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
            try
            {
                InitializeFriendsList();
                service = new MemoryServer();
                this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(_user.id))));
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        private void InitializeFriendsList()
        {
            try
            {
                service = new MemoryServer();
                List<UserGame> friends = service.GetFriendsList(userGame.id);
                for (int i = 0; i < friends.Count(); i++)
                {
                    string nametag = friends[i].nametag;
                    listFriends.Items.Add(nametag);
                }
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }       
        }

        private void ClicExit(object sender, RoutedEventArgs e)
        {
            Home windowHome = new Home(userGame);
            windowHome.Show();
            this.Close();
        }

        private void ClicAddFriend(object sender, RoutedEventArgs e)
        {
            object itemSelected = listSearchUsers.SelectedItem;
            if (itemSelected != null)
            {
                try
                {
                    string nametagAddressee = itemSelected.ToString();
                    service = new MemoryServer();
                    List<UserGame> addressee = service.GetUsersByInitialesOfNametag(nametagAddressee);
                    if (!ExistsRequestImpediment(addressee[0]))
                    {
                        bool added = service.AddFriendRequest(userGame.id, addressee[0].id);
                        if (added)
                        {
                            if (language.Equals("es-MX"))
                            {
                                MessageBox.Show("Solicitud enviada con exito");
                            }
                            else
                            {
                                MessageBox.Show("Request sent successfully");
                            }
                        }
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
                    MessageBox.Show("Debes seleccionar a alguien");
                }
                else
                {
                    MessageBox.Show("You must select someone");
                }
            }
            listSearchUsers.Items.Clear();
        }

        private bool ExistsRequestImpediment(UserGame addressee)
        {
            bool exists = false;
            service = new MemoryServer();
            if (addressee.nametag.Equals(userGame.nametag))
            {
                exists = true;
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("No puedes enviar una solicitud a ti mismo");
                }
                else
                {
                    MessageBox.Show("You cannot submit a request to yourself");
                }
            }

            try
            {
                bool existsFriendship = service.ExistsFriendship(userGame.id, addressee.id);
                if (!existsFriendship)
                {
                    bool exitsRequest = service.ExistsPendingRequest(userGame.id, addressee.id);
                    if (exitsRequest)
                    {
                        exists = true;
                        if (language.Equals("es-MX"))
                        {
                            MessageBox.Show("Ya hay una solicitud pendiente");
                        }
                        else
                        {
                            MessageBox.Show("There is already a pending request");
                        }
                    }
                }
                else
                {
                    exists = true;
                    if (language.Equals("es-MX"))
                    {
                        MessageBox.Show("El jugador ya es tu amigo");
                    }
                    else
                    {
                        MessageBox.Show("The player is already your friend");
                    }
                }
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
            return exists;
        }

        private void ClicSearchFriend(object sender, RoutedEventArgs e)
        {
            if (!ExistsInvalidField())
            {
                try
                {
                    if (language.Equals("es-MX"))
                    {
                        MessageBox.Show("Buscando...");
                    }
                    else
                    {
                        MessageBox.Show("Searching...");
                    }
                    service = new MemoryServer();
                    List<UserGame> coincidences = service.GetUsersByInitialesOfNametag(tbxNametag.Text);
                    FillListSearchUsers(coincidences);
                }
                catch (SystemException)
                {
                    ShowExceptionAlert();
                }
            }
        }

        private void FillListSearchUsers(List<UserGame> coincidences)
        {
            for (int i = 0; i < coincidences.Count(); i++)
            {
                string nametag = coincidences[i].nametag;
                listSearchUsers.Items.Add(nametag);
            }
        }

        private void ClicRemoveFriend(object sender, RoutedEventArgs e)
        {
            object itemSelected = listFriends.SelectedItem;
            if(itemSelected != null)
            {
                try
                {
                    service = new MemoryServer();
                    string nametagFriend = itemSelected.ToString();
                    List<UserGame> friend = service.GetUsersByInitialesOfNametag(nametagFriend);
                    bool deleted = service.DeleteFriend(userGame.id, friend[0].id);
                    if (deleted)
                    {
                        listFriends.Items.Clear();
                        InitializeFriendsList();
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
                    MessageBox.Show("Para eliminar debes seleccionar al amigo de la lista");
                }
                else
                {
                    MessageBox.Show("To remove you must select the friend from the list");
                }
            }
        }

        private void ClicRequests(object sender, RoutedEventArgs e)
        {
            FriendRequests windowFriendRequests = new FriendRequests(userGame);
            windowFriendRequests.Show();
            this.Close();
        }

        private bool ExistsInvalidField()
        {
            bool exists = false;
            string userSearch = tbxNametag.Text;
            if (!string.IsNullOrEmpty(userSearch))
            {
                Regex regex = new Regex("^[A-Za-z0-9]\\S+$");
                if (!regex.IsMatch(userSearch) || userSearch.Length > 10)
                {
                    exists = true;
                    SendAlert(TypeError.INVALIDNAMETAG);
                }
            }
            else
            {
                exists = true;
                SendAlert(TypeError.EMPTYFIELD);
            }
            return exists;
        }

        private void SendAlert(TypeError typeError)
        {
            if (typeError == TypeError.EMPTYFIELD)
            {
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("El campo de texto está vacío");
                }
                else
                {
                    MessageBox.Show("The text field is empty");
                }
            }

            if (typeError == TypeError.INVALIDNAMETAG)
            {
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Existen caracteres inválidos en el nametag a buscar \n" +
                                    "Recuerda que: \n" +
                                    "El nametag solo puede tener más de 10 caracteres \n" +
                                    "El nametag solo puede tener letras y numeros \n" +
                                    "El nametag no puede llevar espacios");
                }
                else
                {
                    MessageBox.Show("There are invalid characters in the nametag to search for \n " +
                                     "Remember that: \n" +
                                     "The nametag can only have more than 10 characters \n" +
                                     "The nametag can only have letters and numbers \n" +
                                     "The nametag cannot have spaces");
                }
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

        public enum TypeError
        {
            EMPTYFIELD, INVALIDNAMETAG
        }
    }
}
