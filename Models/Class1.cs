using System.Collections.Generic;
using Models.Interfaces;

namespace Models
{
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

        public bool AddCategory(string name, ICategory parent, out ICategory newCategory)
        {
            newCategory = new StubCategory { Name = name, Parent = parent, Children = new List<ICategory>() };
            return true;
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

        public IEnumerable<ICategory> GetCategories()
        {
            StubCategory one = new StubCategory { Name = "one", Parent = null, Children = new List<ICategory>() };
            StubCategory topOne = new StubCategory { Name = "Top one", Parent = null,
                Children = new List<ICategory>() { one } };
            one.Parent = topOne;
            yield return topOne;
        }

        public void UpdateAccount(IAccount account)
        {
            // do nothing
        }
    }
}
