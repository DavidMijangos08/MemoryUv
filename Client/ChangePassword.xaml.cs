using Data;
using Host;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        public MemoryServer service;
        string codex;
        UserGame user;
        string language = "es-MX";


        /// <summary>
        /// Constructor de la clase ChangePassword en donde se inicializan los diversos componentes
        /// </summary>
        public ChangePassword()
        {
            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
        }

        /// <summary>
        /// Método que permite cerrar la ventana y regresar a la anterior.
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        /// <summary>
        /// Método que envia a modificar la contrasenia del usuario
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            string code = tbxCode.Text;
            string password = pbPassword.Password.ToString();
            string passwordRepit = pbPasswordRepit.Password.ToString();
            if (!ExistsEmptyFields(code, password, passwordRepit) && !ExistsInvalidPassword(password, passwordRepit)){
                if (code.Equals(codex))
                {
                    SendToModify(password);
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
            }
        }

        /// <summary>
        /// Método que envia el codigo generado al email del usuario
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void SendCodeClick(object sender, RoutedEventArgs e)
        {
            if (!ExistsInvalidEmail(tbxEmail.Text))
            {
                codex = service.SendEmail(tbxEmail.Text);
                tbxCode.IsEnabled = true;
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Codigo enviado...");
                }
                else
                {
                    MessageBox.Show("code sent...");
                }
            }
        }

        /// <summary>
        /// Método que conecta a la base de datos para modificar el atributo password
        /// </summary>
        /// <param name="sender"> Corresponde al objeto del método </param>
        /// <param name="e"> Corresponde al evento del método </param>
        private void SendToModify(string password)
        {
            try
            {
                service = new MemoryServer();
                bool updated = service.UpdatePassword(user.id, password);
                if (updated)
                {
                    if (language.Equals("es-MX"))
                    {
                        MessageBox.Show("Se actualizo correctamente la contraseña");
                    }
                    else
                    {
                        MessageBox.Show("The password was updated correctly");
                    }
                }
            }
            catch (SystemException)
            {
                ShowExceptionAlert();
            }               
        }
        /// <summary>
        /// Método que verifica si existen campos vacios
        /// </summary>
        /// <param name="code"> Corresponde al campo de codigo </param>
        /// <param name="password"> Corresponde al campo de contrasenia</param>
        /// <param name="passwordRepit"> Corresponde al campo de repetir contrasenia</param>
        private bool ExistsEmptyFields(string code, string password, string passwordRepit)
        {
            bool exists = false;
            if (string.IsNullOrEmpty(tbxCode.Text) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordRepit))
            {
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Existe campo vacío");
                }
                else
                {
                    MessageBox.Show("Empty field exists");
                }
                exists = true;
            }
            return exists;
        }

        /// <summary>
        /// Método que verifica si el email se encuentra registrado en la base de datos
        /// </summary>
        /// <param name="email"> Corresponde al campo de email </param>
        private bool ExistsInvalidEmail(string email)
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

            if (exists != true)
            {
                if (string.IsNullOrEmpty(email))
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
            }
            
            if(exists != true)
            {
                try
                {
                    service = new MemoryServer();
                    UserGame user = service.GetUserByEmail(tbxEmail.Text);
                    if (user != null)
                    {
                        this.user = user;
                    }
                    else
                    {
                        exists = true;
                        if (language.Equals("es-MX"))
                        {
                            MessageBox.Show("No existe un usuario asociado al correo");
                        }
                        else
                        {
                            MessageBox.Show("There is no user associated with the email");
                        }
                    }
                }
                catch (SystemException)
                {
                    ShowExceptionAlert();
                }
            }
            return exists;
        }

        /// <summary>
        /// Método que verifica si la contrasenia coincide con la repetida
        /// </summary>
        /// <param name="password"> Corresponde a la contrasenia </param>
        /// <param name="passwordRepit"> Corresponde a la contrasenia repetida </param>
        private bool ExistsInvalidPassword(string password, string passwordRepit)
        {
            bool exists = false;
            if (!ExistsInvalidCharacters(password))
            {
                if (!password.Equals(passwordRepit))
                {
                    exists = true;
                    if (language.Equals("es-MX"))
                    {
                        MessageBox.Show("Las contraseñas no son iguales");
                    }
                    else
                    {
                        MessageBox.Show("Passwords are not the same");
                    }
                }
            }
            else
            {
                exists = true;
            }
            return exists;
        }

        /// <summary>
        /// Método que verifica si existen caracteres invalidos
        /// </summary>
        /// <param name="password"> Corresponde al campo de contrasenia </param>
        private bool ExistsInvalidCharacters(string password)
        {
            bool exists = false;
            Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)\\S{8,15}$");
            if (!regex.IsMatch(password))
            {
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
                exists = true;
            }
            return exists;
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
