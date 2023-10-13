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
    class DialogoFinalizarReservaVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;
        private GridReservas reservaAFinalizar;

        public GridReservas ReservaAFinalizar
        {
            get { return reservaAFinalizar; }
            set { SetProperty(ref reservaAFinalizar, value); }
        }
        private ElementoSeguridad elementoSeguridadApi;

        public ElementoSeguridad ElementoSeguridadApi
        {
            get { return elementoSeguridadApi; }
            set { SetProperty(ref elementoSeguridadApi, value); }
        }
        public DialogoFinalizarReservaVM()
        {
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();
            ElementoSeguridadApi = api.GetElementoSeguridad();
            ReservaAFinalizar = WeakReferenceMessenger.Default.Send<GridReservasSeleccionadaMessage>();
        }

        public void FinalizaReserva()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            String mensaje = api.FinalizaReserva(api.CreateSesion(), ReservaAFinalizar.idReserva, ReservaAFinalizar.idLibro);
            if (mensaje != "Reserva finalizada")
            {
                servicioDialogos.MensajeError("Error", "Error al finalizar la reserva");
            }
            else 
            {
                api.MarcaUnidadComoDisponibleReserva(api.CreateSesion(), ReservaAFinalizar.idLibro);
                servicioDialogos.MensajeInformativo("OK", mensaje);
            }
        }

    }
}
