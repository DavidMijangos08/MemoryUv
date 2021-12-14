﻿using Data;
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
        string language = "es-MX";
        public Login()
        {
            InitializeComponent();
            language = Properties.Settings.Default.languageCode;
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
                        if (language.Equals("es-MX"))
                        {
                            MessageBox.Show("Correo o contraseña incorrecta, revisa bien!");
                        }
                        else
                        {
                            MessageBox.Show("Incorrect email or password, check well!");
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
                    MessageBox.Show("Existe campo vacio o selección faltante");
                }
                else
                {
                    MessageBox.Show("There is an empty field or missing selection");
                }
            }
        }

        private bool ExistsInvalidPassword(string password)
        {
            bool exists = false;
            Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)\\S{8,15}$");
            if (!regex.IsMatch(password))
            {
                exists = true;
                if (language.Equals("es-MX"))
                {
                    MessageBox.Show("Caracteres inválidos");
                }
                else
                {
                    MessageBox.Show("Invalid characters");
                }              
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

        private void CbLanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
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
