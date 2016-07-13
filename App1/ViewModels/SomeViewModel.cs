using AppStudio.Uwp.Controls;
using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace App1.ViewModels
{
    public class SomeViewModel : ViewModelBase, ISomeViewModel
    {
        private INavigationService _pageNavigationService;
        public ObservableCollection<NavigationItem> MenuItems { get; }
        public Type InitialPage { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(
              this, new PropertyChangedEventArgs(propertyName));
        }

        public SomeViewModel(INavigationService pageNavigationService) : base(pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            Caption = "Titulazo";
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
                _pageNavigationService.For<AnotherViewModel>().Navigate();
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

    }
}
