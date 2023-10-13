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

namespace BiblioAppEscritorio.Vistas.Libros
{
    /// <summary>
    /// Lógica de interacción para DialogoCreaLibro.xaml
    /// </summary>
    public partial class DialogoCreaLibro : Window
    {
        DialogoCreaLibroVM vm;
        public DialogoCreaLibro()
        {
            vm = new DialogoCreaLibroVM();
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
            vm.CreaLibro();
        }

        private void CancelaButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            vm.BuscaLibros();
        }
    }
}
