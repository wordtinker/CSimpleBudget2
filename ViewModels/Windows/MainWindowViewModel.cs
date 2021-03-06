﻿using Models.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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
        public string OpenedFile
        {
            get { return openedFile; }
            set
            {
                if (SetProperty(ref openedFile, value))
                {
                    RaisePropertyChanged(nameof(IsFileOpened));
                }
            }
        }
        public bool IsFileOpened => !string.IsNullOrEmpty(OpenedFile);
        public bool IsReadyToSetAccounts => dataProvider.AccountTypes.Any();
        /// <summary>
        /// Property showing that we have enough
        /// info for reports and budgeting.
        /// </summary>
        public bool IsFullyReady => (from c in dataProvider.Categories where c.Children.Any() select c).Any();
        public IEnumerable<IAccountItem> Accounts
        {
            get
            {
                int count = 0;
                decimal sum = 0;
                foreach (IAccount acc in dataProvider.Accounts.Where(acc => !acc.Closed))
                {
                    count++;
                    sum += acc.Balance;
                    yield return new AccountItem(acc, dataProvider, eventAggregator);
                }
                // show total
                if (count > 0)
                {
                    yield return new AccountAggregate
                    {
                        Name = "Total",
                        Balance = sum
                    };
                }
            }
        }
        public ObservableCollection<BudgetBar> Bars { get; }
        public int CurrentMonth { get; }
        public int CurrentYear { get; }
        // Commands
        public ICommand CreateFile { get; private set; }
        public ICommand OpenFile { get; private set; }
        public ICommand CloseFile { get; private set; }
        public ICommand Exit { get; private set; }
        public ICommand ManageAccTypes { get; private set; }
        public ICommand ManageAccounts { get; private set; }
        public ICommand ManageCategories { get; private set; }
        public ICommand ManageBudget { get; private set; }
        public ICommand ShowBudgetReport { get; private set; }
        public ICommand ShowBalanceReport { get; private set; }
        public ICommand ShowCategoriesReport { get; private set; }
        public ICommand ShowHelp { get; private set; }
        // ctor
        public MainWindowViewModel(IUIMainWindowService windowService, IFileHandler fileHandler, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.windowService = windowService;
            this.fileHandler = fileHandler;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;

            Bars = new ObservableCollection<BudgetBar>();
            CurrentMonth = DateTime.Now.Month;
            CurrentYear = DateTime.Now.Year;

            CreateCommands();
            ConnectEvents();
            LoadLastOpenedFile();
        }
        // Methods
        private void CreateCommands()
        {
            CreateFile = new DelegateCommand(_CreateFile);
            OpenFile = new DelegateCommand(_OpenFile);
            CloseFile = new DelegateCommand(_CloseFile)
                .ObservesCanExecute(() => IsFileOpened);
            Exit = new DelegateCommand(windowService.Shutdown);
            ManageAccTypes = new DelegateCommand(windowService.ManageAccountTypes)
                .ObservesCanExecute(() => IsFileOpened);
            ManageAccounts = new DelegateCommand(windowService.ManageAccounts)
                .ObservesCanExecute(() => IsReadyToSetAccounts);
            ManageCategories = new DelegateCommand(windowService.ManageCategories)
                .ObservesCanExecute(() => IsFileOpened);
            ManageBudget = new DelegateCommand(windowService.ManageBudget)
                .ObservesCanExecute(() => IsFullyReady);
            ShowBudgetReport = new DelegateCommand(windowService.ShowBudgetReport)
                .ObservesCanExecute(() => IsFullyReady);
            ShowBalanceReport = new DelegateCommand(windowService.ShowBalanceReport)
                .ObservesCanExecute(() => IsFullyReady);
            ShowCategoriesReport = new DelegateCommand(windowService.ShowCategoriesReport)
                .ObservesCanExecute(() => IsFullyReady);
            ShowHelp = new DelegateCommand(windowService.ShowHelp);
        }
        private void ConnectEvents()
        {
            dataProvider.AccountTypes.CollectionChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(IsReadyToSetAccounts));
            };
            dataProvider.Accounts.CollectionChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(Accounts));
                RefreshBars();
            };
            dataProvider.Categories.CollectionChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(IsFullyReady));
            };
            // Connect transaction creation, deletion, change
            eventAggregator.GetEvent<TransactionAdded>().Subscribe(tri => RaisePropertyChanged(nameof(Accounts)));
            eventAggregator.GetEvent<TransactionDeleted>().Subscribe(tri => RaisePropertyChanged(nameof(Accounts)));
            eventAggregator.GetEvent<TransactionChanged>().Subscribe(tri => RaisePropertyChanged(nameof(Accounts)));
            eventAggregator.GetEvent<TransactionAdded>().Subscribe(
                tri => RefreshBars(), ThreadOption.PublisherThread, false,
                tri => tri.Date.Month == CurrentMonth && tri.Date.Year == CurrentYear);
            eventAggregator.GetEvent<TransactionDeleted>().Subscribe(
                tri => RefreshBars(), ThreadOption.PublisherThread, false,
                tri => tri.Date.Month == CurrentMonth && tri.Date.Year == CurrentYear);
            eventAggregator.GetEvent<TransactionChanged>().Subscribe(
                trch => RefreshBars(), ThreadOption.PublisherThread, false,
                trch => trch.New.Date.Month == CurrentMonth && trch.New.Date.Year == CurrentYear ||
                        trch.Old.Date.Month == CurrentMonth && trch.Old.Date.Year == CurrentYear);
            // Сonnect budget record creation, deletion, change
            eventAggregator.GetEvent<BudgetRecordAdded>().Subscribe(
                ri => RefreshBars(), ThreadOption.PublisherThread, false,
                ri => ri.Month == CurrentMonth && ri.Year == CurrentYear);
            eventAggregator.GetEvent<BudgetRecordDeleted>().Subscribe(
                ri => RefreshBars(), ThreadOption.PublisherThread, false,
                ri => ri.Month == CurrentMonth && ri.Year == CurrentYear);
            eventAggregator.GetEvent<BudgetRecordChanged>().Subscribe(
                rich => RefreshBars(), ThreadOption.PublisherThread, false,
                rich => rich.New.Month == CurrentMonth && rich.New.Year == CurrentYear ||
                        rich.Old.Month == CurrentMonth && rich.Old.Year == CurrentYear);
        }
        private void CleanUpData()
        {
            RaisePropertyChanged(nameof(Accounts));
            Bars.Clear();
        }
        private void LoadUpData()
        {
            RaisePropertyChanged(nameof(Accounts));
            RefreshBars();
        }
        private void RefreshBars()
        {
            Bars.Clear();
            foreach (var spending in dataProvider.GetSpendings(CurrentYear, CurrentMonth))
            {
                Bars.Add(new BudgetBar(spending));
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
        public void ShowTransactionRoll(IAccountItem item)
        {
            if (item is AccountItem account)
            {
                if (IsFullyReady)
                {
                    windowService.ShowTransactionRoll(account);
                }
                else
                {
                    windowService.ShowMessage("Set categories first!");
                }
            }
        }
    }
}
