using Microsoft.Win32;
using SimpleBudget.Reports;
using SimpleBudget.Windows;
using System;
using System.Reflection;
using System.Windows;
using Unity;
using Unity.Resolution;
using ViewModels.Elements;
using ViewModels.Interfaces;
using ViewModels.Reports;
using ViewModels.Windows;

namespace SimpleBudget.Services
{
    class BaseWindowService : IUIBaseService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
    class MainWindowService : BaseWindowService, IUIMainWindowService
    {
        private Window mainWindow;

        public MainWindowService(Window mainWindow)
        {
            this.mainWindow = mainWindow;
        }
        public string SaveFileDialog(string fileExtension)
        {
            string text = string.Format("Budget files (*{0})|*{0}", fileExtension);
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = text
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }
        public string OpenFileDialog(string fileExtension)
        {
            string text = string.Format("Budget files (*{0})|*{0}", fileExtension);
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = text
            };
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }
        public string LastSavedFileName
        {
            // Fetch filename from config file, it could be empty.
            get => Properties.Settings.Default.FileName;
            set
            {
                Properties.Settings.Default.FileName = value;
                Properties.Settings.Default.Save();
            }
        }
        public void Shutdown()
        {
            App.Current.Shutdown();
        }
        public void ManageAccountTypes()
        {
            AccTypeManager window = new AccTypeManager
            {
                Owner = mainWindow,
                DataContext = App.Container.Resolve<AccTypeManagerViewModel>()
            };
            window.ShowDialog();
        }
        public void ManageAccounts()
        {
            AccountsManager window = new AccountsManager
            {
                Owner = mainWindow,
                DataContext = App.Container.Resolve<AccountsManagerViewModel>()
            };
            window.ShowDialog();
        }

        public void ManageCategories()
        {
            CategoriesManager window = new CategoriesManager
            {
                Owner = mainWindow,
                DataContext = App.Container.Resolve<CategoriesManagerViewModel>()
            };
            window.ShowDialog();
        }
        public void ShowTransactionRoll(AccountItem accItem)
        {
            TransactionRoll window = new TransactionRoll()
            {
                Owner = mainWindow
            };
            IUITransactionRollService service = new TransactionRollService(window);
            window.DataContext = App.Container.Resolve<TransactionRollViewModel>(new ResolverOverride[]
            {
                new ParameterOverride("accItem", accItem),
                new ParameterOverride("service", service)
            });
            window.ShowDialog();
        }

        public void ManageBudget()
        {
            BudgetManager window = new BudgetManager
            {
                Owner = mainWindow
            };
            IUIBudgetWindowService service = new BudgetManagerService(window);
            window.DataContext = App.Container.Resolve<BudgetManagerViewModel>(
                new ParameterOverride("service", service));
            window.ShowDialog();
        }

        public void ShowBudgetReport()
        {
            BudgetReport window = new BudgetReport
            {
                Owner = mainWindow,
                DataContext = App.Container.Resolve<BudgetReportViewModel>()
            };
            window.ShowDialog();
        }

        public void ShowBalanceReport()
        {
            BalanceReport window = new BalanceReport
            {
                Owner = mainWindow,
                DataContext = App.Container.Resolve<BalanceReportViewModel>()
            };
            window.ShowDialog();
        }

        public void ShowCategoriesReport()
        {
            // TODO
        }

        public void ShowHelp()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            MessageBox.Show($"Simple Budget 2: {version.ToString()}");
        }
    }
}
