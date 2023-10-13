using BiblioAppEscritorio.ViewModel.Interbibliotecario;
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

namespace BiblioAppEscritorio.Vistas.VentanaPrestamoInterbibliotecario
{
    /// <summary>
    /// Lógica de interacción para PrestamoInterbibliotecarioUserControl.xaml
    /// </summary>
    public partial class PrestamoInterbibliotecarioUserControl : UserControl
    {
        PrestamoInterbibliotecarioVM vm;
        public PrestamoInterbibliotecarioUserControl()
        {
            vm = new PrestamoInterbibliotecarioVM();
            InitializeComponent();
            DataContext = vm;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            vm.BuscaLibros();
        }
    }
}
