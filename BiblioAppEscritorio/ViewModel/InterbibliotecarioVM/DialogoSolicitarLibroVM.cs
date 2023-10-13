using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.ExternalApi;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.ViewModel.InterbibliotecarioVM
{
    class DialogoSolicitarLibroVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;
        private Item resultadoSeleccionado;

        public Item ResultadoSeleccionado
        {
            get { return resultadoSeleccionado; }
            set { SetProperty(ref resultadoSeleccionado, value); }
        }
        private string imagen;

        public string Imagen
        {
            get { return imagen; }
            set { SetProperty(ref imagen, value); }
        }
        private string titulo;

        public string Titulo
        {
            get { return titulo; }
            set { SetProperty(ref titulo, value); }
        }

        private string isbn;

        public string Isbn
        {
            get { return isbn; }
            set { SetProperty(ref isbn, value); }
        }

        public DialogoSolicitarLibroVM()
        {
            servicioDialogos = new ServicioDialogos();
            ResultadoSeleccionado = WeakReferenceMessenger.Default.Send<PrestamoInterbibliotecarioMessage>();
            if (ResultadoSeleccionado.volumeInfo.imageLinks != null)
            {
                Imagen = ResultadoSeleccionado.volumeInfo.imageLinks.thumbnail;
            }
            else
            {
                Imagen = "https://images-na.ssl-images-amazon.com/images/I/211uDEb4KPL._SY264_BO1,204,203,200_QL40_ML2_.jpg";
            }
            if (ResultadoSeleccionado.volumeInfo.title != null)
            {
                Titulo = ResultadoSeleccionado.volumeInfo.title.ToString();
            }
            else
            {
                Titulo = "Título no disponible";
            }
            if (ResultadoSeleccionado.volumeInfo.industryIdentifiers != null)
            {
                Isbn = ResultadoSeleccionado.volumeInfo.industryIdentifiers[0].identifier.ToString();
            }
            else
            {
                Isbn = "ISBN no disponible";
            }

            api = new ApiService();
        }

        public void Solicita()
        {
            String mensaje = api.CreaRegistroInterbiblitecario(api.CreateSesion(), new PrestamoInterbibliotecario(false, 0, isbn, titulo));
            if (mensaje != "Prétamo interbibliotecario creado")
            {
                servicioDialogos.MensajeError("Error", "Error al crear préstamo");
            }
            else
            {
                servicioDialogos.MensajeInformativo("OK", "Prétamo interbibliotecario solicitado");
                WeakReferenceMessenger.Default.Send(new ActualizarPrestamoInterbibliotecarioMessage(new PrestamoInterbibliotecario(false, 0, isbn, titulo)));
            }
        }
    }
}
