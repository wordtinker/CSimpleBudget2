using Microsoft.Win32;
using System.Windows;
using ViewModels.Interfaces;

namespace SimpleBudget.Windows
{
    class MainWindowService : IUIMainWindowService
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
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        public void Shutdown()
        {
            App.Current.Shutdown();
        }
    }
}
