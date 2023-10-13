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
    class DialogoEliminarReservaVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;

        private GridReservas reservaAEliminar;
       
        public GridReservas ReservaAEliminar
        {
            get { return reservaAEliminar; }
            set { SetProperty(ref reservaAEliminar, value); }
        }
        private ElementoSeguridad elementoSeguridadApi;

        public ElementoSeguridad ElementoSeguridadApi
        {
            get { return elementoSeguridadApi; }
            set { SetProperty(ref elementoSeguridadApi, value); }
        }

        public DialogoEliminarReservaVM()
        {
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();
            ElementoSeguridadApi = api.GetElementoSeguridad();
            ReservaAEliminar = WeakReferenceMessenger.Default.Send<GridReservasSeleccionadaMessage>();
        }

        public void EliminaReserva()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            String mensaje = api.EliminaReservas(api.CreateSesion(), ReservaAEliminar.idReserva);
            if (mensaje != "Reserva eliminada")
            {
                servicioDialogos.MensajeError("Error", "Error al eliminar reserva");
            }
            else
            {
                servicioDialogos.MensajeInformativo("OK", mensaje);
            }
        }

    }
}