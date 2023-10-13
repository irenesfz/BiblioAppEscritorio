using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.Clases
{
    class Libros
    {
        public int anyoPublicacion { get; set; }
        public string autores { get; set; }
        public string categoria { get; set; }
        public string descripcion { get; set; }
        public string editorial { get; set; }
        public string idioma { get; set; }
        public string imagen { get; set; }
        public string isbn { get; set; }
        public int paginas { get; set; }
        public string subcategorias { get; set; }
        public string titulo { get; set; }

        public Libros() { }

        public Libros(int anyoPublicacion, 
            string autores, string categoria, 
            string descripcion, string editorial, 
            string idioma, string imagen, string isbn, 
            int paginas, string subcategorias, string titulo)
        {
            this.anyoPublicacion = anyoPublicacion;
            this.autores = autores;
            this.categoria = categoria;
            this.descripcion = descripcion;
            this.editorial = editorial;
            this.idioma = idioma;
            this.imagen = imagen;
            this.isbn = isbn;
            this.paginas = paginas;
            this.subcategorias = subcategorias;
            this.titulo = titulo;
        }
    }
}
