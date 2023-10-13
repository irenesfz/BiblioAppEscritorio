using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.Clases
{
    class PrestamoInterbibliotecario
    {
        public bool finalizada { get; set; }
        public int idInterbibliotecario { get; set; }
        public string isbn { get; set; }
        public string titulo { get; set; }

        public PrestamoInterbibliotecario(bool finalizada, int idInterbibliotecario, string isbn, string titulo)
        {
            this.finalizada = finalizada;
            this.idInterbibliotecario = idInterbibliotecario;
            this.isbn = isbn;
            this.titulo = titulo;
        }
    }
}
