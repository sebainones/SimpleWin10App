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
        private readonly INavigationService _pageNavigationService;
        private readonly IRestClient _restClient;
        private readonly IMessageDialog _messageDialog;
        private readonly ITileManager _tileManager;
        private readonly ILocalSettings _localSettings;

        public MainViewModel(INavigationService pageNavigationService, IRestClient restClient, IMessageDialog messageDialog, ITileManager tileManager, ILocalSettings localSettings) : base(pageNavigationService)
        {
            Caption = "Cotización en Argentina";

            _pageNavigationService = pageNavigationService;
            _restClient = restClient;
            _messageDialog = messageDialog;
            _tileManager = tileManager;
            _localSettings = localSettings;

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


        public RateIndicator euroCompraRateIndicator;
        public RateIndicator EuroCompraRateIndicator
        {
            get
            {
                return euroCompraRateIndicator;
            }
            set
            {
                if (euroCompraRateIndicator != value)
                {
                    euroCompraRateIndicator = value;
                    NotifyOfPropertyChange(() => EuroCompraRateIndicator);
                }
            }
        }

        private RateIndicator euroVentaRateindicator;
        public RateIndicator EuroVentaRateIndicator
        {
            get
            {
                return euroVentaRateindicator;
            }
            set
            {
                if (euroVentaRateindicator != value)
                {
                    euroVentaRateindicator = value;
                    NotifyOfPropertyChange(() => EuroVentaRateIndicator);
                }
            }
        }

        public RateIndicator dolarCompraRateIndicator;
        public RateIndicator DolarCompraRateIndicator
        {
            get
            {
                return dolarCompraRateIndicator;
            }
            set
            {
                if (dolarCompraRateIndicator != value)
                {
                    dolarCompraRateIndicator = value;
                    NotifyOfPropertyChange(() => DolarCompraRateIndicator);
                }
            }
        }

        private RateIndicator dolarVentaRateindicator;
        public RateIndicator DolarVentaRateIndicator
        {
            get
            {
                return dolarVentaRateindicator;
            }
            set
            {
                if (dolarVentaRateindicator != value)
                {
                    dolarVentaRateindicator = value;
                    NotifyOfPropertyChange(() => DolarVentaRateIndicator);
                }
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
                
        private void PopulateRates(ArsRate arsRate)
        {
            PopulateDolar(arsRate.Dolar);

            PopulateEuro(arsRate.Euro);

            PopulateDateTime(arsRate);
        }

        private void DeletePrevious(ArsRate arsRate)
        {
            _localSettings.Delete(arsRate.Dolar.Name);
            _localSettings.Delete(arsRate.Euro.Name);

        }

        private void PopulateDateTime(ArsRate arsRate)
        {
            if (DateTime.TryParse(arsRate.LastUpdate, out LastUpdate))
            {
                LastUpdated = LastUpdate.ToString("dd MM yyyy");
            }
        }

        private void PopulateDolar(Rate currentDolarRate)
        {
            if (currentDolarRate.HasValue)
            {
                DolarCompra = Math.Round(currentDolarRate.Compra, 2);
                DolarVenta = Math.Round(currentDolarRate.Venta, 2);

                Rate previousDolarRate = TryGetPreviousRate(currentDolarRate);

                if (previousDolarRate.HasValue && IsPreviousDateErlierThanNow(previousDolarRate))
                    SetDolarRateComparison(previousDolarRate, currentDolarRate);

                else //No Previous Rate
                    CreatePreviosRate(currentDolarRate);

                UpdatePreviousRate(previousDolarRate, DolarCompra, DolarVenta);
            }
        }

        //TODO: Refactor this to make just one method!!!
        private void PopulateEuro(Rate currentEuroRate)
        {
            if (currentEuroRate.HasValue)
            {
                EuroCompra = Math.Round(currentEuroRate.Compra, 2);
                EuroVenta = Math.Round(currentEuroRate.Venta, 2);

                Rate previousEuroRate = TryGetPreviousRate(currentEuroRate);

                if (previousEuroRate.HasValue && IsPreviousDateErlierThanNow(previousEuroRate))
                    SetEuroRateComparison(previousEuroRate, currentEuroRate);

                else //No Previous Rate
                    CreatePreviosRate(currentEuroRate);

                UpdatePreviousRate(previousEuroRate, EuroCompra, EuroVenta);
            }
        }

        private bool IsPreviousDateErlierThanNow(Rate previousRate)
        {
            var date = _localSettings.TryGetValue(previousRate.Name, "date");
            var previousDate = DateTime.Parse(date);

            return previousDate.DayOfYear < DateTime.Now.DayOfYear;
        }

        private void CreatePreviosRate(Rate currentDolarRate)
        {
            _localSettings.CreateSetting(currentDolarRate.Name, EnumHandler.GetDescriptionFromEnumValue(RateOptions.Buy), DolarCompra.ToString());
            _localSettings.CreateSetting(currentDolarRate.Name, EnumHandler.GetDescriptionFromEnumValue(RateOptions.Sell), DolarVenta.ToString());
            _localSettings.CreateSetting(currentDolarRate.Name, "date", DateTime.Now.ToString());//.ToString("dd MM yyyy")
        }

        private void UpdatePreviousRate(Rate previousRate, double buy, double sell)
        {
            if (IsPreviousDateErlierThanNow(previousRate))
            {
                _localSettings.UpdateSetting(previousRate.Name, EnumHandler.GetDescriptionFromEnumValue(RateOptions.Buy), buy.ToString());
                _localSettings.UpdateSetting(previousRate.Name, EnumHandler.GetDescriptionFromEnumValue(RateOptions.Sell), sell.ToString());
                _localSettings.UpdateSetting(previousRate.Name, "date", DateTime.Now.ToString());
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
        private void SetDolarRateComparison(Rate previousRate, Rate curentRate)
        {
            if (curentRate.Compra > previousRate.Compra)
                DolarCompraRateIndicator = RateIndicator.Increased;
            else if (curentRate.Compra < previousRate.Compra)
                DolarCompraRateIndicator = RateIndicator.Decreased;
            else
                DolarCompraRateIndicator = RateIndicator.Equal;


            if (curentRate.Venta > previousRate.Venta)
                DolarVentaRateIndicator = RateIndicator.Increased;
            else if (curentRate.Venta < previousRate.Venta)
                DolarVentaRateIndicator = RateIndicator.Decreased;
            else
                DolarVentaRateIndicator = RateIndicator.Equal;
        }

        private void SetEuroRateComparison(Rate previousRate, Rate curentRate)
        {
            if (curentRate.Compra > previousRate.Compra)
                EuroCompraRateIndicator = RateIndicator.Increased;
            else if (curentRate.Compra < previousRate.Compra)
                EuroCompraRateIndicator = RateIndicator.Decreased;
            else
                EuroCompraRateIndicator = RateIndicator.Equal;


            if (curentRate.Venta > previousRate.Venta)
                EuroVentaRateIndicator = RateIndicator.Increased;
            else if (curentRate.Venta < previousRate.Venta)
                EuroVentaRateIndicator = RateIndicator.Decreased;
            else
                EuroVentaRateIndicator = RateIndicator.Equal;
        }

        private void UpdateTile()
        {
            var xmlText = _tileManager.CreateAdaptiveTile("Dolar", "Compra", DolarCompra.ToString(), "Venta", DolarVenta.ToString());

            _tileManager.Update(xmlText);
        }
    }
}
