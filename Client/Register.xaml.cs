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
using Data;
using Logic;
using Host;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Data;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Registrar.xaml
    /// </summary>
    public partial class Register : Window
    {
        public MemoryServer service;
        string codex;
        string language = "es-MX";

        public Register()
        {
            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
        }

        /// <summary>
        /// Método que finaliza ventana
        /// </summary>
        /// <param name="sender"> Identificador del usuario que desea enviar la solicitud </param>
        /// <param name="e"> Identificador del usuario que recibe la solicitud </param>
        /// <returns> No retorna </returns>
        private void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        /// <summary>
        /// Método que habilita los campos de texto de codigo, nametag y envia un codigo de verificacion al correo del usuario.
        /// </summary>
        /// <param name="sender"> Identificador del usuario que desea enviar la solicitud </param>
        /// <param name="e"> Identificador del usuario que recibe la solicitud </param>
        /// <returns> No retorna </returns>
        private void ButtonContinuar_Click(object sender, RoutedEventArgs e)
        {
            service = new MemoryServer();            
            string email = tbCorreo.Text;
            string password = pbPassword.Password.ToString();
                         
            if (!ExistsInvalidFields(email, password))
            {
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Codigo enviado...");
                }
                else
                {
                    MessageBox.Show("code sent...");
                }
                codex = service.SendEmail(email);
                tbCodigo.IsEnabled = true;
                tbNametag.IsEnabled = true;
            }
        }

        /// <summary>
        /// Método que regresa guarda un UserGame en la base de datos y regresa a la ventana Login
        /// </summary>
        /// <param name="sender"> Identificador del usuario que desea enviar la solicitud </param>
        /// <param name="e"> Identificador del usuario que recibe la solicitud </param>
        /// <returns> No retorna </returns>
        private void ButtonAcceptClick(object sender, RoutedEventArgs e)
        {
            service = new MemoryServer();
            string code = tbCodigo.Text;
            string nametag = tbNametag.Text;
            string email = tbCorreo.Text;
            string password = pbPassword.Password.ToString();

            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(nametag) && !ExistsInvalidNametag(nametag))
            {
                if(code == codex)
                {
                    try
                    {
                        bool saved = service.RegisterUser(email, password, nametag);
                        if (saved)
                        {
                            if (language.Equals("es-MX"))
                            {
                                MessageBox.Show("Se agrego correctamente al usuario");
                            }
                            else
                            {
                                MessageBox.Show("Successfully added to user");
                            }
                            CreateStatisticsUser(nametag);
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
                        MessageBox.Show("Codigo incorrecto");
                    }
                    else
                    {
                        MessageBox.Show("Incorrect code");
                    }
                }               
                Login login = new Login();
                login.Show();
                this.Hide();                       
            }
            else
            {
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Existe campo vacío");
                }
                else
                {
                    MessageBox.Show("Empty field exists");
                }
            }
        }

        /// <summary>
        /// Método que permite crear las estadisticas vacías del nuevo jugador
        /// </summary>
        /// <param name="nametag"> Corresponde al nametag del jugador a crear sus estadisticas </param>

        private void CreateStatisticsUser(string nametag)
        {
            try
            {
                List<UserGame> user = service.GetUsersByInitialesOfNametag(nametag);
                bool addedStatistics = service.AddedStatisticUser(user[0].id, nametag);
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
        }

        /// <summary>
        /// Método que verifica si existen campos invalidos
        /// </summary>
        /// <param name="email"> Correo electronico del usuario  </param>
        /// <param name="password"> Identificador del usuario que recibe la solicitud </param>
        /// <returns> No retorna </returns>
        private Boolean ExistsInvalidFields(string email, string password)
        {
            bool exists = false;
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                exists = true;
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Existe campo vacío");
                }
                else
                {
                    MessageBox.Show("Empty field exists");
                }
            }
            else if (ExistsInvalidEmail(email) || ExistsInvalidPassword(password) || ExistsEqualEmail(email))
            {
                exists = true;
            }
            return exists;
        }

        private Boolean ExistsInvalidEmail(string email)
        {
            bool exists = false;
            try
            {
                MailAddress mailAdress = new MailAddress(email);
            }
            catch (FormatException)
            {
                exists = true;
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Existen caracteres invalidos en el correo electronico");
                }
                else
                {
                    MessageBox.Show("There are invalid characters in the email");
                }
            }
            return exists;
        }

        private Boolean ExistsInvalidPassword(string password)
        {
            bool exists = false;
            Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)\\S{8,15}$");
            if (!regex.IsMatch(password))
            {
                exists = true;
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Contraseña insegura \n"
                                        + "La contraseña debe tener entre 8 y 16 caracteres \n"
                                        + "La contraseña debe tener por lo menos un digito \n"
                                        + "La contraseña debe tener por lo menos una letra mayúscula \n"
                                        + "La contraseña debe tener por lo menos una letra minúscula");
                }
                else
                {
                    MessageBox.Show("Weak password \n "
                                         + "Password must be between 8 and 16 characters \n"
                                         + "The password must have at least one digit \n"
                                         + "Password must have at least one capital letter \n"
                                         + "The password must have at least one lowercase letter");
                }
            }
            return exists;
        }

        private Boolean ExistsInvalidNametag(string nametag)
        {
            bool exists = false;
            Regex regex = new Regex("^[A-Za-z0-9]\\S+$");
            service = new MemoryServer();
            try
            {
                if (!regex.IsMatch(nametag) || nametag.Length > 10 || nametag.Length < 4)
                {
                    exists = true;
                    if (language.Equals("es-MX"))
                    {
                        MessageBox.Show("El nametag solo puede tener entre 4 y 10 caracteres \n" +
                                        " El nametag solo puede tener letras y numeros \n" +
                                        "El nametag no puede llevar espacios");
                    }
                    else
                    {
                        MessageBox.Show("The nametag can only be between 4 and 10 characters long \n " +
                                         "The nametag can only have letters and numbers \n" +
                                         "The nametag cannot have spaces");
                    }
                }
                else if (service.ExistsNametag(nametag))
                {
                    exists = true;
                    if (language.Equals("es-MX"))
                    {
                        MessageBox.Show("El nametag ya esta registrado en el juego");
                    }
                    else
                    {
                        MessageBox.Show("The nametag is already registered in the game");
                    }
                }
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
            return exists;
        }

        private Boolean ExistsEqualEmail(string email)
        {
            bool exists = false;
            try
            {              
                service = new MemoryServer();
                if (service.ExistsEmail(email))
                {
                    exists = true;
                    if (language.Equals("es-MX"))
                    {
                        MessageBox.Show("El correo electronico ya esta registrado en el juego");
                    }
                    else
                    {
                        MessageBox.Show("The email is already registered in the game");
                    }
                }
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }
            return exists;
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
