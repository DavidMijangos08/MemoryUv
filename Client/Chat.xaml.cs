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
        string language = "es-MX";

        public Chat(string _username)
        {

            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
            userName = _username;
            try
            {
                InstanceContext context = new InstanceContext(this);
                client = new ChatService.ChatServiceClient(context);
                client.Join(userName);
                if (language.Equals("es-MX"))
                {
                    lbId.Text = "Bienvenido " + userName;
                }
                else
                {
                    lbId.Text = "Welcome " + userName;
                }
            }
            catch (CommunicationException)
            {
                ShowExceptionAlert();
            }
        }

        public void RecieveMessage(string user, string message)
        {
            string newMessage = $"{user} : {message}";
            messageDisplay.Items.Add(newMessage);
        }

        public void UsersUpdate(Dictionary<object, string>.ValueCollection users)
        {
            listUsersOnline.Items.Clear();
            cbUsers.Items.Clear();
            if (language.Equals("es-MX"))
            {
                cbUsers.Items.Add("Todos");
            }
            else
            {
                cbUsers.Items.Add("All");
            }
            foreach (string user in users)
            {
                listUsersOnline.Items.Add(user);
                cbUsers.Items.Add(user);
            }
        }

        private void BtnSendClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtChat.Text))
            {
                if (cbUsers.SelectedIndex == -1)
                {
                    if (language.Equals("es-MX"))
                    {
                        MessageBox.Show("Seleccione un destinatario");
                    }
                    else
                    {
                        MessageBox.Show("Select a recipient");
                    }
                }
                else
                {
                    try
                    {
                        Object itemSelected = cbUsers.SelectedItem;
                        string receiver = itemSelected.ToString();
                        if (receiver == "Todos" || receiver == "All")
                        {
                            client.SendMessage(txtChat.Text);
                            txtChat.Text = "";
                        }
                        else if (receiver == userName)
                        {
                            if (language.Equals("es-MX"))
                            {
                                MessageBox.Show("No puedes enviarte un mensaje a ti mismo");
                            }
                            else
                            {
                                MessageBox.Show("You can't message yourself");
                            }
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
