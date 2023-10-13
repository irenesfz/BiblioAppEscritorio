using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.ViewModel
{
    class EventosUserControlVM : ObservableRecipient
    {
        //variables
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;
        private Evento eventoSeleccionado;

        public Evento EventoSeleccionado
        {
            get { return eventoSeleccionado; }
            set { SetProperty(ref eventoSeleccionado, value); }
        }

        private ObservableCollection<Evento> listaEventos;
        public ObservableCollection<Evento> ListaEventos
        {
            get { return listaEventos; }
            set { SetProperty(ref listaEventos, value); }
        }

        //COMMANDS
        public RelayCommand RefrescaListaCommand { get; }
        public RelayCommand AddEventoCommand { get; }
        public RelayCommand EditarEventoCommand { get; }
        public RelayCommand EliminarEventoCommand { get; }


        public EventosUserControlVM()
        {
            //INICIALIZAR
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();

            //COMMANDS
            RefrescaListaCommand = new RelayCommand(RefrescaLista);
            AddEventoCommand = new RelayCommand(AddEvento);
            EditarEventoCommand = new RelayCommand(EditaEvento);
            EliminarEventoCommand = new RelayCommand(EliminaEvento);

            //VARIABLES
            ListaEventos = api.MuestraTodosLosEventos();
            EventoSeleccionado = null;

            //SERVICIO DE MENSAJERÍA
            WeakReferenceMessenger.Default.Register<EventosUserControlVM, EventoSeleccionadoMessage>(this, (r, m) =>
            {
                m.Reply(r.EventoSeleccionado);
            });
            //actualiza grid
            WeakReferenceMessenger.Default.Register<ActualizarGridEventoMessage>(this, (r, m) =>
            {
                ListaEventos = api.MuestraTodosLosEventos();
            });
        }

        //FUNCIONES DEL COMMAND
        private void RefrescaLista()
        {
            ListaEventos = api.MuestraTodosLosEventos();
            EventoSeleccionado = null;
        }

        private void AddEvento()
        {
            servicioDialogos.CargarVentanaAddEvento();
        }

        private void EditaEvento()
        {
            servicioDialogos.AbrirDialogoEditarEvento();
        }

        private void EliminaEvento()
        {
            servicioDialogos.AbrirDialogoEliminarEvento();
        }
    }
}
