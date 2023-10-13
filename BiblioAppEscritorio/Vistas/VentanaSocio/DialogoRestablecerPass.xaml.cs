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

namespace BiblioAppEscritorio.Vistas.Socio
{
    /// <summary>
    /// Lógica de interacción para DialogoRestablecerPass.xaml
    /// </summary>
    public partial class DialogoRestablecerPass : Window
    {
        DialogoRestablecerPassVM vm;
        public DialogoRestablecerPass()
        {
            vm = new DialogoRestablecerPassVM();
            InitializeComponent();
            DataContext = vm;
        }

        private void AcceptarButton_Click(object sender, RoutedEventArgs e)
        {
            vm.RestablecePassword();
            DialogResult = true;
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
