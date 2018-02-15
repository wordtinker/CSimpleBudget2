
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Models.Interfaces
{
    public interface IFileHandler
    {
        string Extension { get; }
        bool InitializeFile(string fileName);
        bool LoadFile(string fileName);
        void CloseFile();
    }
    public interface IStorageProvider
    {
        event EventHandler On;
        event EventHandler Off;
        IStorage Storage { get; }
    }
    public interface IPredictor
    {
        IEnumerable<(DateTime date, decimal amount, ICategory category)> Predict(int year, int month);
    }
    public interface IDataProvider
    {
        ObservableCollection<string> AccountTypes { get; }
        bool AddAccountType(string accountType);
        bool DeleteAccountType(string accountType);

        ObservableCollection<IAccount> Accounts { get; }
        bool AddAccount(string accName, out IAccount newAccount);
        bool DeleteAccount(IAccount account);
        bool UpdateAccount(IAccount account);

        ObservableCollection<ICategory> Categories { get; }
        bool AddCategory(string name, ICategory parent, out ICategory newCategory);
        bool DeleteCategory(ICategory category);

        IEnumerable<ITransaction> GetTransactions(IAccount account);
        IEnumerable<ITransaction> GetTransactions(int year, int month);
        IEnumerable<ITransaction> GetTransactions(int year, int month, ICategory category);
        bool DeleteTransaction(ITransaction transaction);
        bool AddTransaction(IAccount account, DateTime date, decimal amount, string info, ICategory category, out ITransaction newTransaction);
        bool UpdateTransaction(ITransaction transaction, DateTime date, decimal amount, string info, ICategory category);

        (int minYear, int maxYear) GetActiveBudgetYears();
        IEnumerable<IBudgetRecord> CopyRecords(int fromMonth, int fromYear, int toMonth, int toYear);
        IEnumerable<IBudgetRecord> GetRecords(int year, int month);
        bool DeleteRecord(IBudgetRecord record);
        bool AddBudgetRecord(decimal amount, ICategory category, BudgetType budgetType, int onDay, int year, int month, out IBudgetRecord newRecord);
        bool UpdateRecord(IBudgetRecord record, decimal amount, ICategory category, BudgetType budgetType, int onDay, int month, int year);

        IEnumerable<ISpending> GetSpendings(int year, int month);
        (decimal balance, DateTime date) GetBalanceToDate(int year, int month);
    }
    public interface IAccount
    {
        /// <summary>
        /// Account name. Name in NOT unique. 
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Corresponding account type reference. 
        /// </summary>
        string Type { get; set; }
        /// <summary>
        /// Total sum of all transactions. 
        /// </summary>
        decimal Balance { get; set; }
        /// <summary>
        /// Flag "Closed"
        /// Defines if the account is closed. Closed accounts do
        /// not contibute to totals. Closed accounts are considered
        /// in budgeting.
        /// True - Account is closed.
        /// False - Account is opened. 
        /// </summary>
        bool Closed { get; set; }
        /// <summary>
        /// Flag "Out of Budget"
        /// Defines if the account is considered in budgeting.
        /// False - Transactions from that account are included in all budget
        /// reports and forecasts.
        /// True - Transactions from that account are excluded from all budget
        /// reports and forecasts.
        /// </summary>
        bool Excluded { get; set; }
        /// <summary>
        /// Unique id.
        /// </summary>
        int Id { get; }
    }
    public interface ICategory
    {
        /// <summary>
        /// Name of the category.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Parent category. If category is top tier, parent is Null.
        /// </summary>
        ICategory Parent { get; }
        /// <summary>
        /// Child categories. For bottom tier categories this list is empty.
        /// </summary>
        IEnumerable<ICategory> Children { get; }
        // Unique category ID.
        int Id { get; }
    }
    public interface ISpending
    {
        // Category of the spending
        ICategory Category { get; }
        // Sum of the planned budget records.
        decimal Budget { get; }
        // Sum of the transactions.
        decimal Value { get; }
        // Month of the spending
        int Month { get; }
    }
    public interface ITransaction
    {
        DateTime Date { get; }
        decimal Amount { get; }
        string Info { get; }
        ICategory Category { get; }
        IAccount Account { get; }
        int Id { get; }
    }
    public interface IBudgetRecord
    {
        // Budget record value.
        // Value is set for whole month regardless of budget type.
        decimal Amount { get; }
        // Budget category. Every budget has a category.
        ICategory Category { get; }
        // Type of budget behaviour.
        BudgetType Type { get; }
        // Planned year on which transaction will occur.
        int Year { get; }
        // Planned month on which transaction will occur.
        int Month { get; }
        // Planned day of the transaction.
        // 0 is default value for Daily, Monthly
        // 1-30 range is available for Point
        // 0-6 range is available for Weekly. See DayOfWeek enum.
        int OnDay { get; }
        int Id { get; }
    }
    /// <summary>
    /// Every budget record has a type that defines
    /// its behaviour in the budget forecasts.
    /// </summary>
    public enum BudgetType
    {
        Monthly, // One time spending, forecast spending is on the last day of the month
        Point, // One time spending on the specified day of the month
        Daily, // Spending is evenly divided among days of the month
        Weekly // Wekly spending on the specified day of the week
    }
}
