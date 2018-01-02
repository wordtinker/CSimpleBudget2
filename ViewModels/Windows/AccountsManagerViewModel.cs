using Models.Interfaces;
using Prism.Events;
using System.Collections.ObjectModel;
using ViewModels.Elements;
using ViewModels.Events;
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
        public ObservableCollection<string> AccTypes { get; }

        public AccountsManagerViewModel(IUIBaseService serviceProvider, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.serviceProvider = serviceProvider;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
            Accounts = new ObservableCollection<AccountItem>();
            AccTypes = new ObservableCollection<string>();
            foreach (var item in dataProvider.GetAccounts())
            {
                Accounts.Add(new AccountItem(item, dataProvider, eventAggregator));
            }
            foreach (var item in dataProvider.GetAccountTypes())
            {
                AccTypes.Add(item);
            }
        }
        public void AddAccount(string accName)
        {
            if (dataProvider.AddAccount(AccTypes[0], accName, out IAccount newAccount))
            {
                AccountItem newAccVM = new AccountItem(newAccount, dataProvider, eventAggregator);
                Accounts.Add(newAccVM);
                eventAggregator.GetEvent<AccountAdded>().Publish(newAccVM);
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
                eventAggregator.GetEvent<AccountDeleted>().Publish(item);
            }
            else
            {
                serviceProvider.ShowMessage("Can't delete account.");
            }
        }
    }
}
