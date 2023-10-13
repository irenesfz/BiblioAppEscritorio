using BiblioAppEscritorio.ExternalApi;
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
    class PrestamoInterbibliotecarioVM : ObservableRecipient
    {
        //variables
        private readonly ServicioDialogos servicioDialogos;
        private readonly ExternalApiService api;
        private readonly NavigationService servicionNavegacion;
        private Item resultadoSeleccionado;

        public Item ResultadoSeleccionado
        {
            get { return resultadoSeleccionado; }
            set { SetProperty(ref resultadoSeleccionado, value); }
        }

        private List<Item> listaResultados;
        public List<Item> ListaResultados
        {
            get { return listaResultados; }
            set { SetProperty(ref listaResultados, value); }
        }

        private string palabraABuscar;

        public string PalabraABuscar
        {
            get { return palabraABuscar; }
            set { SetProperty(ref palabraABuscar, value); }
        }

        //QuitaSeleccionCommand
        //SolicitaLibroCommand
        public RelayCommand QuitaSeleccionCommand { get; }
        public RelayCommand SolicitaLibroCommand { get; }
        public RelayCommand SearchCommand { get; }


        public PrestamoInterbibliotecarioVM()
        {
            //INICIALIZAR
            servicioDialogos = new ServicioDialogos();
            servicionNavegacion = new NavigationService();
            api = new ExternalApiService();

            //COMMANDS
            QuitaSeleccionCommand = new RelayCommand(QuitaSeleccion);
            SolicitaLibroCommand = new RelayCommand(SolicitaLibro);
            SearchCommand = new RelayCommand(BuscaLibros);

            //VARIABLES
            ListaResultados = new List<Item>();
            ResultadoSeleccionado = null;
            PalabraABuscar = String.Empty;

            //SERVICIO DE MENSAJERÍA
            WeakReferenceMessenger.Default.Register<PrestamoInterbibliotecarioVM, PrestamoInterbibliotecarioMessage>(this, (r, m) =>
            {
                m.Reply(r.ResultadoSeleccionado);
            });

        }

        //MÉTODOS COMMAND
        private void SolicitaLibro()
        {
            servicioDialogos.AbrirDialogoDialogoSolicitarLibro();
        }

        private void QuitaSeleccion()
        {
            ResultadoSeleccionado = null;
        }

        public void BuscaLibros()
        {
            if (PalabraABuscar != null && PalabraABuscar != String.Empty)
            {
                Root r = api.MuestraTodosLosLibros(PalabraABuscar);
                ListaResultados = r.items;

            }
            else
            {
                ListaResultados = new List<Item>();
                servicioDialogos.MensajeError("ERROR", "Error");
            }

        }
    }
}
