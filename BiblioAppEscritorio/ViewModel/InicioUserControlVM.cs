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
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BiblioAppEscritorio.ViewModel
{
    class InicioUserControlVM : ObservableRecipient
    {
        private readonly ApiService api;
        private string blockNotas;

        public string BlockNotas
        {
            get { return blockNotas; }
            set { SetProperty(ref blockNotas, value); }
        }

        private ObservableCollection<Evento> listaEventos;
        public ObservableCollection<Evento> ListaEventos
        {
            get { return listaEventos; }
            set { SetProperty(ref listaEventos, value); }
        }
        private string fecha;

        public string Fecha
        {
            get { return fecha; }
            set { SetProperty(ref fecha, value); }
        }

        private int numeroEventos;

        public int NumeroEventos
        {
            get { return numeroEventos; }
            set { SetProperty(ref numeroEventos, value); }
        }
        private DispatcherTimer timer;

        public DispatcherTimer Timer
        {
            get { return timer; }
            set { SetProperty(ref timer, value); }
        }

        private string time;

        public string Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }


        public RelayCommand GuardarNotas { get; }
        public InicioUserControlVM()
        {
            BlockNotas = Properties.Settings.Default.NotasBlock;
            GuardarNotas = new RelayCommand(GuardaNotas);
            Fecha = DateTime.Today.ToString("dd-MM-yyyy");
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += TimerTick;
            Timer.Start();
            api = new ApiService();
            ListaEventos = api.BuscaEventoPorFecha(DateTime.Today.ToString("yyyy-MM-dd"));
            NumeroEventos = ListaEventos.Count;
            //cuando se añada un evendo, carga los de hoy por si se ha añadido uno con la fecha de hoy
            WeakReferenceMessenger.Default.Register<ActualizarGridEventoMessage>(this, (r, m) =>
            {
                ListaEventos = api.BuscaEventoPorFecha(DateTime.Today.ToString("yyyy-MM-dd"));
                NumeroEventos = ListaEventos.Count;
            });
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Time = DateTime.Now.ToLongTimeString();
        }

        private void GuardaNotas()
        {
            Properties.Settings.Default.NotasBlock = BlockNotas;
            Properties.Settings.Default.Save();
        }

    }
}
