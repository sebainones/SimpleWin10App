using App1.Error;
using App1.Model;
using App1.Services;
using AppStudio.Uwp.Controls;
using Caliburn.Micro;
using NotificationsExtensions;
using NotificationsExtensions.Tiles;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.ApplicationModel;
using Windows.UI.Notifications;

namespace App1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService _pageNavigationService;
        private IRestClient _restClient;
        private IMessageDialog _messageDialog;

        public ObservableCollection<NavigationItem> MenuItems { get; }
        public Type InitialPage { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(
              this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel(INavigationService pageNavigationService, IRestClient restClient, IMessageDialog messageDialog) : base(pageNavigationService)
        {
            Caption = "Cotización en Argentina";

            _pageNavigationService = pageNavigationService;
            _restClient = restClient;
            _messageDialog = messageDialog;

            if (!DesignMode.DesignModeEnabled)
            {
                GetExchangeRates();

                CreateAdaptiveLayout();
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

        public async void GetExchangeRates()
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
            string erroMessage = "Connection problem";
            Log.Warn(erroMessage);
            _messageDialog.SimpleMessageDialog(erroMessage);
        }

        private void PopulateFakeRates()
        {
            ArsRate fakeArsRate = new ArsRate();
            fakeArsRate.Dolar = new Currency();
            fakeArsRate.Euro = new Currency();
            fakeArsRate.Dolar.value_buy = 10.99;
            fakeArsRate.Dolar.value_sell = 11.99;
            fakeArsRate.LastUpdate = DateTime.Now.ToString("d MMM yyyy");

            PopulateRates(fakeArsRate);
        }

        private void PopulateRates(ArsRate arsRate)
        {
            if (arsRate.Dolar.HasValue)
            {
                DolarCompra = arsRate.Dolar.value_buy;
                DolarVenta = arsRate.Dolar.value_sell;
            }

            if (arsRate.Euro.HasValue)
            {
                EuroCompra = arsRate.Euro.value_buy;
                EuroVenta = arsRate.Euro.value_sell;
            }

            if (DateTime.TryParse(arsRate.LastUpdate, out LastUpdate))
            {
                LastUpdated = LastUpdate.ToString("d MMM yyyy");
            }
        }

        private void CreateAdaptiveLayout()
        {
            //Create a TileBinding
            var tilebinding = new TileBinding();
            //Create a TileContent
            var tileContent = new TileBindingContentAdaptive();
            tileContent.TextStacking = TileTextStacking.Center;
            //Create a groups and subgroups
            var tileGroup = new AdaptiveGroup();

            var subgroup1 = new AdaptiveSubgroup();

            //Create a subtitle
            var subTitle = new AdaptiveText();
            subTitle.HintStyle = AdaptiveTextStyle.Body;
            subTitle.Text = "Dolar Argentina";
            subgroup1.Children.Add(subTitle);




            var subgroup2 = new AdaptiveSubgroup();
            //Create a bodysubtile
            var bodyTitle = new AdaptiveText();
            bodyTitle.HintAlign = AdaptiveTextAlign.Left;
            bodyTitle.Text = "DolarCompra";

            var bodyTitleValue = new AdaptiveText();
            bodyTitleValue.HintAlign = AdaptiveTextAlign.Right;
            bodyTitleValue.Text = DolarCompra.ToString();

            subgroup2.Children.Add(bodyTitle);
            subgroup2.Children.Add(bodyTitleValue);


            //tileGroup.Children.Add(subgroup1);
            tileGroup.Children.Add(subgroup2);

            //tileContent.Children.Add(subTitle);
            //tileContent.Children.Add(bodyTitle);
            //tileContent.Children.Add(bodyTitleValue);

            tileContent.Children.Add(tileGroup);
            tilebinding.Content = tileContent;

            //Create visual object
            var tileVisual = new TileVisual();
            tileVisual.TileWide = tilebinding;

            //Create tile object
            var tileObject = new TileContent();
            tileObject.Visual = tileVisual;

            var xmltext = tileObject.GetContent();
            Update(xmltext);

        }

        private void Update(string xmldocumentinfo)
        {
            var xmldocument = new Windows.Data.Xml.Dom.XmlDocument();
            xmldocument.LoadXml(xmldocumentinfo);
            TileNotification tileNotification = new TileNotification(xmldocument);
            TileUpdater updateMgr = TileUpdateManager.CreateTileUpdaterForApplication();
            updateMgr.Update(tileNotification);
        }
    }
}
