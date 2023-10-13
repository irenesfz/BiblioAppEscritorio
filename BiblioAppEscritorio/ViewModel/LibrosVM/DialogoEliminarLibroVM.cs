using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio
{
    class DialogoEliminarLibroVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ApiService api;
        private readonly ServicioDialogos servicioDialogos;
        private Libros libroAEliminar;

        public Libros LibroAEliminar
        {
            get { return libroAEliminar; }
            set { SetProperty(ref libroAEliminar, value); }
        }
        private int? unidades;

        public int? Unidades
        {
            get { return unidades; }
            set { SetProperty(ref unidades, value); }
        }

        public DialogoEliminarLibroVM()
        {
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();
            LibroAEliminar = WeakReferenceMessenger.Default.Send<LibroSeleccionadoMessage>();
            Unidades = api.GetUnidadesTotalesLibroPorId(LibroAEliminar.isbn);
        }
        public void EliminarLibro()
        {
            string resultado = "";
            int numeroUnidades = api.GetUnidadesTotalesLibroPorId(LibroAEliminar.isbn);

            if (numeroUnidades > 0)
            {
                resultado = api.EliminaTodasLasUnidadesDeLibro(api.GetElementoSeguridad(), LibroAEliminar.isbn);
                if (resultado != "Error genérico")
                {
                    DeleteApi();
                }
            }
            else
            {
                DeleteApi();
            }
        }
        private void DeleteApi()
        {
            String mensaje = api.EliminaLibros(api.CreateSesion(), LibroAEliminar.isbn);
            if (mensaje != "Libro eliminado")
            {
                servicioDialogos.MensajeError("Error", mensaje);
            }
            else
            {
                servicioDialogos.MensajeInformativo("OK", mensaje);
                WeakReferenceMessenger.Default.Send(new ActualizarGridLibrosMessage(LibroAEliminar));
            }
        }
    }
}

