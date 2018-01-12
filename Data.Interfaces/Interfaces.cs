using System;
using System.Collections.Generic;

namespace Data.Interfaces
{
    // TODO
    public interface IStorage : IDisposable
    {
        bool Initialize();

        IEnumerable<string> SelectAccTypes();
        bool AddAccountType(string name);
        bool DeleteAccountType(string name);

        IEnumerable<(string name, string type, decimal balance, bool closed, bool excluded, int id)> SelectAccounts();
        bool AddAccount(string name, string type, out int id);
        bool UpdateAccount(int id, string type, decimal balance, bool closed, bool excluded);
        bool DeleteAccount(int id);

    }
}
