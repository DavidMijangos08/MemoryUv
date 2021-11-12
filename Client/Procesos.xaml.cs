using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Client
{
    /// <summary>
    /// Lógica de interacción para Procesos.xaml
    /// </summary>
    public partial class Procesos : Window
    {

        //MediaPlayer mediaPlayer = new MediaPlayer();
        //string filename = "/Resources/Sound/GameSoundtrack.mp3";

        private MediaPlayer mediaPlayer = new MediaPlayer();
        public Procesos()
        {
            
            InitializeComponent();
            //InitializeMusic();
            //test();
        }

        public void InitializeMusic()
        {
            /*OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            
                mediaPlayer.Open(new Uri(filename));
                mediaPlayer.Play();
            */
            /*SoundPlayer player = new SoundPlayer("/Resources/Sound/song.wav");
            player.LoadAsync();
            player.Play(); */

            SoundPlayer player = new SoundPlayer();
            player.Stream = Properties.Resources.song;
            try
            {
                player.Load();
                player.Play();
            }
            catch (Exception E) { }

        }
        public void test()
        {
            /*SoundPlayer player = new SoundPlayer("/Resources/Sound/song.wav");
            player.Load();
            player.Play();*/
            MediaPlayer Sound1 = new MediaPlayer();
            Sound1.Open(new Uri(@"Resources/Sound/song.wav"));
            Sound1.Play();
        }

        private void btnOpenAudioFile_Click(object sender, RoutedEventArgs e)
        {
            /*OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
                mediaPlayer.Play();  */

            /*SoundPlayer player = new SoundPlayer();
            var direct = Environment.CurrentDirectory + "Resources/Sound/song.wav";
            player.SoundLocation = direct;
            try
            {
                player.Load();
                player.Play();
            }
            catch (Exception E) { } */

            

        }
    }
}
