using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Globalization;
using System.Windows;

namespace BiblioAppEscritorio
{
    class EditarSocioWindowVM : ObservableRecipient
    {
        private static readonly string filtrosRuta = "JPG (*.jpg)|*.jpg|PNG (*.png*)|*.png*";
        private readonly ApiService api;
        private readonly ServicioDialogos servicioDialogos;

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

        private Socio socioAEditar;

        public Socio SocioAEditar
        {
            get { return socioAEditar; }
            set { SetProperty(ref socioAEditar, value); }
        }

        public EditarSocioWindowVM()
        {
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();

            SocioAEditar = WeakReferenceMessenger.Default.Send<SocioSeleccionadoMessage>();
            string s = SocioAEditar.fechaNacimiento.Substring(0, 10);
            Fecha = DateTime.Parse(s);
            Imagen = SocioAEditar.imagen;
        }
        public void ExaminaSocio()
        {
            string rutaImagen = servicioDialogos.AbrirArchivoDialogo(filtrosRuta);
            string base64 = GetBase64StringForImage(rutaImagen);
            SocioAEditar.imagen = base64;
            Imagen = base64;
        }

        public void EditaSocio()
        {
            if (CamposCorrectos())
            {
                SocioAEditar.fechaNacimiento = Fecha.ToString("yyyy-MM-dd") + "T00:00:00Z[UTC]";
                string resultado = api.EditaSocio(api.CreateSesion(), SocioAEditar);
                if (resultado == "Socio actualizado")
                {
                    servicioDialogos.MensajeInformativo("OK", resultado);
                    WeakReferenceMessenger.Default.Send(new ActualizarGridSociosMessage(SocioAEditar));
                }
                else
                {
                    servicioDialogos.MensajeError("Error", "No se ha podido editar el socio");
                }
            }
        }
        public void RestablecePassword()
        {
            servicioDialogos.AbrirDialogoRestablecePass();
        }
        private string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        private bool CamposCorrectos()
        {
            return SocioAEditar.apellidos != string.Empty &&
                 SocioAEditar.nombre != string.Empty &&
                 SocioAEditar.telefono.ToString() != string.Empty &&
                 SocioAEditar.imagen != string.Empty &&
                 SocioAEditar.correo != string.Empty &&
                 SocioAEditar.fechaNacimiento != string.Empty;
        }
    }
}