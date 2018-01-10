using SimpleBudget.Windows;
using System.Windows;
using Unity;
using ViewModels.Interfaces;
using ViewModels.Windows;

namespace SimpleBudget.Services
{
    class BudgetManagerService : BaseWindowService, IUIBudgetWindowService
    {
        private Window window;

        public BudgetManagerService(Window window)
        {
            this.window = window;
        }

        public bool RequestMonthAndYear(out int month, out int year)
        {
            BudgetManagerCopyRequest requestWindow = new BudgetManagerCopyRequest
            {
                Owner = window
            };
            BudgetManagerCopyRequestViewModel context = App.Container.Resolve<BudgetManagerCopyRequestViewModel>();
            requestWindow.DataContext = context;

            if (requestWindow.ShowDialog() == true)
            {
                month = context.SelectedMonth;
                year = context.SelectedYear;
                return true;
            }
            else
            {
                month = 0;
                year = 0;
                return false;
            }
        }
    }
}
