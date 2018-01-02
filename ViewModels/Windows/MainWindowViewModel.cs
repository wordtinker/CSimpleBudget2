using Models.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ViewModels.Elements;
using ViewModels.Events;
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
        private IEventAggregator eventAggregator;

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
                    // TODO ???
                    RaisePropertyChanged(nameof(CanShowReport));
                }
            }
        }
        public bool IsReadyToSetAccounts
        {
            get => AccTypes.Count > 0;
        }
        public ObservableCollection<IAccountItem> Accounts { get; }
        public ObservableCollection<AccTypeItem> AccTypes { get; }
        // Commands
        public ICommand CreateFile { get; private set; }
        public ICommand OpenFile { get; private set; }
        public ICommand CloseFile { get; private set; }
        public ICommand Exit { get; private set; }
        public ICommand ManageAccTypes { get; private set; }
        public ICommand ManageAccounts { get; private set; }
        // ctor
        public MainWindowViewModel(IUIMainWindowService windowService, IFileHandler fileHandler, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.windowService = windowService;
            this.fileHandler = fileHandler;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;

            Accounts = new ObservableCollection<IAccountItem>();
            AccTypes = new ObservableCollection<AccTypeItem>();
            AccTypes.CollectionChanged += (sender, e) => RaisePropertyChanged(nameof(IsReadyToSetAccounts));

            CreateCommands();
            ConnectEvents();
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
            ManageAccounts = new DelegateCommand(windowService.ManageAccounts)
                .ObservesCanExecute(() => IsReadyToSetAccounts);
        }
        private void ConnectEvents()
        {
            eventAggregator.GetEvent<AccountTypeAdded>().Subscribe(ati => AccTypes.Add(ati));
            eventAggregator.GetEvent<AccountTypeDeleted>().Subscribe(ati => AccTypes.Remove(ati));
            // Those events are rare, it's easier to refresh whole collection
            eventAggregator.GetEvent<AccountAdded>().Subscribe(a => RefreshAccounts());
            eventAggregator.GetEvent<AccountDeleted>().Subscribe(a => RefreshAccounts());
            eventAggregator.GetEvent<AccountChanged>().Subscribe(a => RefreshAccounts());
        }
        private bool IsFileOpened()
        {
            return !string.IsNullOrEmpty(OpenedFile);
        }
        // TODO
        private void CleanUpData()
        {
            AccTypes.Clear();
            Accounts.Clear();
        }
        // TODO
        private void LoadUpData()
        {
            foreach (var item in dataProvider.GetAccountTypes())
            {
                AccTypes.Add(new AccTypeItem(item));
            }
            RefreshAccounts();
        }
        private void RefreshAccounts()
        {
            Accounts.Clear();
            foreach (IAccount acc in dataProvider.GetAccounts())
            {
                if (!acc.Closed)
                {
                    Accounts.Add(new AccountItem(acc, dataProvider, eventAggregator));
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
