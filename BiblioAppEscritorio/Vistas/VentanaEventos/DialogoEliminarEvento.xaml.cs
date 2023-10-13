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
    /// Lógica de interacción para DialogoEliminarEvento.xaml
    /// </summary>
    public partial class DialogoEliminarEvento : Window
    {
        DialogoEliminarEventoVM vm;
        public DialogoEliminarEvento()
        {
            vm = new DialogoEliminarEventoVM();
            InitializeComponent();
            DataContext = vm;
        }

        private void AcceptarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            vm.EliminaEvento();
        }

        private void CancelarButton_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
