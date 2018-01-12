
using System;
using System.Collections.Generic;
using Data.Interfaces;

namespace Data
{
    public class StubStorage : IStorage
    {
        public bool AddAccount(string name, string type, out int id)
        {
            id = DateTime.Now.Millisecond;
            return true;
        }

        public bool AddAccountType(string name)
        {
            return true;
        }

        public bool DeleteAccount(int id)
        {
            return true;
        }

        public bool DeleteAccountType(string name)
        {
            return true;
        }

        public void Dispose()
        {
            // TODO using pattern
        }

        public bool Initialize()
        {
            return true;
        }

        public IEnumerable<(string name, string type, decimal balance, bool closed, bool excluded, int id)> SelectAccounts()
        {
            yield return ("1254", "one", 1254m, false, false, 1);
            yield return ("1254", "two", 1254m, true, true, 2);
            yield return ("8745", "one", 8745m, false, false, 3);
        }

        public IEnumerable<string> SelectAccTypes()
        {
            yield return "one";
            yield return "two";
            yield return "2222";
        }

        public bool UpdateAccount(int id, string type, decimal balance, bool closed, bool excluded)
        {
            return true;
        }
        // TODO ctor might throw exception
    }
}
