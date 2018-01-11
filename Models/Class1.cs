using System;
using System.Collections.Generic;
using Models.Interfaces;

namespace Models
{
    public class StubBudgetRecord : IBudgetRecord
    {
        public decimal Amount { get; set; }
        public ICategory Category { get; set; }
        public BudgetType Type { get; set; }
        public int OnDay { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
    public class StubTransaction : ITransaction
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Info { get; set; }
        public ICategory Category { get; set; }
        public IAccount Account { get; set; }
    }

    public class StubAccount : IAccount
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public bool Closed { get; set; }
        public string Type { get; set; }
        public bool Excluded { get; set; }
    }
    public class StubCategory : ICategory
    {
        public string Name { get; set; }
        public ICategory Parent { get; set; }
        public IEnumerable<ICategory> Children { get; set; }
    }

    public class StubFileHandler : IFileHandler, IStorageProvider
    {
        // TODO format?
        public string Extension => "Budget files (*.sbdb)|*.sbdb";

        public void CloseFile()
        {
            //
        }

        public bool InitializeFile(string fileName)
        {
            return true;
        }
        public bool LoadFile(string fileName)
        {
            return true;
        }
    }
    public class StubDataProvider : IDataProvider
    {
        public StubDataProvider(IStorageProvider storageProvider)
        {

        }

        public bool AddAccount(string accType, string accName, out IAccount newAccount)
        {
            newAccount = new StubAccount { Balance = 0, Closed = false, Name = accName, Type = accType };
            return true;
        }

        public bool AddAccountType(string accountType)
        {
            return true;
        }

        public IBudgetRecord AddBudgetRecord(decimal amount, ICategory category, BudgetType budgetType, int onDay, int month, int year)
        {
            return new StubBudgetRecord
            {
                Amount = amount,
                Category = category,
                Month = month,
                OnDay = onDay,
                Type = budgetType,
                Year = year
            };
        }

        public bool AddCategory(string name, ICategory parent, out ICategory newCategory)
        {
            newCategory = new StubCategory { Name = name, Parent = parent, Children = new List<ICategory>() };
            return true;
        }

        public void AddTransaction(IAccount account, DateTime date, decimal amount, string info, ICategory category, out ITransaction transaction)
        {
            transaction = new StubTransaction {Account = account, Date = date, Amount = amount, Info = info, Category = category };
        }

        public IEnumerable<IBudgetRecord> CopyRecords(int fromMonth, int fromYear, int toMonth, int toYear)
        {
            return GetRecords(fromMonth, fromYear);
        }

        public bool DeleteAccount(IAccount account)
        {
            return true;
        }

        public bool DeleteAccountType(string accountType)
        {
            return true;
        }

        public bool DeleteCategory(ICategory category)
        {
            return true;
        }

        public void DeleteRecord(IBudgetRecord record)
        {
            // Do nothing
        }

        public void DeleteTransaction(ITransaction transaction)
        {
            // do nothing
        }

        public IEnumerable<IAccount> GetAccounts()
        {
            yield return new StubAccount
            {
                Balance = 1254m,
                Closed = false,
                Name = "1254",
                Excluded = false,
                Type = "one"
            };
            yield return new StubAccount
            {
                Balance = 1254m,
                Closed = true,
                Name = "1254",
                Excluded = true,
                Type = "two"
            };
            yield return new StubAccount
            {
                Balance = 8745m,
                Closed = false,
                Name = "8745",
                Excluded = false,
                Type ="one"
            };
        }

        public IEnumerable<string> GetAccountTypes()
        {
            yield return "one";
            yield return "two";
        }

        public (int minYear, int maxYear) GetActiveBudgetYears()
        {
            return (2013, 2018);
        }

        // Top tier with structure
        public IEnumerable<ICategory> GetCategories()
        {
            StubCategory one = new StubCategory { Name = "one", Parent = null, Children = new List<ICategory>() };
            StubCategory two = new StubCategory { Name = "two", Parent = null, Children = new List<ICategory>() };
            StubCategory topOne = new StubCategory { Name = "Top one", Parent = null,
                Children = new List<ICategory>() { one, two } };
            one.Parent = topOne;
            two.Parent = topOne;
            yield return topOne;
        }

        public IEnumerable<IBudgetRecord> GetRecords(int year, int month)
        {
            List<ICategory> categories = new List<ICategory>(GetCategories());
            var subCats = new List<ICategory>(categories[0].Children);
            var br = new StubBudgetRecord
            {
                Amount = 100,
                Category = subCats[0],
                OnDay = 1,
                Type = BudgetType.Point,
                Month = 1,
                Year = 2017
            };
            yield return br;
        }

        public IEnumerable<ITransaction> GetTransactions(IAccount account)
        {
            List<ICategory> categories = new List<ICategory>(GetCategories());
            var subCats = new List<ICategory>(categories[0].Children);
            yield return new StubTransaction
            {
                Account = new StubAccount { Name = "One" },
                Amount = 25.14m,
                Category = subCats[1],
                Date = DateTime.Now,
                Info = "Test"
            };
        }

        public void UpdateAccount(IAccount account)
        {
            // do nothing
        }

        public void UpdateTransaction(ITransaction transaction, DateTime date, decimal amount, string info, ICategory category)
        {
            // do nothing
        }
    }
}
