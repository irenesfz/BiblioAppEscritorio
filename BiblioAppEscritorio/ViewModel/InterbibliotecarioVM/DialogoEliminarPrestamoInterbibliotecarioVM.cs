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

namespace BiblioAppEscritorio.ViewModel.InterbibliotecarioVM
{
    class DialogoEliminarPrestamoInterbibliotecarioVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;
        private PrestamoInterbibliotecario prestamoAEliminar;

        public PrestamoInterbibliotecario PrestamoAEliminar
        {
            get { return prestamoAEliminar; }
            set { SetProperty(ref prestamoAEliminar, value); }
        }

        public DialogoEliminarPrestamoInterbibliotecarioVM()
        {
            servicioDialogos = new ServicioDialogos();
            PrestamoAEliminar = WeakReferenceMessenger.Default.Send<PrestamoInterbibliotecarioRegistroMessage>();
            api = new ApiService();
        }

        public void EliminaPrestamo()
        {
            String mensaje = api.EliminaRegistroInterbiblitecario(api.CreateSesion(), PrestamoAEliminar.idInterbibliotecario);
            if (mensaje != "Prétamo interbibliotecario eliminado")
            {
                servicioDialogos.MensajeError("Error", "Error al eliminar préstamo interbibliotecario");
            }
            else
            {
                servicioDialogos.MensajeInformativo("OK", mensaje);
                WeakReferenceMessenger.Default.Send(new ActualizarPrestamoInterbibliotecarioMessage(PrestamoAEliminar));
            }
        }
    }
}