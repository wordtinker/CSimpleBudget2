﻿
using System.Collections.Generic;
using Data.Interfaces;

namespace Data
{
    public class StubStorage : IStorage
    {
        public void Dispose()
        {
            // TODO using pattern
        }

        public bool Initialize()
        {
            return true;
        }

        public IEnumerable<string> SelectAccTypes()
        {
            yield return "one";
            yield return "two";
            yield return "2222";
        }
        // TODO ctor might throw exception
    }
}