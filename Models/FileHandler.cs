using Data.Interfaces;
using Models.Interfaces;
using System;
using System.IO;

namespace Models
{
    public class FileHandler : IFileHandler, IStorageProvider
    {
        private Func<string, IStorage> getStorage;

        public event EventHandler On;
        public event EventHandler Off;

        // TODO does it know it?
        public string Extension => ".sbdb";
        public IStorage Storage { get; private set; }
        public bool InitializeFile(string fileName)
        {
            // Delete file in order to replace it with newly created one
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            catch (Exception)
            {
                return false;
            }
            // Initialize new file
            using (IStorage storage = getStorage(fileName))
            {
                return storage.Initialize();
            }
        }
        public bool LoadFile(string fileName)
        {
            if (!File.Exists(fileName)) return false;
            try
            {
                // close previously opened file
                CloseFile();
                // Get new storage connection
                Storage = getStorage(fileName);
                On?.Invoke(this, null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void CloseFile()
        {
            Storage?.Dispose();
            Storage = null;
            Off?.Invoke(this, null);
        }
        public FileHandler(Func<string, IStorage> getStorage)
        {
            this.getStorage = getStorage;       
        }
    }
}
