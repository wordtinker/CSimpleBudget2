using System;
using System.Collections.Generic;
using Models.Elements;
using Models.Interfaces;

namespace Models
{
    /// <summary>
    /// Object that contains sum of all transaction
    /// and sum of all budget records for a given
    /// period(month and year) and category.
    /// </summary>
    public class Spending : ISpending
    {
        // Category of the spending
        public ICategory Category { get; internal set; }
        // Sum of the planned budget records.
        public decimal Budget { get; internal set; }
        // Sum of the transactions.
        public decimal Value { get; internal set; }
        // Month of the spending
        public int Month { get; internal set; }
    }
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
    
    public class StubCategory : ICategory
    {
        public string Name { get; set; }
        public ICategory Parent { get; set; }
        public IEnumerable<ICategory> Children { get; set; }
    }

    public class DataProvider : StubDataProvider, IDataProvider
    {
        private IStorageProvider storageProvider;

        public DataProvider(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }
        public IEnumerable<string> GetAccountTypes()
        {
            foreach(string accType in storageProvider.Storage?.SelectAccTypes())
            {
                yield return accType;
            }
        }
        public bool AddAccountType(string accountType)
        {
            return storageProvider.Storage?.AddAccountType(accountType) ?? false;
        }
        public bool DeleteAccountType(string accountType)
        {
            return storageProvider.Storage?.DeleteAccountType(accountType) ?? false;
        }
        public IEnumerable<IAccount> GetAccounts()
        {
            foreach (var (name, type, balance, closed, excluded, id)
                in storageProvider.Storage?.SelectAccounts())
            {
                yield return new Account
                {
                    Name = name,
                    Type = type,
                    Balance = balance,
                    Closed = closed,
                    Excluded = excluded,
                    Id = id
                };
            }
        }
        public bool AddAccount(string accType, string accName, out IAccount newAccount)
        {
            int id = -1;
            if(storageProvider.Storage?.AddAccount(accName, accType, out id) ?? false)
            {
                newAccount = new Account
                {
                    Name = accName,
                    Type = accType,
                    Balance = 0m,
                    Closed = false,
                    Excluded = false,
                    // id won't be -1 here, AddAccount returns valid id on successful run
                    Id = id
                };
                return true;
            }
            else
            {
                newAccount = null;
                return false;
            }
        }
        public bool UpdateAccount(IAccount account)
        {
            return storageProvider.Storage?
                .UpdateAccount(account.Id, account.Type, account.Balance, account.Closed, account.Excluded ) ?? false;
        }
        public bool DeleteAccount(IAccount account)
        {
            return storageProvider.Storage?.DeleteAccount(account.Id) ?? false;
        }
    }

    public class StubDataProvider
    {
        

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

        public ITransaction AddTransaction(IAccount account, DateTime date, decimal amount, string info, ICategory category)
        {
            return new StubTransaction {Account = account, Date = date, Amount = amount, Info = info, Category = category };
        }

        public IEnumerable<IBudgetRecord> CopyRecords(int fromMonth, int fromYear, int toMonth, int toYear)
        {
            return GetRecords(fromMonth, fromYear);
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
                Year = 2018
            };
            yield return br;
        }

        public IEnumerable<ISpending> GetSpendings(int year, int month)
        {
            List<ICategory> categories = new List<ICategory>(GetCategories());
            var subCats = new List<ICategory>(categories[0].Children);
            yield return new Spending
            {
                Budget = 25m,
                Category = subCats[0],
                Month = 1,
                Value = 20m
            };
        }

        public IEnumerable<ITransaction> GetTransactions(IAccount account)
        {
            List<ICategory> categories = new List<ICategory>(GetCategories());
            var subCats = new List<ICategory>(categories[0].Children);
            yield return new StubTransaction
            {
                Account = new Account { Name = "One" },
                Amount = 25.14m,
                Category = subCats[1],
                Date = DateTime.Now,
                Info = "Test"
            };
        }

        

        public void UpdateRecord(IBudgetRecord record, decimal amount, ICategory category, BudgetType budgetType, int onDay, int month, int year)
        {
            // do nothing
        }

        public void UpdateTransaction(ITransaction transaction, DateTime date, decimal amount, string info, ICategory category)
        {
            // do nothing
        }
    }
}
