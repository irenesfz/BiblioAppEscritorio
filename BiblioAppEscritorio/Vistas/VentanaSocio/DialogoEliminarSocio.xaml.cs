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
    /// Lógica de interacción para DialogoEliminarSocio.xaml
    /// </summary>
    public partial class DialogoEliminarSocio : Window
    {
        DialogoEliminarSocioVM vm;
        public DialogoEliminarSocio()
        {
            vm = new DialogoEliminarSocioVM();
            InitializeComponent();
            DataContext = vm;
        }

        private void AcceptarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            vm.EliminarSocio();
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
