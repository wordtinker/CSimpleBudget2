
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
        void AddTransaction(IAccount account, DateTime date, decimal amount, string info, ICategory category, out ITransaction transaction);
        void UpdateTransaction(ITransaction transaction, DateTime date, decimal amount, string info, ICategory category);
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
    public interface ITransaction
    {
        DateTime Date { get; }
        decimal Amount { get; }
        string Info { get; }
        ICategory Category { get; }
        IAccount Account { get; }
    }
}
