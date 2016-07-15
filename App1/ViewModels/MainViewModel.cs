using App1.Model;
using App1.Services;
using AppStudio.Uwp.Controls;
using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace App1.ViewModels
{
    public class MainViewModel : ViewModelBase, ISomeViewModel
    {
        private INavigationService _pageNavigationService;
        private IRestClient _restClient;

        public ObservableCollection<NavigationItem> MenuItems { get; }
        public Type InitialPage { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(
              this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel(INavigationService pageNavigationService, IRestClient restClient) : base(pageNavigationService)
        {
            Caption = "Cotización en Argentina";

            _pageNavigationService = pageNavigationService;
            _restClient = restClient;

            GetExchangeRates();
        }

        private string _myMessage;
        public string MyMessage
        {
            get { return _myMessage; }
            set
            {
                _myMessage = value;
                NotifyOfPropertyChange(() => MyMessage);
            }
        }

        private string _caption;
        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                NotifyOfPropertyChange(() => Caption);
            }
        }

        private bool _IsPaneOpen;

        public bool IsPaneOpen
        {
            get { return _IsPaneOpen; }
            set
            {
                _IsPaneOpen = value;
                NotifyOfPropertyChange(() => IsPaneOpen);
            }
        }

        public void SayHello()
        {
            MyMessage = "Hello World!";
        }

        public void GoNext()
        {
            try
            {
                _pageNavigationService.For<InformationViewModel>().Navigate();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public void HandlePane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        private double compra;
        public double Compra
        {
            get { return compra; }
            set
            {
                compra = value;
                NotifyOfPropertyChange(() => Compra);
            }
        }

        private double venta;
        public double Venta
        {
            get { return venta; }
            set
            {
                venta = value;
                NotifyOfPropertyChange(() => Venta);
            }
        }


        private double euroVenta;
        public double EuroVenta
        {
            get { return euroVenta; }
            set
            {
                euroVenta = value;
                NotifyOfPropertyChange(() => EuroVenta);
            }
        }

        private double euroCompra;
        public double EuroCompra
        {
            get { return euroCompra; }
            set
            {
                euroCompra = value;
                NotifyOfPropertyChange(() => EuroCompra);
            }
        }


        private DateTime lastUpdated;

        public DateTime LastUpdated
        {
            get { return lastUpdated; }
            set { lastUpdated = value; }
        }

        public void ShowAbout()
        {
            _pageNavigationService.For<InformationViewModel>().Navigate();
        }

        public async void GetExchangeRates()
        {
            var response = await _restClient.Get<ArsRate>();

            PopulateRates(response);

        }

        private void PopulateRates(ArsRate arsRate)
        {
            Compra = arsRate.oficial.value_buy;
            Venta = arsRate.oficial.value_sell;

            EuroCompra = arsRate.oficial_euro.value_buy;
            EuroVenta = arsRate.oficial_euro.value_sell;
            //TODO: put format on it
            LastUpdated = DateTime.Parse(arsRate.last_update);
        }

    }
}
