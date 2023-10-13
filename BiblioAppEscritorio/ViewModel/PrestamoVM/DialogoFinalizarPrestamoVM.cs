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
    class DialogoFinalizarPrestamoVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;
        private GridPrestamos prestamoAFinalizar;

        public GridPrestamos PrestamoAFinalizar
        {
            get { return prestamoAFinalizar; }
            set { SetProperty(ref prestamoAFinalizar, value); }
        }
        private ElementoSeguridad elementoSeguridadApi;

        public ElementoSeguridad ElementoSeguridadApi
        {
            get { return elementoSeguridadApi; }
            set { SetProperty(ref elementoSeguridadApi, value); }
        }
        public DialogoFinalizarPrestamoVM()
        {
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();
            ElementoSeguridadApi = api.GetElementoSeguridad();
            PrestamoAFinalizar = WeakReferenceMessenger.Default.Send<PrestamoGridPrestamosSeleccionadoMessage>();
        }

        public void FinalizaPrestamo()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            String mensaje = api.FinalizaPrestamo(ElementoSeguridadApi, PrestamoAFinalizar.idPrestamo);
            if (mensaje != "Préstamo finalizado")
            {
                servicioDialogos.MensajeError("Error", "Error al finalizar préstamo");
            }
            else
            {
                api.MarcaUnidadComoDisponible(api.CreateSesion(), PrestamoAFinalizar.idLibro);
                servicioDialogos.MensajeInformativo("OK", mensaje);
            }
        }

    }
}
