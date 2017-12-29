using System.Collections.Generic;
using Models.Interfaces;

namespace Models
{
    public class StubAccount : IAccount
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public bool Closed { get; set; }
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

        public IEnumerable<IAccount> GetAccounts()
        {
            yield return new StubAccount
            {
                Balance = 1254m,
                Closed = false,
                Name = "1254"
            };
            yield return new StubAccount
            {
                Balance = 1254m,
                Closed = true,
                Name = "1254"
            };
            yield return new StubAccount
            {
                Balance = 8745m,
                Closed = false,
                Name = "8745"
            };
        }
    }
}
