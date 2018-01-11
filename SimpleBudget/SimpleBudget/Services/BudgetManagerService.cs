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

        public void RequestMonthAndYear(int toMonth, int toYear)
        {
            BudgetManagerCopyRequest requestWindow = new BudgetManagerCopyRequest
            {
                Owner = managerWindow,
                DataContext = App.Container.Resolve<BudgetManagerCopyRequestViewModel>(new ResolverOverride[]
                {
                    new ParameterOverride("toMonth", toMonth),
                    new ParameterOverride("toYear", toYear)
                })
            };
            requestWindow.ShowDialog();
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
