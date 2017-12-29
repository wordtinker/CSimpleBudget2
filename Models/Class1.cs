using Models.Interfaces;

namespace Models
{
    
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
    }
}
