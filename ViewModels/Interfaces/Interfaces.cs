﻿using ViewModels.Elements;

namespace ViewModels.Interfaces
{
    public interface IUIBaseService
    {
        void ShowMessage(string message);
    }
    public interface IUIMainWindowService : IUIBaseService
    {
        void ShowBudgetReport();
        void ShowBalanceReport();
        void ShowCategoriesReport();
        void ShowTransactionRoll(AccountItem accItem);
        void ManageAccountTypes();
        void ManageAccounts();
        void ManageCategories();
        void ManageBudget();
        void Shutdown();
        void ShowHelp();
        string LastSavedFileName { get; set; }
        string SaveFileDialog(string fileExtension);
        string OpenFileDialog(string fileExtension);
    }
    // TODO
    public interface IUITransactionRollService : IUIBaseService
    {
        //bool? ShowTransactionEditor(TransactionEditorViewModel vm);
    }
    // TODO
    public interface IUIBudgetWindowService
    {
        //bool? ShowBudgetRecordEditor(BudgetRecordEditorViewModel vm);
        //bool RequestMonthAndYear(out int monthToCopyFrom, out int yearToCopyFrom);
    }
}
