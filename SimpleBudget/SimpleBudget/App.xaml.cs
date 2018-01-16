using Prism.Events;
using Prism.Logging;
using SimpleBudget.Services;
using SimpleBudget.Windows;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using Unity;
using Unity.Resolution;
using ViewModels.Interfaces;
using ViewModels.Windows;

namespace SimpleBudget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IUnityContainer Container { get; private set; }
        /// <summary>
        /// Prepares environmental settings for app and starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            SetupCulture();
            // Get app name from config file
            string appName = Tools.Settings.Read("appName");
            if (string.IsNullOrWhiteSpace(appName))
            {
                MessageBox.Show("Error reading app settings.\nApp can't start.");
                App.Current.Shutdown();
                return;
            }
            // Get app directory
            string appDir;
            try
            {
                appDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                appDir = Path.Combine(appDir, appName);
                Directory.CreateDirectory(appDir);
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("App can't start.\n{0}", err.Message));
                App.Current.Shutdown();
                return;
            }

            // Configure and start model
            var modelFactory = new ModelFactory.Factory();
            // Make unity container out of model container
            Container = modelFactory.Container.CreateChildContainer();
            // Register event aggregator
            Container.RegisterInstance<IEventAggregator>(new EventAggregator());
            // Register base service provider
            Container.RegisterInstance<IUIBaseService>(new BaseWindowService());
            // Register logging class
            Container.RegisterInstance<ILoggerFacade>(new SimpleLogger(appDir));
            // Start main window
            MainWindow = new MainWindow
            {
                Title = appName
            };
            // Inject dependencies and properties
            IUIMainWindowService service = new MainWindowService(MainWindow);
            MainWindow.DataContext = Container.Resolve<MainWindowViewModel>(new ParameterOverride("windowService", service));
            MainWindow.Show();
        }
        private void SetupCulture()
        {
            // Creating a Global culture specific to our application.
            CultureInfo cultureInfo = new CultureInfo("en-US");
            // Creating the DateTime Information specific to our application.
            DateTimeFormatInfo dateTimeInfo = new DateTimeFormatInfo
            {
                // Defining various date and time formats.
                DateSeparator = "/",
                LongDatePattern = "dd/MM/yyyy",
                ShortDatePattern = "dd/MM/yyyy"
            };
            // Setting application wide date time format.
            cultureInfo.DateTimeFormat = dateTimeInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
        /// <summary>
        /// General exception cather. Logs exceptions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Container.Resolve<ILoggerFacade>()
                .Log(e.Exception.ToString(), Category.Exception, Priority.High);
        }
    }
}
