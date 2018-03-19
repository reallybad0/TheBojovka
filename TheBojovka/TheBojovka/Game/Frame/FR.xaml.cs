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
using System.IO;
using System.Collections.ObjectModel;
using WpfAnimatedGif;
using System.Diagnostics;
using FileHelpers;
using System.Windows.Media.Animation;
using System.Threading;
using System.Reflection;
using System.Media;

namespace TheBojovka
{
    /// <summary>
    /// Interakční logika pro FR.xaml
    /// </summary>
    public partial class FR : Page
    {
        ObservableCollection<Player> CurrentPlayer = new ObservableCollection<Player>();
        ObservableCollection<Item> InventoryCollection = new ObservableCollection<Item>();
        Random random = new Random();
        SoundPlayer SPlayer = new SoundPlayer();
        public FR(int SceneID, Player p, List<Item> i)
        {
            InitializeComponent();
            PlaySound(1);
            #region
            CurrentPlayer.Add(p);
            Versus.Visibility = Visibility.Hidden;
            EnemyProgressBar.Visibility = Visibility.Hidden;
            for (int r = 0; r < i.Count(); r++) { InventoryCollection.Add(i[r]); }

            PlayerName.Content = p.Name;
            PlayerLVL.Content = "LVL " + p.Level;
            PGB.Value = p.hp;
            #endregion
            var Scenes = GetScenes();

            Scene s = new Scene(SceneID, Scenes[SceneID].Description, Scenes[SceneID].Type, Scenes[SceneID].OptionCount);
            IMG imageuri = new IMG(s.ID, GetFilePath(Scenes[SceneID].ID + ".gif"));
            FillScene(s, imageuri);
        }
        //Collections
        public TheBojovka.Option[] GetOptions()
        {
            var engineOpts = new FileHelperEngine<Option>();
            var OptionsList = engineOpts.ReadFile(GetDBFilePath("Options.csv"));
            return OptionsList;
        }
        public TheBojovka.Scene[] GetScenes()
        {
            var engine = new FileHelperEngine<Scene>();
            var Scenes = engine.ReadFile(GetDBFilePath("LocationsCS.csv"));
            return Scenes;
        }
        public TheBojovka.NPC[] GetNpc()
        {
            var engine = new FileHelperEngine<NPC>();
            var Scenes = engine.ReadFile(GetDBFilePath("NPC.csv"));
            return Scenes;
        }

        //Fillings 
        protected void FillAfterOpt(object sender, EventArgs e)
        {
            int FollowingScene = 0;
            Button button = sender as Button;
            string ct = button.Content.ToString();

            var SceneList = GetScenes();
            var OptionsList = GetOptions();

            for (int l = 0; l < OptionsList.Count(); l++)
            {
                if (OptionsList[l].Description == ct)
                {
                    FollowingScene = OptionsList[l].FollowingScene;
                }
            }
            for (int i = 0; i < SceneList.Count(); i++)
            {
                if (SceneList[i].ID == FollowingScene)
                {
                    Scene s = new Scene(SceneList[i].ID, SceneList[i].Description, SceneList[i].Type, SceneList[i].OptionCount);
                    IMG imageuri = new IMG(s.ID, GetFilePath(SceneList[i].ID + ".gif"));
                    FillScene(s, imageuri);
                }
                
            }

        }
       
        public void FillOptions(int SceneID, int limit)
        {
            ButtonPanel.Children.Clear();
            
            int margin = 0;
            var OptionsList = GetOptions();

            for (int r = 0; r < OptionsList.Count() - 1; r++)
            {
                if (OptionsList[r].Scene_ID == SceneID)
                {
                    margin = margin + 20;
                    Button b = new Button();
                    b.Padding = new Thickness(4);
                    b.Content = OptionsList[r].Description;
                    b.Margin = new Thickness(margin, 0, 0, 0);
                    b.Click += new RoutedEventHandler(FillAfterOpt);
                    ButtonPanel.Children.Add(b);


                }
            }
        }
        public void FillScene(Scene Scene, IMG imageuri)
        {
            CurrentPlayer[0].FrameID = Scene.ID;
            PlaySound(Scene.ID);
            //NPC
            if (Scene.Type == 2)
            {
                
                IMG NPCuri = new IMG(Scene.ID, GetFilePath(Scene.ID + "NPC.gif"));
                
                FillBackgroundImage(imageuri);
                FillNPCImage(NPCuri);
                Description.Text = Scene.Description;
             
                FillOptions(Scene.ID, Scene.OptionCount);
            }
            else if (Scene.Type == 4)
            {
                FillBackgroundImage(imageuri);
                Description.Text = Scene.Description;
                
                FillOptions(Scene.ID, Scene.OptionCount);
            }
            //BOJ
            else if (Scene.Type == 5)

            {
                IMG NPCuri = new IMG(Scene.ID, GetFilePath(Scene.ID + "NPC.gif"));

                FillBackgroundImage(imageuri);
                FillNPCImage(NPCuri);
                Description.Text = Scene.Description;
                FillOptions(Scene.ID, Scene.OptionCount);

                var NPCS = GetNpc();
                NPC Monster = new NPC();

                try
                {
                    for (int u = 0; u < NPCS.Count(); u++)
                    {
                        if (NPCS[u].ID == Scene.ID)
                        {
                            Monster = NPCS[u];
                        }
                    }
                    Fight(Monster, CurrentPlayer[0]);
                }
                catch
                {

                    MessageBox.Show("NE, TOHLE NEPŮJDE?");
                    MessageBox.Show(Monster.ID + " " + Monster.HP);

                }

                
            }      
                     
            else
            {
                FillBackgroundImage(imageuri);
                Description.Text = Scene.Description;
                
                FillOptions(Scene.ID, Scene.OptionCount);
            }
        }
        public void FillNPCImage(IMG imageuri)
        {
            Image img = new Image();

            img.Width = 300;
            img.Height = 300;
            img.VerticalAlignment = VerticalAlignment.Bottom;
            img.HorizontalAlignment = HorizontalAlignment.Center;

            var image = new BitmapImage();
            image.BeginInit();

            image.UriSource = new Uri(imageuri.Adress);
            image.EndInit();
            
            ImageBehavior.SetAnimatedSource(img, image);
            ImageHere.Children.Add(img);
            //ImageBehavior.SetRepeatBehavior(img, new RepeatBehavior(1));
        }

        public void FillBackgroundImage(IMG imageuri)
        {
            Image img = new Image();
            img.Margin = new Thickness(0, 0, 0, 0);
            img.Width = 750;
            img.Height = 750;
            img.VerticalAlignment = VerticalAlignment.Top;
            img.HorizontalAlignment = HorizontalAlignment.Center;

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(imageuri.Adress);
            image.EndInit();

            //ImageBehavior.SetRepeatBehavior(image,);
            ImageBehavior.SetAnimatedSource(img, image);
            ImageHere.Children.Clear();
            ImageHere.Children.Add(img);

        }
        //Get Paths
        public string GetFilePath(string fileName)
        {
   
            string dirpath = @Directory.GetCurrentDirectory();
            var gparent = Directory.GetParent(Directory.GetParent(dirpath).ToString());
            string imgfolder = System.IO.Path.Combine(gparent.ToString(), @"Game\IMGFolder\");
            string FullFilePath = System.IO.Path.Combine(imgfolder, @fileName);
            return FullFilePath;

        }

        public string GetSoundFilePath(string fileName)
        {
            string dirpath = @Directory.GetCurrentDirectory();
            var gparent = Directory.GetParent(Directory.GetParent(dirpath).ToString());
            string imgfolder = System.IO.Path.Combine(gparent.ToString(), @"Game\Sounds\");
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
        //FIGHT
        public void Fight(NPC Monster, Player Player)
        {
            
            EnemyProgressBar.Visibility = Visibility.Visible;
            Versus.Visibility = Visibility.Visible;
            EnemyProgressBar.Maximum = Monster.HP;
            EnemyProgressBar.Value = Monster.HP;

            bool alive = true;

            //kdo vyhrál ::  0 = monstrum, 1 = hráč
            int winner = 0;


            while (alive)
            {
                if (Monster.HP <= 0)
                {
                    winner = 1;
                    alive = false;
                }
                else if (CurrentPlayer[0].hp <= 0)
                {
                    winner = 0;
                    alive = false;
                }
                else
                {
                    
                    Description.Text = Monster.HP.ToString();
                    //MessageBox.Show("");
                    //Fill progress bar? 
                    //Fill button na hod / útěk

                    PGB.Value = CurrentPlayer[0].hp;
                    //MessageBox.Show("FIGHT");
                    int MonsterThrow = HodKostkou(Monster.Level);
                    int PlayerThrow = HodKostkou(CurrentPlayer[0].Level);

                    Description.Text = " Hodil jsi  " + PlayerThrow + ". Monstrum " + Monster.Name + " Hodilo na své kostce " + MonsterThrow + ".";
                    
                    int playerdefense;
                    if (CurrentPlayer[0].Level >= 3)
                    {
                        playerdefense = 0;
                    }
                    else
                    {
                        playerdefense = 3 - CurrentPlayer[0].Level;
                    }
                    int minusPlayer = MonsterThrow - playerdefense;
                    CurrentPlayer[0].hp = CurrentPlayer[0].hp - (minusPlayer);
                    
                    
                    //MessageBox.Show(CurrentPlayer[0].hp.ToString());
                    int minus;
                    if (PlayerThrow > Monster.Defense)
                    {
                        minus = PlayerThrow - Monster.Defense;
                    }
                    else
                    {
                        minus = (PlayerThrow - Monster.Defense) * -1;
                    }
                    Description.Text += "\n Celkem jsi monstru ubral "+ minus +" zdraví. Ty jsi ztratil " + minusPlayer;
                    Monster.HP = Monster.HP - (minus);
                    EnemyProgressBar.Value = Monster.HP;
                    MessageBox.Show("OK");
                }
            }

            if (winner == 1)
            {
                MessageBox.Show("Vyhrál jsi!");
                CurrentPlayer[0].Level += 1;
                PlayerLVL.Content = "LVL " + CurrentPlayer[0].Level;
                EnemyProgressBar.Visibility = Visibility.Hidden;
                Versus.Visibility = Visibility.Hidden;
                //FILLSCENE()
            }
            else
            {
                MessageBox.Show("Jsi ded :( ");
                Application.Current.Shutdown();
            }
        }
        public int HodKostkou(int level)
        {
            int RandomNumber = random.Next(level, 10);
            return RandomNumber;
        }

        //CLICKS
        #region
        private void InvShow_Click(object sender, RoutedEventArgs e)
        {
            InventoryMenu.ItemsSource = InventoryCollection;
        //   InventoryMenu.Items   = = = =IsCheckable = true;
        }
        private void ExitNoSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Opravdu si přeješ odejít bez uloženÍ?", "Bez uložení", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                MessageBox.Show("Příště opatrně! >:( ");
            }
            else if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Snad se brzy uvidíme...");
                Application.Current.Shutdown();
            }
        }
        private void ExitSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Player save = new Player();
                var engine = new FileHelperEngine<Player>();
                var pal = new List<Player>();
                pal.Add(CurrentPlayer[0]);
                engine.WriteFile("Player.txt", pal);
                MessageBox.Show("Pokrok byl uložen!");
                MessageBox.Show("Pomalu usínáš na mechovém podnose... ~");
                Application.Current.Shutdown();
            }
            catch
            {
                MessageBox.Show("Něco se pokazilo");
            }

        }
        #endregion

        //MUSIC
        public void PlaySound(int SceneID)
        {

            if (File.Exists(GetSoundFilePath(SceneID+".wav")))
            {
                //SoundPlayer player = new SoundPlayer(GetSoundFilePath("1.wav"));
                SPlayer.SoundLocation = GetSoundFilePath(SceneID+".wav");
                SPlayer.Play();
            }
            else
            {
                SPlayer.Stop();
            }

        }
    }
}

