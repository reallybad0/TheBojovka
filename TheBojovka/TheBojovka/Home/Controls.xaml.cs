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

namespace TheBojovka
{
    /// <summary>
    /// Interakční logika pro Controls.xaml
    /// </summary>
    public partial class Controls : Page
    {
        public Controls()
        {
            InitializeComponent();
            CNTNT.Background = new SolidColorBrush(Colors.White) { Opacity = 0.5 };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

                CNTNT.Visibility = Visibility.Hidden;
            
        }
    }
}
