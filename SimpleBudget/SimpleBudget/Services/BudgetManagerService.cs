using SimpleBudget.Windows;
using System.Windows;
using Unity;
using Unity.Resolution;
using ViewModels.Elements;
using ViewModels.Interfaces;
using ViewModels.Windows;

namespace SimpleBudget.Services
{
    class BudgetManagerService : BaseWindowService, IUIBudgetWindowService
    {
        private Window managerWindow;

        public BudgetManagerService(Window window)
        {
            this.managerWindow = window;
        }

        public bool RequestMonthAndYear(out int month, out int year)
        {
            BudgetManagerCopyRequest requestWindow = new BudgetManagerCopyRequest
            {
                Owner = managerWindow
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
        public void ShowBudgetRecordEditor()
        {
            BudgetRecordEditor editor = new BudgetRecordEditor
            {
                DataContext = App.Container.Resolve<NewBudgetRecordEditorViewModel>(),
                Owner = managerWindow
            };
            editor.ShowDialog();
        }
        public void ShowBudgetRecordEditor(RecordItem recordItem)
        {
            BudgetRecordEditor editor = new BudgetRecordEditor
            {
                DataContext = App.Container.Resolve<OldBudgetRecordEditorViewModel>(
                    new ParameterOverride("recordItem", recordItem)),
                Owner = managerWindow
            };
            editor.ShowDialog();
        }
    }
}
