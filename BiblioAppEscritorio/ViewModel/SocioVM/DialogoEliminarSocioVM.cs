using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BiblioAppEscritorio.ViewModel
{
    class DialogoEliminarSocioVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ApiService api;
        private readonly ServicioDialogos servicioDialogos;
        private Socio socioAEliminar;

        public Socio SocioAEliminar
        {
            get { return socioAEliminar; }
            set { SetProperty(ref socioAEliminar, value); }
        }

        public DialogoEliminarSocioVM()
        {
            api = new ApiService();
            servicioDialogos = new ServicioDialogos();
            SocioAEliminar = WeakReferenceMessenger.Default.Send<SocioSeleccionadoMessage>();
        }
        public void EliminarSocio()
        {
            String mensaje = api.EliminaSocios(api.CreateSesion(), SocioAEliminar.idSocio);
            if (mensaje != "Socio eliminado")
            {
                servicioDialogos.MensajeError("Error", "Error al eliminar socio");
            }
            else
            {
                servicioDialogos.MensajeInformativo("OK", mensaje);
                WeakReferenceMessenger.Default.Send(new ActualizarGridSociosMessage(SocioAEliminar));
            }

        }
    }
}
