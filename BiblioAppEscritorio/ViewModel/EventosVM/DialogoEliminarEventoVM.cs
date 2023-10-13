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

namespace BiblioAppEscritorio.ViewModel
{
    class DialogoEliminarEventoVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ApiService api;
        private readonly ServicioDialogos servicioDialogos;
        private Evento eventoAEliminar;

        public Evento EventoAEliminar
        {
            get { return eventoAEliminar; }
            set { SetProperty(ref eventoAEliminar, value); }
        }

        public DialogoEliminarEventoVM()
        {
            api = new ApiService();
            EventoAEliminar = WeakReferenceMessenger.Default.Send<EventoSeleccionadoMessage>();
            servicioDialogos = new ServicioDialogos();
        }

        public void EliminaEvento()
        {
            String mensaje = api.EliminaEvento(api.CreateSesion(), EventoAEliminar.idEvento);
            if (mensaje != "Evento eliminado")
            {
                servicioDialogos.MensajeError("Error", "Error al eliminar evento");
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new ActualizarGridEventoMessage(EventoAEliminar));
                servicioDialogos.MensajeInformativo("OK", mensaje);
            }
        }

    }
}

