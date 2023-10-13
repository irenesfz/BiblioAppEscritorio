using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.Clases
{
    class GridPrestamos
    {
        public string dniSocio { get; set; }
        public string fechaDevolucion { get; set; }
        public string fechaPrestamo { get; set; }
        public string fechaTope { get; set; }
        public int idLibro { get; set; }
        public int idPrestamo { get; set; }
        public string imagenPortada { get; set; }
        public string isbn { get; set; }
        public string nombreSocio { get; set; }
        public string tituloLibro { get; set; }

        public GridPrestamos(string dniSocio, 
            string fechaDevolucion, 
            string fechaPrestamo, 
            string fechaTope, int idLibro, 
            int idPrestamo, string imagenPortada, 
            string isbn, string nombreSocio, 
            string tituloLibro)
        {
            this.dniSocio = dniSocio;
            this.fechaDevolucion = fechaDevolucion;
            this.fechaPrestamo = fechaPrestamo;
            this.fechaTope = fechaTope;
            this.idLibro = idLibro;
            this.idPrestamo = idPrestamo;
            this.imagenPortada = imagenPortada;
            this.isbn = isbn;
            this.nombreSocio = nombreSocio;
            this.tituloLibro = tituloLibro;
        }
    }
}
