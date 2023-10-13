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
    /// Lógica de interacción para DialogoCreaPrestamo.xaml
    /// </summary>
    public partial class DialogoCreaPrestamo : Window
    {
        DialogoCreaPrestamoVM vm;
        public DialogoCreaPrestamo()
        {
            vm = new DialogoCreaPrestamoVM();
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            vm.CreaPrestamo();
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
