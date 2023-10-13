using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.Clases
{
    class Reserva
    {
        public string fechaReserva { get; set; }
        public bool finalizada { get; set; }
        public int idReserva { get; set; }
        public int libroId { get; set; }
        public bool notificacion { get; set; }
        public int socioId { get; set; }
    }
}
