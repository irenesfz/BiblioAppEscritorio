using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.Clases
{
    class GridSocios : ObservableRecipient
    {
        public ObservableCollection<String> prestamosSocio { get; set; }
        public ObservableCollection<String> reservasSocio { get; set; }
        public Socio socio { get; set; }
    }
}
