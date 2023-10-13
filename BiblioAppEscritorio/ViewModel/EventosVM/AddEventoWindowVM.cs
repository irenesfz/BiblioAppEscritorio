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

namespace BiblioAppEscritorio.ViewModel
{
    class AddEventoWindowVM : ObservableObject
    {
        private readonly ServicioAzure servicioAzure;
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;
        private static readonly string filtrosRuta = "JPG (*.jpg)|*.jpg|PNG (*.png*)|*.png*";
        //VARIABLES
        private Evento eventoNuevo;

        public Evento EventoNuevo
        {
            get { return eventoNuevo; }
            set { SetProperty(ref eventoNuevo, value); }
        }
        private DateTime fecha;

        public DateTime Fecha
        {
            get { return fecha; }
            set { SetProperty(ref fecha, value); }
        }
        //COMMANDS
        public RelayCommand AddEventoCommand { get; }
        public RelayCommand ExaminarImagenCommand { get; }
        public AddEventoWindowVM()
        {
            //INICIALIZO
            servicioDialogos = new ServicioDialogos();
            servicioAzure = new ServicioAzure();
            api = new ApiService();
            fecha = DateTime.Today;

            //COMMANDS
            AddEventoCommand = new RelayCommand(AddEvento);
            ExaminarImagenCommand = new RelayCommand(ExaminarImagen);

            EventoNuevo = new Evento();
        }


        //FUNCIONES DEL COMMAND
        public void AddEvento()
        {
            if (CamposCorrectos())
            {
                //2022-03-10
                EventoNuevo.idEvento = 0;
                EventoNuevo.fechaEvento = fecha.ToString("yyyy-MM-dd") + "T00:00:00Z[UTC]";

                EventoNuevo.fechaPublicacion = EventoNuevo.fechaEvento;
                if (EventoNuevo.imagen == string.Empty || EventoNuevo.imagen == null)
                    EventoNuevo.imagen = "";

                string resultado = api.CreaEventos(api.CreateSesion(), EventoNuevo);
                if (resultado == "Evento insertado")
                {
                    servicioDialogos.MensajeInformativo("OK", resultado);
                    WeakReferenceMessenger.Default.Send(new ActualizarGridEventoMessage(EventoNuevo));
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
            return EventoNuevo.titulo != string.Empty &&
                EventoNuevo.descripcion != string.Empty;
        }

        public void ExaminarImagen()
        {
            try
            {
                string rutaImagen = servicioDialogos.AbrirArchivoDialogo(filtrosRuta);
                string rutaAzure = servicioAzure.GuardarImagen(rutaImagen);
                EventoNuevo.imagen = rutaAzure;
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
