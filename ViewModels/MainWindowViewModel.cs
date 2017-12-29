using Models.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using ViewModels.Interfaces;

namespace ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        // Members
        private string openedFile;
        private IUIMainWindowService windowService;
        private IFileHandler fileHandler;
        private IDataProvider dataProvider;

        // Properties

        /// <summary>
        /// Property showing that we have enough
        /// info for reports.
        /// </summary>
        // TODO why do we need that?
        public bool CanShowReport
        {
            get
            {
                return false;
                // TODO stub
                //bool catExists = (from c in core.Categories
                //                  where c.Parent != null
                //                  select c).Any();
                //return !string.IsNullOrEmpty(OpenedFile) && catExists;
            }
        }
        public string OpenedFile
        {
            get { return openedFile; }
            set
            {
                if (SetProperty(ref openedFile, value))
                {
                    RaisePropertyChanged(nameof(CanShowReport));
                }
            }
        }
        // Commands
        public ICommand CreateFile { get; }
        public ICommand OpenFile { get; }
        public ICommand CloseFile { get; }
        public ICommand Exit { get; }
        // ctor
        public MainWindowViewModel(IUIMainWindowService windowService, IFileHandler fileHandler, IDataProvider dataProvider)
        {
            this.windowService = windowService;
            this.fileHandler = fileHandler;
            this.dataProvider = dataProvider;
            // TODO Test
            CreateFile = new DelegateCommand(_CreateFile);
            // TODO Test
            OpenFile = new DelegateCommand(_OpenFile);
            // TODO Test
            CloseFile = new DelegateCommand(_CloseFile, () => !string.IsNullOrEmpty(OpenedFile))
                .ObservesProperty(() => OpenedFile);
            Exit = new DelegateCommand(windowService.Shutdown);
            LoadLastOpenedFile();
        }
        private void _CreateFile()
        {
            string fileName = windowService.SaveFileDialog(fileHandler.Extension);
            if (fileName != null)
            {
                // Close the file if we have one opened
                _CloseFile();

                // Create new file
                if (fileHandler.InitializeFile(fileName) &&
                    fileHandler.LoadFile(fileName))
                {
                    SaveLastOpenedFile(fileName);

                    // TODO
                    //if (!core.InitializeNewFileReader(fileHandler))
                    //{
                    //    windowService.ShowMessage("File is corrupted.");
                    //    CloseFile.Execute(null);
                    //}
                }
                else
                {
                    windowService.ShowMessage("Can't create file.");
                }
            }
        }
        private void _OpenFile()
        {
            string fileName = windowService.OpenFileDialog(fileHandler.Extension);
            if (fileName != null)
            {
                // Close the file if we have one opened
                _CloseFile();
                // Open file
                if (fileHandler.LoadFile(fileName))
                {
                    SaveLastOpenedFile(fileName);
                    // TODO
                    //if (!core.InitializeNewFileReader(fileHandler))
                    //{
                    //    windowService.ShowMessage("File is corrupted.");
                    //    CloseFile.Execute(null);
                    //}
                }
                else
                {
                    windowService.ShowMessage("Can't open file.");
                }
            }
        }
        private void _CloseFile()
        {
            // TODO
            SaveLastOpenedFile(string.Empty);
            fileHandler.CloseFile();
            //core.InitializeNewFileReader(null);
        }
        private void SaveLastOpenedFile(string fileName)
        {
            OpenedFile = fileName;
            windowService.LastSavedFileName = fileName;
        }
        private void LoadLastOpenedFile()
        {
            string fileName = windowService.LastSavedFileName;
            if (fileName != string.Empty)
            {
                if (fileHandler.LoadFile(fileName))
                {
                    // TODO
                    // TODO refactor similar code _OpenFile
                    //if (!core.InitializeNewFileReader(fileHandler))
                    //{
                    //    windowService.ShowMessage("File is corrupted.");
                    //    CloseFile.Execute(null);
                    //}
                    OpenedFile = fileName;
                }
            }
        }
    }
}
