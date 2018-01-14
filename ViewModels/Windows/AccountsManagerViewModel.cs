using Models.Interfaces;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    // TOTO Later Validation?
    public class AccountsManagerViewModel
    {
        private IUIBaseService serviceProvider;
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;

        public ObservableCollection<AccountItem> Accounts { get; }
        public IEnumerable<string> AccTypes => from t in dataProvider.AccountTypes select t;

        public AccountsManagerViewModel(IUIBaseService serviceProvider, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.serviceProvider = serviceProvider;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
            Accounts = new ObservableCollection<AccountItem>();
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

        public void DeleteAccount(AccountItem item)
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
