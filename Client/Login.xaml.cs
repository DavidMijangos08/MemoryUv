using Data;
using Host;
using Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public MemoryServer service;
        public Login()
        {
            InitializeComponent();   
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = Usuario.Text;
            string password = pbPassword.Password.ToString();

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !ExistsInvalidPassword(password))
            {
                try
                {
                    service = new MemoryServer();
                    UserGame user = service.GetLoggerUser(email, password);
                    if (user != null)
                    {

                        if (!service.ExistsConfigUser(user.id))
                        {
                            service.NewConfigUser(user.id);
                        }
                        Home windowHome = new Home(user);
                        windowHome.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Correo o contraseña incorrecta, revisa bien!");
                    }
                }
                catch (SystemException)
                {
                    ShowExceptionAlert();
                }        
            }
            else
            {
                MessageBox.Show("Existe campo vacio");
            }
        }

        private bool ExistsInvalidPassword(string password)
        {
            bool exists = false;
            Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)\\S{8,15}$");
            if (!regex.IsMatch(password))
            {
                exists = true;
                MessageBox.Show("Caracteres inválidos en la contraseña");
            }
            return exists;
        }

        private void ButtonNewAccountClick(object sender, RoutedEventArgs e)
        {
            Register windowRegister = new Register();
            windowRegister.Show();
            this.Close();
        }

        private void SupportClick(object sender, RoutedEventArgs e)
        {
            ChangePassword windowChangePassword = new ChangePassword();
            windowChangePassword.Show();
            this.Close();
        }

        private void ShowExceptionAlert()
        {
            MessageBox.Show("Ocurrió un error en el sistema, intente más tarde.");
            this.Close();
        }

        private void cbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbLanguage.SelectedIndex == 0)
            {
                Properties.Settings.Default.languageCode = "en-US";
                Properties.Settings.Default.Save();
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

                Login login = new Login();
                login.Show();
                this.Hide();
            }
            else
            {
                Properties.Settings.Default.languageCode = "es-MX";
                Properties.Settings.Default.Save();
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-MX");

                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }
    }
}
