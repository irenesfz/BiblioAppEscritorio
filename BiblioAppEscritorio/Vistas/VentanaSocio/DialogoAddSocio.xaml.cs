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
    /// Lógica de interacción para AddSocioWindow.xaml
    /// </summary>
    public partial class DialogoAddSocio : Window
    {
        DialogoAddSocioVM vm;
        public DialogoAddSocio()
        {
            vm = new DialogoAddSocioVM();
            InitializeComponent();
            DataContext = vm;
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e)
        {
            vm.AddSocio();
            DialogResult = true;
        }

        private void ExaminarButton_Click(object sender, RoutedEventArgs e)
        {
            vm.ExaminaSocio();
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
