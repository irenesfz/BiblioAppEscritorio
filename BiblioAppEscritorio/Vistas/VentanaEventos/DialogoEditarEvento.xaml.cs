using BiblioAppEscritorio.ViewModel.Eventos;
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

namespace BiblioAppEscritorio.Vistas.Eventos
{
    /// <summary>
    /// Lógica de interacción para DialogoEditarEvento.xaml
    /// </summary>
    public partial class DialogoEditarEvento : Window
    {
        DialogoEditarEventoVM vm;
        public DialogoEditarEvento()
        {
            vm = new DialogoEditarEventoVM();
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.ExaminarImagen();
        }

        private void EditaButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            vm.EditaEvento();
        }
    }
}
