using System;
using System.Collections.Generic;

namespace Data.Interfaces
{
    public interface IStorage : IDisposable
    {
        /// <summary>
        /// Runs storage initialization procedures.
        /// </summary>
        /// <returns></returns>
        bool Initialize();

        /// <summary>
        /// Provides a list of account types.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> SelectAccountTypes();
        /// <summary>
        /// Adds account type into storage.
        /// </summary>
        /// <param name="name">Account type name.</param>
        /// <returns>True if account type was added.</returns>
        bool AddAccountType(string name);
        /// <summary>
        /// Deletes account type from storage.
        /// </summary>
        /// <param name="name">Account type name</param>
        /// <returns>True if account type was deleteed.</returns>
        bool DeleteAccountType(string name);

        /// <summary>
        /// Provides a list of accounts.
        /// </summary>
        /// <returns></returns>
        IEnumerable<(string name, string type, decimal balance, bool closed, bool excluded, int id)> SelectAccounts();
        /// <summary>
        /// Adds account into storage.
        /// </summary>
        /// <param name="name">Name of the account.</param>
        /// <param name="type">Account type.</param>
        /// <param name="id">Returns id of newly created account or -1.</param>
        /// <returns>True if account was created.</returns>
        bool AddAccount(string name, string type, out int id);
        /// <summary>
        /// Updates account in the storage.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <param name="type">Account type.</param>
        /// <param name="balance">Account balance.</param>
        /// <param name="closed">True if account is closed.</param>
        /// <param name="excluded">True if account is excluded from budget.</param>
        /// <returns>True if account was updated.</returns>
        bool UpdateAccount(int id, string type, decimal balance, bool closed, bool excluded);
        /// <summary>
        /// Deletes account from the storage.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <returns>True if account was deleted.</returns>
        bool DeleteAccount(int id);

        /// <summary>
        /// Provides a list of top level categories.
        /// </summary>
        /// <returns></returns>
        IEnumerable<(string name, int id)> SelectTopCategories();
        /// <summary>
        /// Provides a list of sub level categories
        /// for a given top category.
        /// </summary>
        /// <param name="parent">Name of the top level
        /// category.</param>
        /// <returns></returns>
        IEnumerable<(string name, int id)> SelectSubCategories(string parent);
        /// <summary>
        /// Adds top level category to the storage.
        /// </summary>
        /// <param name="name">Category name.</param>
        /// <param name="id">Returns newly created id.</param>
        /// <returns>True if top category was added.</returns>
        bool AddTopCategory(string name, out int id);
        /// <summary>
        /// Adds sub level category to the storage.
        /// </summary>
        /// <param name="name">Category name.</param>
        /// <param name="parent">Parent category name.</param>
        /// <param name="id">Returns newly created id.</param>
        /// <returns>True if sub category was added.</returns>
        bool AddSubCategory(string name, string parent, out int id);
        /// <summary>
        /// Deletes top category.
        /// </summary>
        /// <param name="name">Category name.</param>
        /// <returns>True if category was deleted.</returns>
        bool DeleteTopCategory(string name);
        /// <summary>
        /// Deletes sub category.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>True if category was deleted.</returns>
        bool DeleteSubCategory(int id);

        /// <summary>
        /// Provides a list of transaction for a given account.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IEnumerable<(DateTime date, decimal amount, string info, int categoryId, int id)> SelectTransactions(int accountId);
        /// <summary>
        /// Provides a list of transaction for a given year and month.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        IEnumerable<(DateTime date, decimal amount, string info, int categoryId, int accountId, int id)> SelectTransactions(int year, int month);
        /// <summary>
        /// Provides a list of transaction for a given year, month and category.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        IEnumerable<(DateTime date, decimal amount, string info, int categoryId, int accountId, int id)> SelectTransactions(int year, int month, int categoryId);
        /// <summary>
        /// Deletes a transaction.
        /// </summary>
        /// <param name="id">Transaction id.</param>
        /// <returns>True if transaction was deleted.</returns>
        bool DeleteTransaction(int id);
        /// <summary>
        /// Adds a transaction.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="info"></param>
        /// <param name="categoryId"></param>
        /// <param name="id">Returns newly created transaction id or -1.</param>
        /// <returns>True if transaction was added.</returns>
        bool AddTransaction(int accountId, DateTime date, decimal amount, string info, int categoryId, out int id);
        /// <summary>
        /// Updates transaction.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="info"></param>
        /// <param name="categoryId"></param>
        /// <returns>True if transaction was updated.</returns>
        bool UpdateTransaction(int id, DateTime date, decimal amount, string info, int categoryId);
        /// <summary>
        /// Returns total decimal value of all transactions for specified
        /// year, month and category.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        decimal SelectTransactionsCombined(int year, int month, int categoryId);
        /// <summary>
        /// Returns total decimal value of all transactions up to specified date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        decimal SelectTransactionsCombinedUpTo(DateTime date);
        /// <summary>
        /// Provides date of the last in budget account transaction prior to the specified
        /// month and year.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        DateTime SelectLastTransactionDate(int year, int month);

        /// <summary>
        /// Provides a list of budget records for a given month and year.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        IEnumerable<(decimal amount, int categoryId, string type, int onDay, int id)> SelectRecords(int year, int month);
        /// <summary>
        /// Adds budget record.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="categoryId"></param>
        /// <param name="type"></param>
        /// <param name="onDay"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="id">Returns id of newly created record or -1.</param>
        /// <returns>True if record was created.</returns>
        bool AddRecord(decimal amount, int categoryId, string type, int onDay, int year, int month, out int id);
        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if record was deleted.</returns>
        bool DeleteRecord(int id);
        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <param name="categoryId"></param>
        /// <param name="type"></param>
        /// <param name="onDay"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>True if record was updated.</returns>
        bool UpdateRecord(int id, decimal amount, int categoryId, string type, int onDay, int year, int month);
        /// <summary>
        /// Calculates decimal value of all budget records for a given year, month and category.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        decimal SelectRecordsCombined(int year, int month, int categoryId);

        /// <summary>
        /// Returns the last year of the available budget record
        /// or transaction.
        /// </summary>
        /// <returns></returns>
        int? GetMaximumYear();
        /// <summary>
        /// Returns the first year of the available budget record
        /// or transaction.
        /// </summary>
        /// <returns></returns>
        int? GetMinimumYear();
    }
}
