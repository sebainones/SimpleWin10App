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

            // Or typecast to exact instance what you intend to use.
            //MyViewModel vm = DataContext as MyViewModel;

            //var mainViewModel = new Scenario1ViewModel();

        }



        private void SomeView_Loaded(object sender, RoutedEventArgs e)
        {
            // Get you the object of ViewModel.
            MainViewModel viewModelInstance = DataContext as MainViewModel;

            //viewModelInstance.MenuItems.Add(new NavigationItem()
            //{
            //    Label = "Page 1",
            //    DestinationPage = typeof(AnotherView),
            //    Symbol = Symbol.Bookmarks
            //});
        }

    }
}
