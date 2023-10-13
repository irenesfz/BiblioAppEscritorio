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
    /// Lógica de interacción para AddEventoWindow.xaml
    /// </summary>
    public partial class AddEventoWindow : Window
    {
        AddEventoWindowVM vm;
        public AddEventoWindow()
        {
            vm = new AddEventoWindowVM();
            InitializeComponent();
            DataContext = vm;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.ExaminarImagen();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            vm.AddEvento();
            DialogResult = true;
        }
    }
}
