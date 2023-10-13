using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.Clases
{
    class GridReservas
    {
        public string dniSocio { get; set; }
        public string fechaReserva { get; set; }
        public bool finalizada { get; set; }
        public int idLibro { get; set; }
        public int idReserva { get; set; }
        public string imagenPortada { get; set; }
        public string isbn { get; set; }
        public string nombreSocio { get; set; }
        public string tituloLibro { get; set; }

    }
}
