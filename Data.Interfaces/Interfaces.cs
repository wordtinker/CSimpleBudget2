using System;
using System.Collections.Generic;

namespace Data.Interfaces
{
    // TODO
    public interface IStorage : IDisposable
    {
        bool Initialize();

        IEnumerable<string> SelectAccTypes();
    }
}
