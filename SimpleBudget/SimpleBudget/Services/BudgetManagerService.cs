using System.Windows;
using ViewModels.Interfaces;

namespace SimpleBudget.Services
{
    class BudgetManagerService : BaseWindowService, IUIBudgetWindowService
    {
        private Window window;

        public BudgetManagerService(Window window)
        {
            this.window = window;
        }

    }
}
