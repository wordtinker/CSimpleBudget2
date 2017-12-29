
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
        IEnumerable<IAccount> GetAccounts();
    }
    // TODO
    public interface IAccount
    {
        string Name { get; }
        decimal Balance { get; }
        bool Closed { get; }
    }
}
