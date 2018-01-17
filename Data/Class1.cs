
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

        public bool DeleteRecord(int id)
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

        

        public bool UpdateRecord(int id, decimal amount, int categoryId, string type, int onDay, int year, int month)
        {
            return true;
        }

        
    }
}
