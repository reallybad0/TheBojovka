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

namespace TheBojovka
{
    /// <summary>
    /// Interakční logika pro Bojovka.xaml
    /// </summary>
    public partial class Bojovka : Window
    {
        public Bojovka(int Frame,Player player,List<Item> items)
        {
            InitializeComponent();
            MainFrame.Content = new FR(Frame,player,items);
        }
    }
}
