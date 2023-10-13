using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;

namespace BiblioAppEscritorio.ViewModel
{
    class DialogoEliminarPrestamoVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;
        private GridPrestamos prestamoAEliminar;
        
        public GridPrestamos PrestamoAEliminar
        {
            get { return prestamoAEliminar; }
            set { SetProperty(ref prestamoAEliminar, value); }
        }

        public DialogoEliminarPrestamoVM()
        {
            servicioDialogos = new ServicioDialogos();
            PrestamoAEliminar = WeakReferenceMessenger.Default.Send<PrestamoGridPrestamosSeleccionadoMessage>();
            api = new ApiService();
        }

        public void EliminaPrestamo()
        {
            String mensaje = api.EliminaPrestamo(api.GetElementoSeguridad(), PrestamoAEliminar.idPrestamo);
            if (mensaje != "Préstamo eliminado")
            {
                servicioDialogos.MensajeError("Error", "Error al eliminar préstamo");
            }
            else
            {
                servicioDialogos.MensajeInformativo("OK", mensaje);
            }
        }

    }
}

