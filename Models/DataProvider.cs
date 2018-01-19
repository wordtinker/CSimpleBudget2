using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Models.Elements;
using Models.Interfaces;

namespace Models
{
    public class DataProvider : IDataProvider
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
            // TODO refactor private method to get First()
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
            foreach (string accType in storageProvider.Storage?.SelectAccountTypes())
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
                foreach(var cat in storageProvider.Storage?.SelectSubCategories(name))
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
                if (storageProvider.Storage?.AddSubCategory(name, parent.Name, out id) ?? false)
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
                if(storageProvider.Storage?.DeleteTopCategory(category.Name) ?? false)
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
        public IEnumerable<ITransaction> GetTransactions(int year, int month, ICategory category)
        {
            foreach (var (date, amount, info, categoryId, accountId, id) in storageProvider.Storage?.SelectTransactions(year, month, category.Id))
            {
                yield return new Transaction
                {
                    Account = Accounts.Where(acc => acc.Id == accountId).First(),
                    Amount = amount,
                    Category = (from topCat in Categories
                                from cat in topCat.Children
                                where cat.Id == categoryId
                                select cat).First(),
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
        /// Provides a pair of years during which a budget records were set.
        /// Default is current year.
        public (int minYear, int maxYear) GetActiveBudgetYears()
        {
            int currentYear = DateTime.Today.Year;
            // Ensure that we set current year if we have no record of year in storage
            int minYear = storageProvider.Storage?.GetMinimumYear() ?? currentYear;
            int maxYear = storageProvider.Storage?.GetMaximumYear() ?? currentYear;
            // Set current year if budget year is way behind
            maxYear = Math.Max(maxYear, currentYear);
            return (minYear, maxYear);
        }
        public IEnumerable<IBudgetRecord> GetRecords(int year, int month)
        {
            foreach (var (amount, categoryId, type, onDay, id) in storageProvider.Storage?.SelectRecords(year, month))
            {
                Enum.TryParse(type, out BudgetType budgetType);
                yield return new BudgetRecord
                {
                    Id = id,
                    Amount = amount,
                    Category = (from topCat in Categories
                                from cat in topCat.Children
                                where cat.Id == categoryId
                                select cat).First(),
                    Year = year,
                    Month = month,
                    OnDay = onDay,
                    Type = budgetType
                };
            }            
        }
        public IEnumerable<IBudgetRecord> CopyRecords(int fromMonth, int fromYear, int toMonth, int toYear)
        {
            foreach (var (amount, categoryId, type, onDay, id) in storageProvider.Storage?.SelectRecords(fromYear, fromMonth))
            {
                var category = (from topCat in Categories
                            from cat in topCat.Children
                            where cat.Id == categoryId
                            select cat).First();
                Enum.TryParse(type, out BudgetType budgetType);
                if (AddBudgetRecord(amount, category, budgetType, onDay, toYear, toMonth, out IBudgetRecord newRecord))
                {
                    yield return newRecord;
                }
            }
        }
        public bool AddBudgetRecord(decimal amount, ICategory category, BudgetType budgetType, int onDay, int year, int month, out IBudgetRecord newRecord)
        {
            int id = -1;
            if (storageProvider.Storage?.AddRecord(amount, category.Id, budgetType.ToString(), onDay, year, month, out id) ?? false)
            {
                newRecord = new BudgetRecord
                {
                    // Id will be valid here
                    Id = id,
                    Amount = amount,
                    Category = category,
                    Month = month,
                    Year = year,
                    OnDay = onDay,
                    Type = budgetType
                };
                return true;
            }
            else
            {
                newRecord = null;
                return false;
            }
        }
        public bool DeleteRecord(IBudgetRecord record)
        {
            return storageProvider.Storage?.DeleteRecord(record.Id) ?? false;
        }
        public bool UpdateRecord(IBudgetRecord record, decimal amount, ICategory category, BudgetType budgetType, int onDay, int month, int year)
        {
            return storageProvider.Storage?.UpdateRecord(record.Id, amount, category.Id, budgetType.ToString(), onDay, year, month) ?? false;
        }
        public IEnumerable<ISpending> GetSpendings(int year, int month)
        {
            var subcats = from cat in Categories from c in cat.Children select c;
            foreach (ICategory cat in subcats)
            {
                // should be positive
                decimal budget = Math.Abs(storageProvider.Storage?.SelectRecordsCombined(year, month, cat.Id) ?? decimal.Zero);
                decimal spent = Math.Abs(storageProvider.Storage?.SelectTransactionsCombined(year, month, cat.Id) ?? decimal.Zero);
                if (budget == decimal.Zero && spent == decimal.Zero)
                {
                    continue;
                }
                yield return new Spending
                {
                    Category = cat,
                    Month = month,
                    Budget = budget,
                    Value = spent
                };
            }
        }
    }
}
