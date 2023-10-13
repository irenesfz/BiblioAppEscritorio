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

namespace BiblioAppEscritorio.Vistas
{
    /// <summary>
    /// Lógica de interacción para DialogoEliminarPrestamo.xaml
    /// </summary>
    public partial class DialogoEliminarPrestamo : Window
    {
        DialogoEliminarPrestamoVM vm;
        public DialogoEliminarPrestamo()
        {
            vm = new DialogoEliminarPrestamoVM();
            InitializeComponent();
            DataContext = vm;
        }
        private void AcceptarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            vm.EliminaPrestamo();
        }

        private void CancelarButton_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
