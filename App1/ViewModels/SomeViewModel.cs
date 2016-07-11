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

        

    }
}
