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

namespace BiblioAppEscritorio.ViewModel.Interbibliotecario
{
    class RegistroPrestamoInterbiblitoecarioVM : ObservableRecipient
    {
        //VARIABLES
        readonly ServicioDialogos dialogoService;
        private readonly ApiService api;

        private PrestamoInterbibliotecario prestamoSeleccionado;

        public PrestamoInterbibliotecario PrestamoSeleccionado
        {
            get { return prestamoSeleccionado; }
            set { SetProperty(ref prestamoSeleccionado, value); }
        }

        private ElementoSeguridad elementoSeguridadApi;

        public ElementoSeguridad ElementoSeguridadApi
        {
            get { return elementoSeguridadApi; }
            set { SetProperty(ref elementoSeguridadApi, value); }
        }

        private ObservableCollection<PrestamoInterbibliotecario> listaPrestamos;

        public ObservableCollection<PrestamoInterbibliotecario> ListaPrestamos
        {
            get { return listaPrestamos; }
            set { SetProperty(ref listaPrestamos, value); }
        }



        //COMMANDS
        public RelayCommand QuitarSeleccionPrestamoCommand { get; }
        public RelayCommand RefrescaListaCommand { get; }
        public RelayCommand FinalizarPrestamoCommand { get; }
        public RelayCommand EliminarPrestamoCommand { get; }

        public RegistroPrestamoInterbiblitoecarioVM()
        {
            //COMMANDS
            QuitarSeleccionPrestamoCommand = new RelayCommand(QuitarSeleccion);
            RefrescaListaCommand = new RelayCommand(RefrescaLista);
            FinalizarPrestamoCommand = new RelayCommand(FinalizaPrestamo);
            EliminarPrestamoCommand = new RelayCommand(EliminaPrestamo);

            //INICIALIZAR
            dialogoService = new ServicioDialogos();
            api = new ApiService();

            //DATOS
            PrestamoSeleccionado = null;
            ElementoSeguridadApi = api.GetElementoSeguridad();
            ListaPrestamos = api.MuestraTodosLosRegistrosInterbiblitecarios(ElementoSeguridadApi);
            //SERVICIO DE MENSAJERÍA
            WeakReferenceMessenger.Default.Register<RegistroPrestamoInterbiblitoecarioVM, PrestamoInterbibliotecarioRegistroMessage>(this, (r, m) =>
            {
                m.Reply(r.PrestamoSeleccionado);
            });
            //refresca lista ActualizarGridLibrosMessage
            WeakReferenceMessenger.Default.Register<ActualizarPrestamoInterbibliotecarioMessage>(this, (r, m) =>
            {
                RefrescaLista();
            });
        }

        //FUNCIONES DEL COMMAND
        private void QuitarSeleccion()
        {
            PrestamoSeleccionado = null;
        }

        private void RefrescaLista()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            ListaPrestamos = api.MuestraTodosLosRegistrosInterbiblitecarios(ElementoSeguridadApi);
            PrestamoSeleccionado = null;
        }

        private void FinalizaPrestamo()
        {
            dialogoService.AbrirDialogoFinalizarPrestamoInter();
        }

        private void EliminaPrestamo()
        {
            dialogoService.AbrirDialogoEliminarPrestamoInter();
        }
    }

}