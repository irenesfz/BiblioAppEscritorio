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
    class DialogoFinalizarPrestamoInterbibliotecarioVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;
        private PrestamoInterbibliotecario prestamoAFinalizar;

        public PrestamoInterbibliotecario PrestamoAFinalizar
        {
            get { return prestamoAFinalizar; }
            set { SetProperty(ref prestamoAFinalizar, value); }
        }

        public DialogoFinalizarPrestamoInterbibliotecarioVM()
        {
            servicioDialogos = new ServicioDialogos();
            PrestamoAFinalizar = WeakReferenceMessenger.Default.Send<PrestamoInterbibliotecarioRegistroMessage>();
            api = new ApiService();
        }

        public void FinalizaPrestamo()
        {
            PrestamoAFinalizar.finalizada = true;
            String mensaje = api.EditaRegistroInterbiblitecario(api.CreateSesion(), PrestamoAFinalizar);
            if (mensaje != "Prétamo interbibliotecario editado")
            {
                servicioDialogos.MensajeError("Error", "Error al finalizar el préstamo interbibliotecario");
            }
            else
            {
                servicioDialogos.MensajeInformativo("OK", "Prétamo interbibliotecario finalizado");
                WeakReferenceMessenger.Default.Send(new ActualizarPrestamoInterbibliotecarioMessage(PrestamoAFinalizar));
            }
        }
    }
}
