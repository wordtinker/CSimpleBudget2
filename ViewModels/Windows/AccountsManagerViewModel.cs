using Models.Interfaces;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    public class AccountsManagerViewModel
    {
        private IUIBaseService serviceProvider;
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;

        public ObservableCollection<AccountItem> Accounts { get; }
        public IEnumerable<string> AccTypes => from t in dataProvider.AccountTypes select t;
        public ICommand DeleteAccount { get; }

        public AccountsManagerViewModel(IUIBaseService serviceProvider, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.serviceProvider = serviceProvider;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
            Accounts = new ObservableCollection<AccountItem>();
            DeleteAccount = new DelegateCommand<object>(_DeleteAccount);
            foreach (var item in dataProvider.Accounts)
            {
                Accounts.Add(new AccountItem(item, dataProvider, eventAggregator));
            }
        }
        public void AddAccount(string accName)
        {
            if (dataProvider.AddAccount(accName, out IAccount newAccount))
            {
                AccountItem newAccVM = new AccountItem(newAccount, dataProvider, eventAggregator);
                Accounts.Add(newAccVM);
            }
            else
            {
                serviceProvider.ShowMessage("Can't add account.");
            }
        }

        private void _DeleteAccount(object parameter)
        {
            if (parameter is AccountItem item)
            {
                if (dataProvider.DeleteAccount(item.account))
                {
                    Accounts.Remove(item);
                }
                else
                {
                    serviceProvider.ShowMessage("Can't delete account.");
                }
            }
        }
    }
}
