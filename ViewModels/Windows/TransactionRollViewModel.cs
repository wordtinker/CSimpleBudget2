using Models.Interfaces;
using Prism.Events;
using System.Collections.ObjectModel;
using ViewModels.Elements;
using ViewModels.Events;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    public class TransactionRollViewModel
    {
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;
        private IUITransactionRollService service;
        private AccountItem accountItem;

        public ObservableCollection<TransactionItem> Transactions { get; }

        public TransactionRollViewModel(AccountItem accItem, IDataProvider dataProvider, IUITransactionRollService service, IEventAggregator eventAggregator)
        {
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
            this.service = service;
            this.accountItem = accItem;
            Transactions = new ObservableCollection<TransactionItem>();

            foreach (var item in dataProvider.GetTransactions(accountItem.account))
            {
                Transactions.Add(new TransactionItem(item));
            }
            eventAggregator.GetEvent<TransactionAdded>().Subscribe(tri => Transactions.Add(tri));
        }

        public void DeleteTransaction(TransactionItem item)
        {
            dataProvider.DeleteTransaction(item.transaction);
            Transactions.Remove(item);
            eventAggregator.GetEvent<TransactionDeleted>().Publish(item);
        }
        public void ShowTransactionEditor()
        {
            service.ShowTransactionEditor(accountItem);
        }
        public void ShowTransactionEditor(TransactionItem item)
        {
            service.ShowTransactionEditor(item);
        }
    }
}
