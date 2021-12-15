using Host;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para Chat.xaml
    /// </summary>
    public partial class Chat : Window, ChatService.IChatServiceCallback
    {
        public string userName;
        public ChatService.ChatServiceClient client;
        public bool isDataDirty = false;
        MemoryServer service;

        /// <summary>
        /// Constructor de la clase Chat donde se inicializan los diversos componentes
        /// </summary>
        /// <param name="_username"> Corresponde a el nombre del usuario</param>
        public Chat(string _username)
        {

            InitializeComponent();
            userName = _username;
            try
            {
                InstanceContext context = new InstanceContext(this);
                client = new ChatService.ChatServiceClient(context);
                client.Join(userName);
                lbId.Text = "Bienvenido " + userName;
                service = new MemoryServer();
                //this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), service.GetBackgroundUser(_username.id))));
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que recibe el mensaje
        /// </summary>
        /// <param name="user"> Corresponde al usuario </param>
        /// <param name="e"> Corresponde al mensaje </param>
        public void RecieveMessage(string user, string message)
        {
            string newMessage = $"{user} : {message}";
            messageDisplay.Items.Add(newMessage);
        }

        /// <summary>
        /// Método que actualiza los usuarios disponibles
        /// </summary>
        /// <param name="users"> Corresponde a un diccionario que colecciona usuarios </param>
        public void UsersUpdate(Dictionary<object, string>.ValueCollection users)
        {
            listUsersOnline.Items.Clear();
            cbUsers.Items.Clear();
            cbUsers.Items.Add("Todos");
            foreach (string user in users)
            {
                listUsersOnline.Items.Add(user);
                cbUsers.Items.Add(user);
            }
        }

        /// <summary>
        /// Método que envia el mensaje
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void BtnSendClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtChat.Text))
            {
                if (cbUsers.SelectedIndex == -1)
                {
                    MessageBox.Show("Seleccione un destinatario");
                }
                else
                {
                    try
                    {
                        Object itemSelected = cbUsers.SelectedItem;
                        string receiver = itemSelected.ToString();
                        if (receiver == "Todos")
                        {
                            client.SendMessage(txtChat.Text);
                            txtChat.Text = "";
                        }
                        else if (receiver == userName)
                        {
                            MessageBox.Show("No puedes enviarte un mensaje a ti mismo");
                        }
                        else
                        {
                            client.PrivateSendMessage(txtChat.Text, receiver);
                            txtChat.Text = "";
                        }
                    }
                    catch (CommunicationException)
                    {
                        ShowExceptionAlert();
                    }
                }
            }
        }

        /// <summary>
        /// Método que conecta a la base de datos para modificar el atributo password
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isDataDirty = true;
            if (this.isDataDirty)
            {
                try
                {
                    client.Leave(userName);
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
            MessageBox.Show("Ocurrió un error en el sistema, intente más tarde.");
            this.Close();
        }
    }
}
