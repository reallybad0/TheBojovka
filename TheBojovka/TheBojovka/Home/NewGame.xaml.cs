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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileHelpers;
using System.Media;
using System.IO;

namespace TheBojovka.Home
{
    /// <summary>
    /// Interakční logika pro NewGame.xaml
    /// </summary>
    public partial class NewGame : Page
    {
        public NewGame()
        {
            InitializeComponent();
            CNTNT.Background = new SolidColorBrush(Colors.White) { Opacity = 0.5 };
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            CNTNT.Visibility = Visibility.Hidden;

        }
        public string GetDBFilePath(string fileName)
        {
            string dirpath = @Directory.GetCurrentDirectory();
            var gparent = Directory.GetParent(Directory.GetParent(dirpath).ToString());
            string imgfolder = System.IO.Path.Combine(gparent.ToString(), @"Game\Database\");
            string FullFilePath = System.IO.Path.Combine(imgfolder, @fileName);
            return FullFilePath;

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            try
            {
                List<Item> i = new List<Item>();

                Player p = new Player(JmenoTB.Text, 100, 0, 0);

                var engine = new FileHelperEngine<Player>();
                var pal = new List<Player>();
                pal.Add(p);
                engine.WriteFile(GetDBFilePath("Player.txt"), pal);

                var engineItems = new FileHelperEngine<Item>();
                engineItems.WriteFile("Items.txt", i);
                MessageBox.Show("Podařilo se! Klikni na 'Pokračovat'");
                JmenoTB.Clear();

            }
            catch 
            {
                MessageBox.Show("Něco se pokazilo");
            }

            
        }


    }
}
