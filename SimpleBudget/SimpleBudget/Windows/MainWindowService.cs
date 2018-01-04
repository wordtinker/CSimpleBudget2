using Microsoft.Win32;
using System.Windows;
using ViewModels.Interfaces;
using ViewModels.Windows;
using Unity;

namespace SimpleBudget.Windows
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
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = fileExtension
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
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = fileExtension
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

        public void ManageBudget()
        {
            // TODO !!!
        }
    }
}
