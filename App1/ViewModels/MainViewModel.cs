using RateApp.Error;
using RateApp.Model;
using RateApp.Services;
using RateApp.Tiles;
using AppStudio.Uwp.Controls;
using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.ApplicationModel;
using RateApp.BackgroundTasks;

namespace RateApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService _pageNavigationService;
        private IRestClient _restClient;
        private IMessageDialog _messageDialog;
        private ITileManager _tileManager;

        public ObservableCollection<NavigationItem> MenuItems { get; }
        public Type InitialPage { get; }

        //public override event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(
        //      this, new PropertyChangedEventArgs(propertyName));
        //}

        public MainViewModel(INavigationService pageNavigationService, IRestClient restClient, IMessageDialog messageDialog, ITileManager tileManager) : base(pageNavigationService)
        {
            Caption = "Cotización en Argentina";

            _pageNavigationService = pageNavigationService;
            _restClient = restClient;
            _messageDialog = messageDialog;
            _tileManager = tileManager;

            if (!DesignMode.DesignModeEnabled)
            {
                PopulateRates();

                RegisterBackGroundTaskAsync();
            }
        }

        public async void RegisterBackGroundTaskAsync()
        {
            BackgroundTaskManager backgroundTaskManager = new BackgroundTaskManager();

            var backGroundTask = await backgroundTaskManager.RegisterBackGroundTaskAsync("UpdateTask", "RateTileUpdater.UpdateTask");

            backGroundTask.Completed += BackGroundTask_Completed;
        }

        private void BackGroundTask_Completed(Windows.ApplicationModel.Background.BackgroundTaskRegistration sender, Windows.ApplicationModel.Background.BackgroundTaskCompletedEventArgs args)
        {
            // UI updates should be performed asynchronously, to avoid holding up the UI thread
            UpdateTile();
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

        public void GoNext()
        {
            try
            {
                _pageNavigationService.For<InformationViewModel>().Navigate();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void HandlePane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        private double dolarCompra;
        public double DolarCompra
        {
            get { return dolarCompra; }
            set
            {
                dolarCompra = value;
                NotifyOfPropertyChange(() => DolarCompra);
            }
        }

        private double dolarVenta;
        public double DolarVenta
        {
            get { return dolarVenta; }
            set
            {
                dolarVenta = value;
                NotifyOfPropertyChange(() => DolarVenta);
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

        private DateTime LastUpdate;

        private string lastUpdated;
        public string LastUpdated
        {
            get { return lastUpdated; }
            set { lastUpdated = value; }
        }

        public void ShowAbout()
        {
            _pageNavigationService.For<InformationViewModel>().Navigate();
        }

        public async void PopulateRates()
        {
            var response = await _restClient.Get<ArsRate>();

            if (response != null)
                PopulateRates(response);
            else
            {
                ShowError();
                PopulateFakeRates();
            }
        }

        private void ShowError()
        {
            string erroMessage = "Problema de Conectividad";

            Log.Warn(erroMessage);

            _messageDialog.SimpleMessageDialog(erroMessage, ErrorHandler.GetDescriptionFromEnumValue(ErrorStatus.Warning));
        }

        private void PopulateFakeRates()
        {
            ArsRate fakeArsRate = new ArsRate();
            fakeArsRate.Dolar = new Currency();
            fakeArsRate.Euro = new Currency();
            fakeArsRate.Dolar.value_buy = 10.99;
            fakeArsRate.Dolar.value_sell = 11.99;
            fakeArsRate.Euro.value_buy = 0.01;
            fakeArsRate.Euro.value_sell = 0.01;
            fakeArsRate.LastUpdate = DateTime.Now.ToString("d MMM yyyy");

            PopulateRates(fakeArsRate);
        }

        private void PopulateRates(ArsRate arsRate)
        {
            if (arsRate.Dolar.HasValue)
            {
                DolarCompra = Math.Round(arsRate.Dolar.value_buy, 2);
                DolarVenta = Math.Round(arsRate.Dolar.value_sell, 2); ;
            }

            if (arsRate.Euro.HasValue)
            {
                EuroCompra = Math.Round(arsRate.Euro.value_buy, 2);
                EuroVenta = Math.Round(arsRate.Euro.value_sell, 2);
            }

            if (DateTime.TryParse(arsRate.LastUpdate, out LastUpdate))
            {
                LastUpdated = LastUpdate.ToString("d MMM yyyy");
            }
        }

        private void UpdateTile()
        {
            var xmlText = _tileManager.CreateAdaptiveTile("Dolar","Compra", DolarCompra.ToString(), "Venta", DolarVenta.ToString());

            _tileManager.Update(xmlText);
        }
    }
}
