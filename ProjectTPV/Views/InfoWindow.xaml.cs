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

namespace ProjectTPV.Views {
    /// <summary>
    /// Lógica de interacción para InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window {
        public InfoWindow() {
            InitializeComponent();
        }

        private void BotonCerrar_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Hide();
        }
    }
}
