using Data;
using Host;
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

namespace Client
{
    /// <summary>
    /// Lógica de interacción para Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        UserGame usergame = new UserGame();
        MemoryServer service;


        public Game(List<UserGame> users, string section, string difficulty)
        {
            InitializeComponent();
        }

        private void changeTextBlock(int lb)
        {
            if (lb == 0)
            {
                //lbUserTurn.Text = userSelected.nametag;
            }
            if (lb == 1)
            {
                lbUserTurn.Text = usergame.nametag;
            }
        }

        
    }
}
