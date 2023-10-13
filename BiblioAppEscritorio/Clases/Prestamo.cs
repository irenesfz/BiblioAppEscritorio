using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.Clases
{
    class Prestamo
    {
        public string fechaDevolucion { get; set; }
        public string fechaPrestamo { get; set; }
        public string fechaTope { get; set; }
        public int idLibro { get; set; }
        public int idPrestamo { get; set; }
        public int idSocio { get; set; }

        public Prestamo(string fechaDevolucion, string fechaPrestamo, string fechaTope, int idLibro, int idPrestamo, int idSocio)
        {
            this.fechaDevolucion = fechaDevolucion;
            this.fechaPrestamo = fechaPrestamo;
            this.fechaTope = fechaTope;
            this.idLibro = idLibro;
            this.idPrestamo = idPrestamo;
            this.idSocio = idSocio;
        }

    }
}
