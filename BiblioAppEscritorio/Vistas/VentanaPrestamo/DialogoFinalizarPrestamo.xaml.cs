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

namespace BiblioAppEscritorio.ViewModel
{
    /// <summary>
    /// Lógica de interacción para DialogoFinalizarPrestamo.xaml
    /// </summary>
    public partial class DialogoFinalizarPrestamo : Window
    {
        DialogoFinalizarPrestamoVM vm;
        public DialogoFinalizarPrestamo()
        {
            vm = new DialogoFinalizarPrestamoVM();
            InitializeComponent();
            DataContext = vm;
        }
        private void AcceptarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            vm.FinalizaPrestamo();
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
