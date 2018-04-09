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
        //FILEWORKS ~* * 
        #region
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
        public TheBojovka.Player[] GetPlayer()
        {
            var engine = new FileHelperEngine<Player>();
            var Scenes = engine.ReadFile(GetDBFilePath("Player.txt"));
            return Scenes;
        }
        //Images 
        public IMG ReturnNpcUri(int ID)
        {
            IMG someimg = new IMG();
            if (ID > 45 && ID < 53 ||ID == 122 ) { someimg = new IMG(ID, GetFilePath(50 + "NPC.gif")); }
            else if (ID > 52 && ID < 66 || ID == 126) { someimg = new IMG(ID, GetFilePath(60 + "NPC.gif")); }
            else if (ID > 66 && ID < 81 || ID == 123) { someimg = new IMG(ID, GetFilePath(70 + "NPC.gif")); }
            else if (ID > 80 && ID < 87 || ID == 125) { someimg = new IMG(ID, GetFilePath(80 + "NPC.gif")); }
            else if (ID > 86 && ID < 98 || ID ==127) { someimg = new IMG(ID, GetFilePath(90 + "NPC.gif")); }
            else if (ID > 97 && ID < 109 || ID ==128) { someimg = new IMG(ID, GetFilePath(100 + "NPC.gif")); }
            else if (ID > 108 && ID < 118 || ID ==124 ) { someimg = new IMG(ID, GetFilePath(110 + "NPC.gif")); }
            else
            {
                someimg = new IMG(ID, GetFilePath(ID + "NPC.gif"));
            }
            return someimg;
        }
        public IMG ReturnBackgroundUri(int ID)
        {
            IMG someimg = new IMG();
            if (ID > 45 && ID < 53 || ID == 122) { someimg = new IMG(ID, GetFilePath(50 + ".gif")); }
            else if (ID > 52 && ID < 66 || ID == 126) { someimg = new IMG(ID, GetFilePath(60 + ".gif")); }
            else if (ID > 66 && ID < 81 || ID == 123) { someimg = new IMG(ID, GetFilePath(70 + ".gif")); }
            else if (ID > 80 && ID < 87 || ID == 125) { someimg = new IMG(ID, GetFilePath(80 + ".gif")); }
            else if (ID > 86 && ID < 98 || ID == 127) { someimg = new IMG(ID, GetFilePath(90 + ".gif")); }
            else if (ID > 97 && ID < 109 || ID == 128) { someimg = new IMG(ID, GetFilePath(100 + ".gif")); }
            else if (ID > 108 && ID < 118 || ID == 124) { someimg = new IMG(ID, GetFilePath(110 + ".gif")); }
            else
            {
                someimg = new IMG(ID, GetFilePath(ID + ".gif"));
            }
            return someimg;

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
        #endregion

        //Filling Scenes
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
                    IMG imageuri = new IMG();
                    if (s.ID> 45 && s.ID < 53) { imageuri = new IMG(s.ID, GetFilePath(SceneList[i].ID + ".gif")); }
                    else if (s.ID > 52 && s.ID < 66) { imageuri = new IMG(s.ID, GetFilePath(SceneList[i].ID + ".gif")); }
                    else if (s.ID > 66 && s.ID < 81) { imageuri = new IMG(s.ID, GetFilePath(SceneList[i].ID + ".gif")); }
                    else if (s.ID > 80 && s.ID < 87) { imageuri = new IMG(s.ID, GetFilePath(SceneList[i].ID + ".gif")); }
                    else if (s.ID > 86 && s.ID < 98) { imageuri = new IMG(s.ID, GetFilePath(SceneList[i].ID + ".gif")); }
                    else if (s.ID > 97 && s.ID < 109) { imageuri = new IMG(s.ID, GetFilePath(SceneList[i].ID + ".gif")); }
                    else if (s.ID > 108 && s.ID < 118) { imageuri = new IMG(s.ID, GetFilePath(SceneList[i].ID + ".gif")); }
                    else
                    {
                        imageuri = new IMG(s.ID, GetFilePath(SceneList[i].ID + ".gif"));
                    }

                  //  IMG imageuri = new IMG(s.ID, GetFilePath(SceneList[i].ID + ".gif"));
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
                IMG NPCuri = ReturnNpcUri(Scene.ID);
                imageuri = ReturnBackgroundUri(Scene.ID);
                FillBackgroundImage(imageuri);
                FillNPCImage(NPCuri);
                Description.Text = Scene.Description;
             
                FillOptions(Scene.ID, Scene.OptionCount);
            }
            //Casual scene
            else if (Scene.Type == 4 || Scene.Type ==1)
            {
                imageuri = ReturnBackgroundUri(Scene.ID);
                FillBackgroundImage(imageuri);
                Description.Text = Scene.Description;
                
                FillOptions(Scene.ID, Scene.OptionCount);
            }
            //Fight
            else if (Scene.Type == 5)

            {
                IMG NPCuri = ReturnNpcUri(Scene.ID);
                imageuri = ReturnBackgroundUri(Scene.ID);
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
            //
            else
            {
                imageuri = ReturnBackgroundUri(Scene.ID);
                FillBackgroundImage(ReturnBackgroundUri(Scene.ID));
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

            if (File.Exists(imageuri.Adress))
            {
                image.UriSource = new Uri(imageuri.Adress);
            }
            else
            {
                image.UriSource = new Uri(GetFilePath(4 + ".gif"));
            }
            image.EndInit();

            //ImageBehavior.SetRepeatBehavior(image,);
            ImageBehavior.SetAnimatedSource(img, image);
            ImageHere.Children.Clear();
            ImageHere.Children.Add(img);

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
                    alive = false;
                }
                else if (CurrentPlayer[0].hp <= 0)
                {
                    alive = false;
                }
                else
                {
                    

                    PGB.Value = CurrentPlayer[0].hp;
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



                    MessageBoxResult result = MessageBox.Show("Přímmý zásah!\nChceš v boji pokračovat?", "Souboj", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                             break;
                        case MessageBoxResult.No:
                            MessageBox.Show("Utíkáš z boje...", "Útěk");
                            winner = 2;
                            alive = false;
                            //alive = false;
                            break;
                    }

                }
            }
            if (Monster.HP <= 0)
            {
                winner = 1;
                
            }
            else if (CurrentPlayer[0].hp <= 0)
            {
                winner = 0;
            
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
            else if(winner == 0)
            {
                MessageBox.Show("Jsi mrtvý :( ");
                Application.Current.Shutdown();
            }
            else if(winner == 2)
            {
                EnemyProgressBar.Visibility = Visibility.Hidden;
                Versus.Visibility = Visibility.Hidden;

            }
        }
        public int HodKostkou(int level)
        {
            int RandomNumber = random.Next(level, 10);
            return RandomNumber;
        }

        //Bottom Menu Clicks
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

                engine.WriteFile(GetDBFilePath("Player.txt"), pal);

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
        public void PlaySound(int ID)
        {
            int soundID;
            if (ID < 15) { soundID = 0; }
            else if (ID > 52 && ID < 66 || ID == 126) { soundID = 0; }
            else if (ID > 66 && ID < 81 || ID == 123) { soundID = 0; }
            else if (ID > 80 && ID < 87 || ID == 125) { soundID = 0; }
            else if (ID > 86 && ID < 98 || ID == 127) { soundID = 0; }
            else if (ID > 97 && ID < 109 || ID == 128) { soundID = 0; }
            else if (ID > 108 && ID < 118 || ID == 124) { soundID = 0; }
            else if (ID == 126 || ID == 127 || ID == 128 || ID == 123 || ID == 124 || ID == 125)
            {
                soundID = 4;
            }
            else
            {
                soundID = 1;
            }


            //return someimg;
            if (File.Exists(GetSoundFilePath(ID+".wav")))
            {
                //SoundPlayer player = new SoundPlayer(GetSoundFilePath("1.wav"));
                SPlayer.SoundLocation = GetSoundFilePath(ID+".wav");
                SPlayer.Play();
            }
            else
            {
                SPlayer.Stop();
            }

        }
    }
}

