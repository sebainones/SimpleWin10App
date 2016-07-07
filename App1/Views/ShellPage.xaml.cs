using AppStudio.Uwp;
using AppStudio.Uwp.Controls;
using AppStudio.Uwp.Navigation;
using System;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.Views
{
    //TODO: Then replace for the cuntionallity already provided by Caliburn.Micro
    //if it is better or more legible


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        private static Brush NavigationItemColor { get { return "NavigationPaneText".Resource() as Brush; } }

        public static ShellPage Current { get; private set; }

        public ShellControl ShellControl
        {
            get { return shell; }
        }

        public Frame AppFrame
        {
            get { return frame; }
        }

        public ShellPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
            ShellPage.Current = this;

            this.SizeChanged += OnSizeChanged;
            if (SystemNavigationManager.GetForCurrentView() != null)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += ((sender, e) =>
                {
                    if (SupportFullScreen && ShellControl.IsFullScreen)
                    {
                        e.Handled = true;
                        ShellControl.ExitFullScreen();
                    }
                    else if (NavigationService.CanGoBack())
                    {
                        NavigationService.GoBack();
                        e.Handled = true;
                    }
                });

                NavigationService.Navigated += ((sender, e) =>
                {
                    if (NavigationService.CanGoBack())
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    }
                    else
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    }
                });
            }
        }

        public bool SupportFullScreen { get; set; }

        #region NavigationItems
        public ObservableCollection<NavigationItem> NavigationItems
        {
            get { return (ObservableCollection<NavigationItem>)GetValue(NavigationItemsProperty); }
            set { SetValue(NavigationItemsProperty, value); }
        }

        public static readonly DependencyProperty NavigationItemsProperty = DependencyProperty.Register("NavigationItems", typeof(ObservableCollection<NavigationItem>), typeof(ShellPage), new PropertyMetadata(new ObservableCollection<NavigationItem>()));
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if DEBUG
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 320, Height = 500 });
#endif
            NavigationService.Initialize(typeof(ShellPage), AppFrame);
            NavigationService.NavigateToPage<SomeView>(e);

            InitializeNavigationItems();

            //I don't think that I need it
            //Bootstrap.Init();
        }

        #region Navigation
        private void InitializeNavigationItems()
        {
            NavigationItem navItem = NodeFromAction("Home", "Home", (ni) => NavigationService.NavigateToRoot(), IconFromSymbol(Symbol.Home));

            //NavigationItems.Add(AppNavigation.NodeFromAction(
            //    "Home",
            //    "Home",
            //    (ni) => NavigationService.NavigateToRoot(),
            //    AppNavigation.IconFromSymbol(Symbol.Home)));

        }
        #endregion

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateDisplayMode(e.NewSize.Width);
        }

        private void UpdateDisplayMode(double? width = null)
        {
            if (width == null)
            {
                width = Window.Current.Bounds.Width;
            }
            this.ShellControl.DisplayMode = width > 640 ? SplitViewDisplayMode.CompactOverlay : SplitViewDisplayMode.Overlay;
            this.ShellControl.CommandBarVerticalAlignment = width > 640 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
        }

        private async void OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.F11)
            {
                if (SupportFullScreen)
                {
                    await ShellControl.TryEnterFullScreenAsync();
                }
            }
            else if (e.Key == Windows.System.VirtualKey.Escape)
            {
                if (SupportFullScreen && ShellControl.IsFullScreen)
                {
                    ShellControl.ExitFullScreen();
                }
                else
                {
                    NavigationService.GoBack();
                }
            }
        }

        public static IconElement IconFromSymbol(Symbol symbol) => NavigationItem.CreateIcon(symbol, NavigationItemColor);

        public static NavigationItem NodeFromAction(string id, string caption, Action<NavigationItem> onClick, IconElement icon = null, Image image = null)
        {
            var node = new NavigationItem(id, caption);
            node.OnClick = onClick;
            node.Icon = icon;
            node.Image = image;
            return node;
        }
    }
}
