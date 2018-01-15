using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

    public class DataProvider : StubDataProvider, IDataProvider
    {
        private IStorageProvider storageProvider;

        public ObservableCollection<string> AccountTypes { get; }
        public ObservableCollection<IAccount> Accounts { get; }
        // Top tier categories with structure
        public ObservableCollection<ICategory> Categories { get; }

        public DataProvider(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
            AccountTypes = new ObservableCollection<string>();
            Accounts = new ObservableCollection<IAccount>();
            Categories = new ObservableCollection<ICategory>();
            storageProvider.On += (sender, e) =>
            {
                SetAccountTypes();
                SetAccounts();
                SetCategories();
            };
            storageProvider.Off += (sender, e) =>
            {
                AccountTypes.Clear();
                Accounts.Clear();
                Categories.Clear();
            };
        }
        private void SetAccountTypes()
        {
            foreach (string accType in storageProvider.Storage?.SelectAccTypes())
            {
                AccountTypes.Add(accType);
            }
        }
        public bool AddAccountType(string accountType)
        {
            if (storageProvider.Storage?.AddAccountType(accountType) ?? false)
            {
                AccountTypes.Add(accountType);
                return true;
            }
            return false;
        }
        public bool DeleteAccountType(string accountType)
        {
            if (storageProvider.Storage?.DeleteAccountType(accountType) ?? false)
            {
                AccountTypes.Remove(accountType);
                return true;
            }
            return false;
        }
        private void SetAccounts()
        {
            foreach (var (name, type, balance, closed, excluded, id)
                in storageProvider.Storage?.SelectAccounts())
            {
                Accounts.Add(new Account
                {
                    Name = name,
                    Type = type,
                    Balance = balance,
                    Closed = closed,
                    Excluded = excluded,
                    Id = id
                });
            }
        }
        public bool AddAccount(string accName, out IAccount newAccount)
        {
            int id = -1;
            string accType = AccountTypes.First();
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
                Accounts.Add(newAccount);
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
            if (storageProvider.Storage?.DeleteAccount(account.Id) ?? false)
            {
                Accounts.Remove(account);
                return true;
            }
            return false;
        }
        private void SetCategories()
        {
            foreach(var (name, id) in storageProvider.Storage?.SelectTopCategories())
            {
                Category topCat = new Category
                {
                    Id = id,
                    Name = name,
                    Parent = null
                };
                foreach(var cat in storageProvider.Storage?.SelectSubCategories(id))
                {
                    Category subCat = new Category
                    {
                        Id = cat.id,
                        Name = cat.name,
                        Parent = topCat
                    };
                    topCat.AddChild(subCat);
                }
                Categories.Add(topCat);
            }
        }
        public bool AddCategory(string name, ICategory parent, out ICategory newCategory)
        {
            int id = -1;
            if (parent == null)
            {
                if (storageProvider.Storage?.AddTopCategory(name, out id) ?? false)
                {
                    newCategory = new Category
                    {
                        // id won't be -1 here, AddTopCategory will return valid id
                        Id = id,
                        Name = name,
                        Parent = null
                    };
                    Categories.Add(newCategory);
                    return true;
                }
                newCategory = null;
                return false;
            }
            else
            {
                if (storageProvider.Storage?.AddSubCategory(name, parent.Id, out id) ?? false)
                {
                    newCategory = new Category
                    {
                        // id won't be -1 here, AddSubCategory will return valid id
                        Id = id,
                        Name = name,
                        Parent = parent
                    };
                    ((Category)parent).AddChild(newCategory);
                    return true;
                }
                newCategory = null;
                return false;
            }
        }
        public bool DeleteCategory(ICategory category)
        {
            if (category.Parent == null)
            {
                if(storageProvider.Storage?.DeleteTopCategory(category.Id) ?? false)
                {
                    Categories.Remove(category);
                    return true;
                }
                return false;
            }
            else
            {
                if(storageProvider.Storage?.DeleteSubCategory(category.Id) ?? false)
                {
                    ((Category)category.Parent).RemoveChild(category);
                    return true;
                }
                return false;
            }
        }
        public IEnumerable<ITransaction> GetTransactions(IAccount account)
        {
            foreach (var (date, amount, info, categoryId, id) in storageProvider.Storage?.SelectTransactions(account.Id))
            {
                yield return new Transaction
                {
                    Account = account,
                    Amount = amount,
                    Category = (from topCat in Categories
                                from cat in topCat.Children
                                where cat.Id == categoryId select cat).First(),
                    Date = date,
                    Info = info,
                    Id = id
                };
            }
        }
        public bool DeleteTransaction(ITransaction transaction)
        {
            return (storageProvider.Storage?.DeleteTransaction(transaction.Id) ?? false);
        }
        public bool AddTransaction(IAccount account, DateTime date, decimal amount, string info, ICategory category, out ITransaction newTransaction)
        {
            int id = -1;
            if (storageProvider.Storage?.AddTransaction(account.Id, date, amount, info, category.Id, out id) ?? false)
            {
                newTransaction = new Transaction
                {
                    // Id will be valid here
                    Id = id,
                    Account = account,
                    Amount = amount,
                    Category = category,
                    Date = date,
                    Info = info
                };
                return true;
            }
            else
            {
                newTransaction = null;
                return false;
            }
        }
        public bool UpdateTransaction(ITransaction transaction, DateTime date, decimal amount, string info, ICategory category)
        {
            return storageProvider.Storage?.UpdateTransaction(transaction.Id, date, amount, info, category.Id) ?? false;
        }

        // TODO Remove
        public override IEnumerable<ICategory> GetCategories()
        {
            return Categories;
        }
    }

    public abstract class StubDataProvider
    {
        public abstract IEnumerable<ICategory> GetCategories();

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

        public IEnumerable<IBudgetRecord> CopyRecords(int fromMonth, int fromYear, int toMonth, int toYear)
        {
            return GetRecords(fromMonth, fromYear);
        }

        

        

        public void DeleteRecord(IBudgetRecord record)
        {
            // Do nothing
        }


        public (int minYear, int maxYear) GetActiveBudgetYears()
        {
            return (2013, 2018);
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

        public void UpdateRecord(IBudgetRecord record, decimal amount, ICategory category, BudgetType budgetType, int onDay, int month, int year)
        {
            // do nothing
        }
    }
}
