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
    /// Lógica de interacción para SociosUserControl.xaml
    /// </summary>
    public partial class SociosUserControl : UserControl
    {
        SociosUserControlVM vm;
        public SociosUserControl()
        {
            vm = new SociosUserControlVM();
            InitializeComponent();
            DataContext = vm;
        }
    }
}
