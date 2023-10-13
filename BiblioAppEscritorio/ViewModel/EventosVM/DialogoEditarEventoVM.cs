using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.ViewModel.Eventos
{
    class DialogoEditarEventoVM : ObservableRecipient
    {
        private readonly ServicioAzure servicioAzure;
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;
        private static readonly string filtrosRuta = "JPG (*.jpg)|*.jpg|PNG (*.png*)|*.png*";
        //VARIABLES
        private Evento eventoAEditar;

        public Evento EventoAEditar
        {
            get { return eventoAEditar; }
            set { SetProperty(ref eventoAEditar, value); }
        }
        private DateTime fecha;

        public DateTime Fecha
        {
            get { return fecha; }
            set { SetProperty(ref fecha, value); }
        }
        //COMMANDS
        public DialogoEditarEventoVM()
        {
            //INICIALIZO
            servicioDialogos = new ServicioDialogos();
            servicioAzure = new ServicioAzure();
            api = new ApiService();


            EventoAEditar = WeakReferenceMessenger.Default.Send<EventoSeleccionadoMessage>();
            string s = EventoAEditar.fechaEvento.Substring(0, 10);
            Fecha = DateTime.Parse(s);
        }


        //FUNCIONES DEL COMMAND
        public void EditaEvento()
        {
            if (CamposCorrectos())
            {
                //2022-03-10
                EventoAEditar.fechaEvento = fecha.ToString("yyyy-MM-dd") + "T00:00:00Z[UTC]";

                EventoAEditar.fechaPublicacion = DateTime.Now.ToString("yyyy-MM-dd") + "T00:00:00Z[UTC]";
                if (EventoAEditar.imagen == string.Empty || EventoAEditar.imagen == null)
                    EventoAEditar.imagen = "";

                string resultado = api.EditaEventos(api.CreateSesion(), EventoAEditar);
                if (resultado == "Evento actualizado")
                {
                    servicioDialogos.MensajeInformativo("OK", resultado);
                    WeakReferenceMessenger.Default.Send(new ActualizarGridEventoMessage(EventoAEditar));
                }
                else
                {
                    servicioDialogos.MensajeError("Error", "No se ha podido crear el evento.");
                }
            }
            else
            {
                servicioDialogos.MensajeError("Error", "No has rellenado los campos requeridos.");
            }
        }

        private bool CamposCorrectos()
        {
            return EventoAEditar.titulo != string.Empty &&
                EventoAEditar.descripcion != string.Empty;
        }

        public void ExaminarImagen()
        {
            try
            {
                string rutaImagen = servicioDialogos.AbrirArchivoDialogo(filtrosRuta);
                string rutaAzure = servicioAzure.GuardarImagen(rutaImagen);
                EventoAEditar.imagen = rutaAzure;
            }//System.AggregateException System.ArgumentOutOfRangeException
            catch (AggregateException)
            {
                servicioDialogos.MensajeError("ERROR", "Error al guardar la imagen");
            }
            catch (ArgumentException) { Console.WriteLine("Ha cerrado dialogo"); }
            catch (Azure.RequestFailedException)
            {
                servicioDialogos.MensajeError("ERROR", "Error con la imagen");
            }
            catch (Exception)
            {
                servicioDialogos.MensajeError("ERROR", "Error desconocido");
            }
        }
    }
}
