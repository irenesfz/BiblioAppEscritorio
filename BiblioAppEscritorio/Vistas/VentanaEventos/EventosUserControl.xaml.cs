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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiblioAppEscritorio.Vistas
{
    /// <summary>
    /// Lógica de interacción para EventosUserControl.xaml
    /// </summary>
    public partial class EventosUserControl : UserControl
    {
        EventosUserControlVM vm;
        public EventosUserControl()
        {
            vm = new EventosUserControlVM();
            InitializeComponent();
            DataContext = vm;
        }
    }
}
