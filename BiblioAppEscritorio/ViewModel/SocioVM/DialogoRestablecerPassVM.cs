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
    class DialogoRestablecerPassVM : ObservableRecipient
    {
        private readonly ApiService api;
        private readonly ServicioDialogos servicioDialogos;


        private Socio socioAEditar;

        public Socio SocioAEditar
        {
            get { return socioAEditar; }
            set { SetProperty(ref socioAEditar, value); }
        }

        public DialogoRestablecerPassVM()
        {
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();

            SocioAEditar = WeakReferenceMessenger.Default.Send<SocioSeleccionadoMessage>();
        }


        public void RestablecePassword()
        {

            string resultado = api.RestablecePass(api.CreateSesion(), SocioAEditar.idSocio);
            if (resultado == "Contraseña actualizada")
            {
                WeakReferenceMessenger.Default.Send(new ActualizarGridSociosMessage(SocioAEditar));
            }
        }
    }
}
