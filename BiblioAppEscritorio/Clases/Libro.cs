using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.Clases
{
    class Libro : ObservableObject
    {
        private int idLibro;

        public int IdLibro
        {
            get { return idLibro; }
            set { SetProperty(ref idLibro, value); }
        }
        private string isbn;

        public string Isbn
        {
            get { return isbn; }
            set { SetProperty(ref isbn, value); }
        }
        private bool disponible;

        public bool Disponible
        {
            get { return disponible; }
            set { SetProperty(ref disponible, value); }
        }
        private bool reservado;

        public bool Reservado
        {
            get { return reservado; }
            set { SetProperty(ref reservado, value); }
        }

        public Libro(int idLibro, string isbn, bool disponible, bool reservado)
        {
            IdLibro = idLibro;
            Isbn = isbn;
            Disponible = disponible;
            Reservado = reservado;
        }
    }
}
