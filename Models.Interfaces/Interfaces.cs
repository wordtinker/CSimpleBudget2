
using System;
using System.Collections.Generic;

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
        // TODO IStorage
    }
    public interface IDataProvider
    {
        IEnumerable<string> GetAccountTypes();
        bool AddAccountType(string accountType);
        bool DeleteAccountType(string accountType);

        IEnumerable<IAccount> GetAccounts();
        bool AddAccount(string accTyp, string accName, out IAccount newAccount);
        bool DeleteAccount(IAccount account);
        void UpdateAccount(IAccount account);

        IEnumerable<ICategory> GetCategories();
        bool AddCategory(string name, ICategory parent, out ICategory newCategory);
        bool DeleteCategory(ICategory category);

        IEnumerable<ITransaction> GetTransactions(IAccount account);
        void DeleteTransaction(ITransaction transaction);
        ITransaction AddTransaction(IAccount account, DateTime date, decimal amount, string info, ICategory category);
        void UpdateTransaction(ITransaction transaction, DateTime date, decimal amount, string info, ICategory category);

        (int minYear, int maxYear) GetActiveBudgetYears();
        IEnumerable<IBudgetRecord> CopyRecords(int fromMonth, int fromYear, int toMonth, int toYear);
        IEnumerable<IBudgetRecord> GetRecords(int year, int month);
        void DeleteRecord(IBudgetRecord record);
        IBudgetRecord AddBudgetRecord(decimal amount, ICategory category, BudgetType budgetType, int onDay, int month, int year);
    }
    public interface IAccount
    {
        string Name { get; set; }
        string Type { get; set; }
        decimal Balance { get; set; }
        bool Closed { get; set; }
        bool Excluded { get; set; }
    }
    public interface ICategory
    {
        string Name { get; }
        ICategory Parent { get; }
        IEnumerable<ICategory> Children { get; }
    }
    // TODO allow set?
    public interface ITransaction
    {
        DateTime Date { get; }
        decimal Amount { get; }
        string Info { get; }
        ICategory Category { get; }
        IAccount Account { get; }
    }
    public interface IBudgetRecord
    {
        decimal Amount { get; }
        ICategory Category { get; }
        BudgetType Type { get; }
        int Year { get; }
        // TODO model, month must be natural based
        int Month { get; }
        int OnDay { get; }
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
