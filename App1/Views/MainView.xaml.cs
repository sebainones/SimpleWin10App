using Microsoft.Advertising.WinRT.UI;
using RateApp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RateApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();
            this.Loaded += SomeView_Loaded;
        }



        private void SomeView_Loaded(object sender, RoutedEventArgs e)
        {
            // Get you the object of ViewModel.
            MainViewModel viewModelInstance = DataContext as MainViewModel;           
        }
      
        private void AdControl_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("AdControl error (" + ((AdControl)sender).Name + "): " + e.ErrorMessage + " ErrorCode: " + e.ErrorCode.ToString());
        }
    }
}
