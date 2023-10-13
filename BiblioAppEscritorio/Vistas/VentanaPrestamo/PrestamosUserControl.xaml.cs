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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiblioAppEscritorio.Vistas
{
    /// <summary>
    /// Lógica de interacción para PrestamosUserControl.xaml
    /// </summary>
    public partial class PrestamosUserControl : UserControl
    {
        PrestamosUserControlVM vm;
        public PrestamosUserControl()
        {
            vm = new PrestamosUserControlVM();
            InitializeComponent();
            DataContext = vm;
        }
    }
}
