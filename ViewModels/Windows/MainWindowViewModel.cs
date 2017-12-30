using Models.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Windows
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
        public ObservableCollection<IAccountItem> Accounts { get; }
        // Commands
        public ICommand CreateFile { get; private set; }
        public ICommand OpenFile { get; private set; }
        public ICommand CloseFile { get; private set; }
        public ICommand Exit { get; private set; }
        public ICommand ManageAccTypes { get; private set; }
        public ICommand ManageAccounts { get; private set; }
        // ctor
        public MainWindowViewModel(IUIMainWindowService windowService, IFileHandler fileHandler, IDataProvider dataProvider)
        {
            this.windowService = windowService;
            this.fileHandler = fileHandler;
            this.dataProvider = dataProvider;

            Accounts = new ObservableCollection<IAccountItem>();

            CreateCommands();

            LoadLastOpenedFile();
        }
        // Methods
        private void CreateCommands()
        {
            CreateFile = new DelegateCommand(_CreateFile);
            OpenFile = new DelegateCommand(_OpenFile);
            CloseFile = new DelegateCommand(_CloseFile, IsFileOpened)
                .ObservesProperty(() => OpenedFile);
            Exit = new DelegateCommand(windowService.Shutdown);
            ManageAccTypes = new DelegateCommand(windowService.ManageAccountTypes, IsFileOpened)
                .ObservesProperty(() => OpenedFile);
            ManageAccounts = new DelegateCommand(windowService.ManageAccounts, () =>
            {
                // TODO Core.Instance.AccountTypes.Count != 0 && !string.IsNullOrEmpty(OpenedFile)
                // TODO observes
                return true;
            });
        }
        private bool IsFileOpened()
        {
            return !string.IsNullOrEmpty(OpenedFile)
        }
        private void CleanUpData()
        {
            Accounts.Clear();
        }
        private void LoadUpData()
        {
            foreach(IAccount acc in dataProvider.GetAccounts())
            {
                if (!acc.Closed)
                {
                    Accounts.Add(new AccountItem(acc, dataProvider));
                }
            }
            // show total
            if (Accounts.Count > 0)
            {
                AccountAggregate total = new AccountAggregate
                {
                    Name = "Total",
                    Balance = Accounts.Select(acc => acc.Balance).Sum(),
                };
                Accounts.Add(total);
            }
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
                    // Do nothing, file is empty
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
                    // Load new data
                    LoadUpData();
                    SaveLastOpenedFile(fileName);
                }
                else
                {
                    windowService.ShowMessage("Can't open file.");
                }
            }
        }
        private void _CloseFile()
        {
            // Release used fileHandler
            fileHandler.CloseFile();
            // Clean data
            CleanUpData();
            SaveLastOpenedFile(string.Empty);
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
                    // Load new data
                    LoadUpData();
                    OpenedFile = fileName;
                }
            }
        }
    }
}
