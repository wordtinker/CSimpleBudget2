﻿
namespace ViewModels.Interfaces
{
    // TODO
    public interface IUIMainWindowService
    {
        void ShowMessage(string message);
        //void ShowBudgetReport();
        //void ShowBalanceReport();
        //void ShowCategoriesReport();
        ////void ShowTransactionRoll(AccountItem accItem);
        //void ManageAccountTypes();
        //void ManageAccounts();
        //void ManageCategories();
        //void ManageBudget();
        void Shutdown();
        string LastSavedFileName { get; set; }
        string SaveFileDialog(string fileExtension);
        string OpenFileDialog(string fileExtension);
    }
    // TODO
    public interface IUITransactionRollService
    {
        //bool? ShowTransactionEditor(TransactionEditorViewModel vm);
    }
    // TODO
    public interface IUIBudgetWindowService
    {
        //bool? ShowBudgetRecordEditor(BudgetRecordEditorViewModel vm);
        //void ShowMessage(string message);
        //bool RequestMonthAndYear(out int monthToCopyFrom, out int yearToCopyFrom);
    }
}
