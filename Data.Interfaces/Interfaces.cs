using System;
using System.Collections.Generic;

namespace Data.Interfaces
{
    public interface IStorage : IDisposable
    {
        bool Initialize();

        IEnumerable<string> SelectAccountTypes();
        bool AddAccountType(string name);
        bool DeleteAccountType(string name);

        IEnumerable<(string name, string type, decimal balance, bool closed, bool excluded, int id)> SelectAccounts();
        bool AddAccount(string name, string type, out int id);
        bool UpdateAccount(int id, string type, decimal balance, bool closed, bool excluded);
        bool DeleteAccount(int id);

        IEnumerable<(string name, int id)> SelectTopCategories();
        IEnumerable<(string name, int id)> SelectSubCategories(int parentId);
        bool AddTopCategory(string name, out int id);
        bool AddSubCategory(string name, int parentId, out int id);
        bool DeleteTopCategory(int id);
        bool DeleteSubCategory(int id);

        IEnumerable<(DateTime date, decimal amount, string info, int categoryId, int id)> SelectTransactions(int accountId);
        bool DeleteTransaction(int id);
        bool AddTransaction(int accountId, DateTime date, decimal amount, string info, int categoryId, out int id);
        bool UpdateTransaction(int id, DateTime date, decimal amount, string info, int categoryId);
        decimal SelectTransactionsCombined(int year, int month, int categoryId);

        IEnumerable<(decimal amount, int categoryId, string type, int onDay, int id)> SelectRecords(int year, int month);
        bool AddRecord(decimal amount, int categoryId, string type, int onDay, int year, int month, out int id);
        bool DeleteRecord(int id);
        bool UpdateRecord(int id, decimal amount, int categoryId, string type, int onDay, int year, int month);
        decimal SelectRecordsCombined(int year, int month, int categoryId);

        int? GetMaximumYear();
        int? GetMinimumYear();
    }
}
