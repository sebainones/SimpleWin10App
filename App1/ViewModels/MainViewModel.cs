using App1.Model;
using App1.Services;
using AppStudio.Uwp.Controls;
using Caliburn.Micro;
using NotificationsExtensions;
using NotificationsExtensions.Tiles;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.UI.Notifications;

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
                ShowError();
        }

        private void ShowError()
        {
            //TODO: IMplement this with the proper error mechanism
            PopulateFakeRates();
        }

        private void PopulateFakeRates()
        {
            ArsRate fakeArsRate = new ArsRate();
            fakeArsRate.oficial = new Oficial();
            fakeArsRate.oficial_euro = new OficialEuro();
            fakeArsRate.oficial.value_buy = 10.99;
            fakeArsRate.oficial.value_sell = 11.99;
            fakeArsRate.last_update = DateTime.Now.ToString("d MMM yyyy");

            PopulateRates(fakeArsRate);
        }

        private void PopulateRates(ArsRate arsRate)
        {
            if (arsRate.oficial.HasValue)
            {
                Compra = arsRate.oficial.value_buy;
                Venta = arsRate.oficial.value_sell;
            }

            if (arsRate.oficial_euro.HasValue)
            {
                EuroCompra = arsRate.oficial_euro.value_buy;
                EuroVenta = arsRate.oficial_euro.value_sell;
            }

            if (DateTime.TryParse(arsRate.last_update, out LastUpdate))
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
            bodyTitle.Text = "Compra";

            var bodyTitleValue = new AdaptiveText();
            bodyTitleValue.HintAlign = AdaptiveTextAlign.Right;
            bodyTitleValue.Text = Compra.ToString();

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
