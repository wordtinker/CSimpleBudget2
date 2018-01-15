
using System;
using System.Collections.Generic;
using Data.Interfaces;

namespace Data
{
    public class StubStorage : IStorage
    {
        public bool AddAccount(string name, string type, out int id)
        {
            id = DateTime.Now.Millisecond;
            return true;
        }

        public bool AddAccountType(string name)
        {
            return true;
        }

        public bool AddSubCategory(string name, int parentId, out int id)
        {
            id = 2;
            return true;
        }

        public bool AddTopCategory(string name, out int id)
        {
            id = 2;
            return true;
        }

        public bool AddTransaction(int accountId, DateTime date, decimal amount, string info, int categoryId, out int id)
        {
            id = 10;
            return true;
        }

        public bool DeleteAccount(int id)
        {
            return true;
        }

        public bool DeleteAccountType(string name)
        {
            return true;
        }

        public bool DeleteSubCategory(int id)
        {
            return true;
        }

        public bool DeleteTopCategory(int id)
        {
            return true;
        }

        public bool DeleteTransaction(int id)
        {
            return true;
        }

        public void Dispose()
        {
            // TODO using pattern
        }

        public int? GetMaximumYear()
        {
            return 2016;
        }

        public int? GetMinimumYear()
        {
            return 2013;
        }

        public bool Initialize()
        {
            return true;
        }

        public IEnumerable<(string name, string type, decimal balance, bool closed, bool excluded, int id)> SelectAccounts()
        {
            yield return ("1254", "one", 1254m, false, false, 1);
            yield return ("1254", "two", 1254m, true, true, 2);
            yield return ("8745", "one", 8745m, false, false, 3);
        }

        public IEnumerable<string> SelectAccTypes()
        {
            yield return "one";
            yield return "two";
            yield return "2222";
        }

        public IEnumerable<(string name, int id)> SelectSubCategories(int parentId)
        {
            yield return ("one", 1);
            yield return ("two", 2);
        }

        public IEnumerable<(string name, int id)> SelectTopCategories()
        {
            yield return ("Top one", 1);
        }

        public IEnumerable<(DateTime date, decimal amount, string info, int categoryId, int id)>
            SelectTransactions(int accountId)
        {
            yield return (DateTime.Now, 25.14m, "Test", 2, 1);
        }

        public bool UpdateAccount(int id, string type, decimal balance, bool closed, bool excluded)
        {
            return true;
        }

        public bool UpdateTransaction(int id, DateTime date, decimal amount, string info, int categoryId)
        {
            return true;
        }
        // TODO ctor might throw exception
    }
}
