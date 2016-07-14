using App1.ViewModels;
using AppStudio.Uwp.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.Views
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
