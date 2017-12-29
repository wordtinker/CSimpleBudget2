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
    }
}
