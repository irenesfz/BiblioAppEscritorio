using BiblioAppEscritorio.ViewModel.LibrosVM;
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

namespace BiblioAppEscritorio.Vistas.VentanaLibros
{
    /// <summary>
    /// Lógica de interacción para DialogoEditaLibro.xaml
    /// </summary>
    public partial class DialogoEditaLibro : Window
    {
        DialogoEditaLibroVM vm;
        public DialogoEditaLibro()
        {
            vm = new DialogoEditaLibroVM();
            InitializeComponent();
            DataContext = vm;
        }
        private void ExaminaButton_Click(object sender, RoutedEventArgs e)
        {
            vm.ExaminarImagen();
        }

        private void GuardaButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            vm.EditaLibro();
        }

        private void CancelaButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
