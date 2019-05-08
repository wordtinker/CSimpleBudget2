using Models.Interfaces;
using System;
using System.Collections.ObjectModel;
/// <summary>
/// Interfaces are a part of the project, both interfaces and
/// implementation are referenced by the same View project.
/// </summary>
namespace ViewModels.Interfaces
{
    public interface ICategoryNode
    {
        ICategory InnerCategory { get; }
        string FullName { get; }
        ObservableCollection<ICategoryNode> Items { get; }
        ICategoryNode Parent { get; set; }
        string Title { get; }

        void AddChild(ICategoryNode node);
        void RemoveChild(ICategoryNode node);
    }

    public interface ITransactionItem
    {
        ITransaction InnerTransaction { get; }
        string Account { get; }
        decimal Amount { get; set; }
        ICategoryNode Category { get; set; }
        DateTime Date { get; set; }
        string Info { get; set; }
    }

    public interface IRecordItem
    {
        IBudgetRecord InnerRecord { get; }
        decimal Amount { get; set; }
        ICategoryNode Category { get; set; }
        int Month { get; set; }
        int OnDay { get; set; }
        string OnDayText { get; }
        BudgetType Type { get; set; }
        string TypeName { get; }
        int Year { get; set; }
    }

    public interface IAccountItem
    {
        string Name { get; }
        string Type { get; set; }
        decimal Balance { get; }
        bool Closed { get; set; }
        bool Excluded { get; set; }

    }

    public interface IUIBaseService
    {
        /// <summary>
        /// Informs a user with a message.
        /// </summary>
        /// <param name="message">Message to be shown.</param>
        void ShowMessage(string message);
    }
    public interface IUIMainWindowService : IUIBaseService
    {
        /// <summary>
        /// Shows a window with a budget report.
        /// </summary>
        void ShowBudgetReport();
        /// <summary>
        /// Shows a window with a balance report.
        /// </summary>
        void ShowBalanceReport();
        /// <summary>
        /// Shows a window with a categories report.
        /// </summary>
        void ShowCategoriesReport();
        /// <summary>
        /// Shows a windows with all transaction for a
        /// specified account.
        /// </summary>
        /// <param name="accItem">Selected account.</param>
        void ShowTransactionRoll(IAccountItem accItem);
        /// <summary>
        /// Shows a windows that manages account types.
        /// </summary>
        void ManageAccountTypes();
        /// <summary>
        /// Shows a windows that manages accounts.
        /// </summary>
        void ManageAccounts();
        /// <summary>
        /// Shows a windows that manages budget categories.
        /// </summary>
        void ManageCategories();
        /// <summary>
        /// Shows a windows that manages budget records.
        /// </summary>
        void ManageBudget();
        /// <summary>
        /// Shut down the app.
        /// </summary>
        void Shutdown();
        /// <summary>
        /// Shows a little help.
        /// </summary>
        void ShowHelp();
        /// <summary>
        /// Property for last saved file name.
        /// </summary>
        string LastSavedFileName { get; set; }
        /// <summary>
        /// Shows window for saving file.
        /// </summary>
        /// <param name="fileExtension">File extension in the ".ext" format.</param>
        /// <returns></returns>
        string SaveFileDialog(string fileExtension);
        /// <summary>
        /// Shows window for opening file.
        /// </summary>
        /// <param name="fileExtension">File extension in the ".ext" format.</param>
        /// <returns></returns>
        string OpenFileDialog(string fileExtension);
    }
    public interface IUITransactionRollService : IUIBaseService
    {
        /// <summary>
        /// Shows a transaction editor for a new transaction
        /// for a given account.
        /// </summary>
        /// <param name="accountItem"></param>
        void ShowTransactionEditor(IAccountItem accountItem);
        /// <summary>
        /// Shows a transaction editor for an existing
        /// transaction.
        /// </summary>
        /// <param name="transactionItem"></param>
        void ShowTransactionEditor(ITransactionItem transactionItem);
    }
    public interface IUIBudgetWindowService :IUIBaseService
    {
        /// <summary>
        /// Shows a budget record editor for a new budget
        /// record.
        /// </summary>
        void ShowBudgetRecordEditor();
        /// <summary>
        /// Shows a budget record editor for an
        /// existing budget record.
        /// </summary>
        /// <param name="recordItem"></param>
        void ShowBudgetRecordEditor(IRecordItem recordItem);
        /// <summary>
        /// Shows a window with selection of month and year
        /// for a budget records
        /// that would be copied to a given month and year.
        /// </summary>
        /// <param name="toMonth"></param>
        /// <param name="toYear"></param>
        void RequestMonthAndYear(int toMonth, int toYear);
    }
}
