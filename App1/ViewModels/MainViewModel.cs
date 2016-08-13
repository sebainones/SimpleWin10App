using Caliburn.Micro;
using RateApp.BackgroundTasks;
using RateApp.Error;
using RateApp.LocalSettings;
using RateApp.Model;
using RateApp.Services;
using RateApp.Tiles;
using RateApp.Utils;
using System;
using Windows.ApplicationModel;

namespace RateApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //TODO: make them readonly??
        private readonly INavigationService _pageNavigationService;
        private readonly IRestClient _restClient;
        private readonly IMessageDialog _messageDialog;
        private readonly ITileManager _tileManager;
        private readonly ILocalSettings _localSettings;

        private ArsRate previousRates;

        public RateIndicator DolarCompraRateIndicator { get; set; }

        public RateIndicator DolarVentaRateIndicator { get; set; }

        public MainViewModel(INavigationService pageNavigationService, IRestClient restClient, IMessageDialog messageDialog, ITileManager tileManager, ILocalSettings localSettings) : base(pageNavigationService)
        {
            Caption = "Cotización en Argentina";

            _pageNavigationService = pageNavigationService;
            _restClient = restClient;
            _messageDialog = messageDialog;
            _tileManager = tileManager;
            _localSettings = localSettings;


            previousRates = new ArsRate();

            if (!DesignMode.DesignModeEnabled)
            {
                PopulateRatesAsync();

                RegisterBackGroundTaskAsync();

                UpdateTile();
            }
        }

        public async void RegisterBackGroundTaskAsync()
        {
            var backGroundTask = await BackgroundTaskManager.RegisterBackGroundTaskAsync("UpdateTask", "RateTileUpdater.UpdateTask");

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

        public async void PopulateRatesAsync()
        {
            var response = await _restClient.GetAsync<ArsRate>();

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

            _messageDialog.SimpleMessageDialogAsync(erroMessage, EnumHandler.GetDescriptionFromEnumValue(ErrorStatus.Warning));
        }

        private void PopulateFakeRates()
        {
            ArsRate fakeArsRate = new FakeArsRate();

            PopulateRates(fakeArsRate);
        }

        //TODO: Complete this functionality properly
        private void PopulateRates(ArsRate arsRate)
        {
            PopulateDolar(arsRate.Dolar, previousRates.Dolar);

            PopulateEuro(arsRate.Euro);

            PopulateDateTime(arsRate);
        }

        private void PopulateDateTime(ArsRate arsRate)
        {
            if (DateTime.TryParse(arsRate.LastUpdate, out LastUpdate))
            {
                LastUpdated = LastUpdate.ToString("d MMM yyyy");
            }
        }

        private void PopulateDolar(Rate currentDolarRate, Rate previousDolarRate)
        {
            if (currentDolarRate.HasValue)
            {
                DolarCompra = Math.Round(currentDolarRate.Compra, 2);
                DolarVenta = Math.Round(currentDolarRate.Venta, 2);

                previousDolarRate = TryGetPreviousRate(currentDolarRate);

                if (previousDolarRate.HasValue)
                    SetDolarComparison(previousDolarRate, currentDolarRate, DolarCompraRateIndicator, DolarVentaRateIndicator);
                else //No Previous Rate
                {
                    _localSettings.CreateSetting(currentDolarRate.Name, EnumHandler.GetDescriptionFromEnumValue(RateOptions.Buy), DolarCompra.ToString());
                    _localSettings.CreateSetting(currentDolarRate.Name, EnumHandler.GetDescriptionFromEnumValue(RateOptions.Sell), DolarVenta.ToString());
                    _localSettings.CreateSetting(currentDolarRate.Name, "date", DateTime.Now.ToString());
                }

                UpdatePreviousRate(previousDolarRate, DolarCompra, DolarVenta);
            }
        }

        private void UpdatePreviousRate(Rate previousRate, double buy, double sell)
        {
            //TODO: Update only when is not the same date and the buy and sell are newer datetime
            //DATETIMENOW but without minutes or secondssss

            var previousDate = DateTime.Parse(_localSettings.TryGetValue(previousRate.Name, "date"));
            
            var dateCompare = DateTime.Compare(previousDate, DateTime.Now);

            if (dateCompare < 0)//previousDate earlier => needs update
            {
                _localSettings.UpdateSetting(previousRate.Name, EnumHandler.GetDescriptionFromEnumValue(RateOptions.Buy), buy.ToString());
                _localSettings.UpdateSetting(previousRate.Name, EnumHandler.GetDescriptionFromEnumValue(RateOptions.Sell), sell.ToString());
                _localSettings.UpdateSetting(previousRate.Name,"date", DateTime.Now.ToString());
            }

        }

        private void PopulateEuro(Rate euroRate)
        {
            if (euroRate.HasValue)
            {
                previousRates.Euro = TryGetPreviousRate(euroRate);

                EuroCompra = Math.Round(euroRate.Compra, 2);
                EuroVenta = Math.Round(euroRate.Venta, 2);
            }
        }

        private Rate TryGetPreviousRate(Rate currentRate)
        {
            Rate previousRate = new Rate(currentRate.Name);
            try
            {
                previousRate.Compra = double.Parse(_localSettings.TryGetValue(currentRate.Name, EnumHandler.GetDescriptionFromEnumValue(RateOptions.Buy)));
                previousRate.Venta = double.Parse(_localSettings.TryGetValue(currentRate.Name, EnumHandler.GetDescriptionFromEnumValue(RateOptions.Sell)));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return previousRate;
        }

        //TODO: refactor this to not use the rateIndicator. Maybe generic or sthg else
        private void SetDolarComparison(Rate previousRate, Rate curentRate, RateIndicator dolarCompraRateIndicator, RateIndicator dolarVentaRateIndicator)
        {
            if (curentRate.Compra > previousRate.Compra)
                dolarCompraRateIndicator = RateIndicator.Increased;
            else if (curentRate.Compra < previousRate.Compra)
                dolarCompraRateIndicator = RateIndicator.Decreased;
            else
                dolarCompraRateIndicator = RateIndicator.Equal;

            if (curentRate.Venta > previousRate.Venta)
                dolarVentaRateIndicator = RateIndicator.Increased;
            else if (curentRate.Venta < previousRate.Venta)
                dolarVentaRateIndicator = RateIndicator.Decreased;
            else
                dolarVentaRateIndicator = RateIndicator.Equal;
        }

        private void UpdateTile()
        {
            var xmlText = _tileManager.CreateAdaptiveTile("Dolar", "Compra", DolarCompra.ToString(), "Venta", DolarVenta.ToString());

            _tileManager.Update(xmlText);
        }
    }
}
