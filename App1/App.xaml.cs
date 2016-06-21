using App1.Logging;
using App1.ViewModels;
using App1.Views;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace App1
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        private WinRTContainer _container;
        private readonly ILog _log;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
            Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
            Microsoft.ApplicationInsights.WindowsCollectors.Session);
            InitializeComponent();
            Suspending += OnSuspending;

#if DEBUG
            //Logger for Caliburn messages.
            LogManager.GetLog = type => new CaliburnLogger(type);
#endif

            //logger for your own code.
            _log = LogManager.GetLog(typeof(App));

        }

        //SelectedAssembly is used in the case of separate views/viewmodels in different DLLs
        //So, here is not necessary at the moment.
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            try
            {
                var assenmblyName = typeof(SomeViewModel).GetTypeInfo().Assembly.GetAssemblyName();
                AssemblyName assemblyName = new AssemblyName(assenmblyName);
                return new[] { Assembly.Load(assemblyName) };
            }
            catch (Exception exception)
            {
                _log.Error(exception);
                throw;
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            try
            {
                // I am launching my main view here
                DisplayRootView<SomeView>();
            }
            catch (Exception exception)
            {
                _log.Error(exception);
                throw;
            }
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            try
            {
                //Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (rootFrame == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    rootFrame = new Frame();

                    rootFrame.NavigationFailed += OnNavigationFailed;

                    // Place the frame in the current Window
                    Window.Current.Content = rootFrame;
                }

                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    //rootFrame.Navigate(typeof(HelloView));
                }

                _container.RegisterNavigationService(rootFrame);
            }
            catch (Exception exception)
            {
                _log.Error(exception);
                throw;
            }
        }

        //        protected override void OnLaunched(LaunchActivatedEventArgs e)
        //        {

        //#if DEBUG
        //            if (System.Diagnostics.Debugger.IsAttached)
        //            {
        //                this.DebugSettings.EnableFrameRateCounter = true;
        //            }
        //#endif

        //            Frame rootFrame = Window.Current.Content as Frame;

        //            // Do not repeat app initialization when the Window already has content,
        //            // just ensure that the window is active
        //            if (rootFrame == null)
        //            {
        //                // Create a Frame to act as the navigation context and navigate to the first page
        //                rootFrame = new Frame();

        //                rootFrame.NavigationFailed += OnNavigationFailed;

        //                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
        //                {
        //                    //TODO: Load state from previously suspended application
        //                }

        //                // Place the frame in the current Window
        //                Window.Current.Content = rootFrame;
        //            }

        //            if (rootFrame.Content == null)
        //            {
        //                // When the navigation stack isn't restored navigate to the first page,
        //                // configuring the new page by passing required information as a navigation
        //                // parameter
        //                rootFrame.Navigate(typeof(MainPage), e.Arguments);
        //            }
        //            // Ensure the current window is active
        //            Window.Current.Activate();

        //        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        //Called after and when  DisplayRootView is called!!!
        protected override void Configure()
        {
            try
            {
                _container = new WinRTContainer();
                _container.RegisterWinRTServices();

                // Register your view models at the container
                // Make sure to register your containers here
                _container.PerRequest<SomeViewModel>();
                _container.PerRequest<AnotherViewModel>();
            }
            catch (Exception exception)
            {
                _log.Error(exception);
                throw;
            }
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetInstance(service, key);
            if (instance != null)
                return instance;
            throw new Exception("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
