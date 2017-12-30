
using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IFileHandler
    {
        string Extension { get; }
        bool InitializeFile(string fileName);
        bool LoadFile(string fileName);
        void CloseFile();
    }
    public interface IStorageProvider
    {
        // TODO IStorage
    }
    public interface IDataProvider
    {
        IEnumerable<string> GetAccountTypes();
        bool AddAccountType(string accountType);
        bool DeleteAccountType(string accountType);
        IEnumerable<IAccount> GetAccounts();
        bool AddAccount(string accTyp, string accName, out IAccount newAccount);
        bool DeleteAccount(IAccount account);
        void UpdateAccount(IAccount account);
    }
    public interface IAccount
    {
        string Name { get; set; }
        string Type { get; set; }
        decimal Balance { get; set; }
        bool Closed { get; set; }
        bool Excluded { get; set; }
    }
}
