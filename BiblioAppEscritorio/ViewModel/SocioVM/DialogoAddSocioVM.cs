using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.ViewModel
{
    class DialogoAddSocioVM : ObservableObject
    {
        private readonly ApiService api;
        private readonly ServicioDialogos servicioDialogos;
        private readonly string filtrosRuta = "JPG (*.jpg)|*.jpg|PNG (*.png*)|*.png*";

        private DateTime fecha;

        public DateTime Fecha
        {
            get { return fecha; }
            set { SetProperty(ref fecha, value); }
        }

        private string imagen;

        public string Imagen
        {
            get { return imagen; }
            set { SetProperty(ref imagen, value); }
        }

        private Socio socioNuevo;

        public Socio SocioNuevo
        {
            get { return socioNuevo; }
            set { SetProperty(ref socioNuevo, value); }
        }
        private ElementoSeguridad elementoSeguridadApi;

        public ElementoSeguridad ElementoSeguridadApi
        {
            get { return elementoSeguridadApi; }
            set { SetProperty(ref elementoSeguridadApi, value); }
        }
        //constructor
        public DialogoAddSocioVM()
        {
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();
            SocioNuevo = new Socio();
            fecha = DateTime.Today;
            Imagen = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_960_720.png";
        }
        public void ExaminaSocio()
        {
            string rutaImagen = servicioDialogos.AbrirArchivoDialogo(filtrosRuta);
            string base64 = GetBase64StringForImage(rutaImagen);
            ElementoSeguridadApi = api.GetElementoSeguridad();
            SocioNuevo.imagen = base64;
            Imagen = base64;
        }
        public void AddSocio()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            // SocioNuevo.fechaNacimiento = fecha.ToString();
            if (CamposCorrectos())
            {
                if (SocioNuevo.categoriasInteres == null) 
                    SocioNuevo.categoriasInteres = "";
                SocioNuevo.fechaNacimiento = fecha.ToString("yyyy-MM-dd") + "T00:00:00Z[UTC]";
                if (socioNuevo.categoriasInteres == string.Empty)
                    socioNuevo.categoriasInteres = " ";
                SocioNuevo.rol = "socio";
                string resultado = api.CreaSocio(api.CreateSesion(), SocioNuevo);
                if (resultado == "Socio creado")
                {
                    servicioDialogos.MensajeInformativo("OK", resultado);
                    WeakReferenceMessenger.Default.Send(new ActualizarGridSociosMessage(SocioNuevo));
                }
                else
                {
                    servicioDialogos.MensajeError("Error", "No se ha podido crear el socio.");
                }
            }
            else
            {
                servicioDialogos.MensajeError("Error", "No has rellenado los campos requeridos.");
            }
            //carga todos los socios
            //WeakReferenceMessenger.Default.Send(new ActualizarGridSociosMessage(ApiService.MuestraTodosLosSocios(ApiService.CreateSesion())));

        }

        private string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        private bool CamposCorrectos()
        {
            return SocioNuevo.apellidos != string.Empty &&
                 SocioNuevo.nombre != string.Empty &&
                 SocioNuevo.telefono.ToString() != string.Empty &&
                 SocioNuevo.correo != string.Empty &&
                 SocioNuevo.fechaNacimiento != string.Empty;
        }
    }
}
