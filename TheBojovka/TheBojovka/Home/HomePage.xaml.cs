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
using TheBojovka.Home;
using FileHelpers;
using System.Windows.Navigation;
using System.IO;

namespace TheBojovka
{
    /// <summary>
    /// Interakční logika pro HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        public HomePage()
        {
            InitializeComponent();
            MainGrid.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), GetFilePath("MainPageBCK.jpg")))); 

        }
        public string GetFilePath(string fileName)
        {
            string dirpath = @Directory.GetCurrentDirectory();
            var gparent = Directory.GetParent(Directory.GetParent(dirpath).ToString());
            string imgfolder = System.IO.Path.Combine(gparent.ToString(), @"Game\IMGFolder\");
            string FullFilePath = System.IO.Path.Combine(imgfolder, @fileName);
            return FullFilePath;

        }
        public string GetDBFilePath(string fileName)
        {
            string dirpath = @Directory.GetCurrentDirectory();
            var gparent = Directory.GetParent(Directory.GetParent(dirpath).ToString());
            string imgfolder = System.IO.Path.Combine(gparent.ToString(), @"Game\Database\");
            string FullFilePath = System.IO.Path.Combine(imgfolder, @fileName);
            return FullFilePath;

        }


        private void Button_Play(object sender, RoutedEventArgs e)
        {
            try
            {
                var engine = new FileHelperEngine<Player>();
                var Player = engine.ReadFile(GetDBFilePath("Player.txt"));

                var engines = new FileHelperEngine<Item>();
                var items = engines.ReadFile(GetDBFilePath("Items.txt"));

                List<Item> i = new List<Item>();
                for (int ee = 0; ee < items.Count(); ee++)
                {
                    i.Add(items[ee]);
                }


                Player p = new Player(Player[0].Name, Player[0].hp, Player[0].Level, Player[0].FrameID);
                Bojovka Bojovka = new Bojovka(p.FrameID, p, i);
                Bojovka.Show();
                Close();

            }
            catch
            {
                MessageBox.Show("Žádný pokrok nebyl nalezen! Prosím pokračujte kliknutím na ~Nová Hra~");
            }
         
        }


        private void Button_Options(object sender, RoutedEventArgs e)
        {
            Frejm.Content = new OptionsList();

        }

        private void Button_Controls(object sender, RoutedEventArgs e)
        {
            Frejm.Content = new Controls();
        }

        private void Button_Quit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Button_NewGame(object sender, RoutedEventArgs e)
        {
            Frejm.Content = new NewGame();
        }

    }
}
