using BiblioAppEscritorio.ViewModel;
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

namespace BiblioAppEscritorio.Vistas.Libros
{
    /// <summary>
    /// Lógica de interacción para DialogoCreaReserva.xaml
    /// </summary>
    public partial class DialogoCreaReserva : Window
    {
        DialogoCreaReservaVM vm;
        public DialogoCreaReserva()
        {
            vm = new DialogoCreaReservaVM();
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            vm.CreaReserva();
        }

        private void CancelarButton_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            vm.BuscaSocio();
        }
    }
}
