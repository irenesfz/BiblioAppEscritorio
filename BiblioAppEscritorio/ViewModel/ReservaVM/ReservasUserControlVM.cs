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
using System.Windows;

namespace BiblioAppEscritorio.ViewModel
{
    class ReservasUserControlVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ServicioDialogos servicioDialogos;
        private readonly ApiService api;
        private GridReservas reservaSeleccionada;

        public GridReservas ReservaSeleccionada
        {
            get { return reservaSeleccionada; }
            set { SetProperty(ref reservaSeleccionada, value); }
        }

        private ObservableCollection<GridReservas> listaReservas;
        public ObservableCollection<GridReservas> ListaReservas
        {
            get { return listaReservas; }
            set { SetProperty(ref listaReservas, value); }
        }
        private ElementoSeguridad elementoSeguridadApi;

        public ElementoSeguridad ElementoSeguridadApi
        {
            get { return elementoSeguridadApi; }
            set { SetProperty(ref elementoSeguridadApi, value); }
        }

        //COMMANDS
        public RelayCommand QuitarSeleccionReservaCommand { get; }
        public RelayCommand RefrescaListaCommand { get; }
        public RelayCommand FinalizarReservaCommand { get; }
        public RelayCommand EliminarReservaCommand { get; }

        public ReservasUserControlVM()
        {
            //INICIALIZAR
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();

            //COMMANDS
            QuitarSeleccionReservaCommand = new RelayCommand(QuitarSeleccion);
            RefrescaListaCommand = new RelayCommand(RefrescaLista);
            FinalizarReservaCommand = new RelayCommand(FinalizaReserva);
            EliminarReservaCommand = new RelayCommand(EliminaReserva);

            //DATOS
            ElementoSeguridadApi = api.GetElementoSeguridad();
            ListaReservas = api.MuestraTodosLosReservasGrid(ElementoSeguridadApi);
            ReservaSeleccionada = null;

            //SERVICIO DE MENSAJERÍA
            WeakReferenceMessenger.Default.Register<ReservasUserControlVM, GridReservasSeleccionadaMessage>(this, (r, m) =>
            {
                m.Reply(r.ReservaSeleccionada);
            });

            //refresca lista ActualizarGridReservasMessage
            WeakReferenceMessenger.Default.Register<ActualizarGridReservasMessage>(this, (r, m) =>
            {
                if (!api.CheckApiKey(ElementoSeguridadApi).validate)
                {
                    ElementoSeguridadApi = api.GetElementoSeguridad();
                }
                ListaReservas = api.MuestraTodosLosReservasGrid(ElementoSeguridadApi);
            });
        }

        //FUNCIONES DEL COMMAND
        private void QuitarSeleccion()
        {
            ReservaSeleccionada = null;
        }

        private void RefrescaLista()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            ListaReservas = api.MuestraTodosLosReservasGrid(ElementoSeguridadApi);
            ReservaSeleccionada = null;
        }

        private void FinalizaReserva()
        {
            servicioDialogos.AbrirDialogoFinalizaReserva();
            RefrescaLista();
        }

        private void EliminaReserva()
        {
            servicioDialogos.AbrirDialogoEliminarReserva();
            RefrescaLista();
        }
    }
}
