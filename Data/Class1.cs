
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
        // TODO ctor might throw exception
    }
}
