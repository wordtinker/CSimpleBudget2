
using System;
using System.Collections.Generic;

namespace Data
{
    public abstract class StubStorage
    {
        public bool AddRecord(decimal amount, int categoryId, string type, int onDay, int year, int month, out int id)
        {
            id = 11;
            return true;
        }

        public bool AddTransaction(int accountId, DateTime date, decimal amount, string info, int categoryId, out int id)
        {
            id = 10;
            return true;
        }

        public bool DeleteRecord(int id)
        {
            return true;
        }

        public bool DeleteTransaction(int id)
        {
            return true;
        }

        public int? GetMaximumYear()
        {
            return 2016;
        }

        public int? GetMinimumYear()
        {
            return 2013;
        }

        public IEnumerable<(decimal amount, int categoryId, string type, int onDay, int id)> SelectRecords(int year, int month)
        {
            yield return (100, 1, "Point", 1, 10);
        }

        public decimal SelectRecordsCombined(int year, int month, int categoryId)
        {
            return 25m;
        }

        public IEnumerable<(DateTime date, decimal amount, string info, int categoryId, int id)>
            SelectTransactions(int accountId)
        {
            yield return (DateTime.Now, 25.14m, "Test", 2, 1);
        }

        public decimal SelectTransactionsCombined(int year, int month, int categoryId)
        {
            return 20m;
        }

        public bool UpdateRecord(int id, decimal amount, int categoryId, string type, int onDay, int year, int month)
        {
            return true;
        }

        public bool UpdateTransaction(int id, DateTime date, decimal amount, string info, int categoryId)
        {
            return true;
        }
    }
}
