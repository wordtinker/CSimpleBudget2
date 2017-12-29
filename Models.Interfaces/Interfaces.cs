
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

    }
}
