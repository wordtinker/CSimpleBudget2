using Models.Interfaces;
using System;
using System.Collections.ObjectModel;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    public class TransactionRollViewModel
    {
        private IDataProvider dataProvider;
        private IUITransactionRollService service;
        private IAccount account;

        public ObservableCollection<TransactionItem> Transactions { get; }

        public TransactionRollViewModel(AccountItem accItem, IDataProvider dataProvider, IUITransactionRollService service)
        {
            this.dataProvider = dataProvider;
            this.service = service;
            this.account = accItem.account;
            Transactions = new ObservableCollection<TransactionItem>();

            foreach (var item in dataProvider.GetTransactions(account))
            {
                Transactions.Add(new TransactionItem(item));
            }
        }

        public void DeleteTransaction(TransactionItem item)
        {
            if (dataProvider.DeleteTransaction(item.transaction))
            {
                Transactions.Remove(item);
                // TODO event ?
            }
            else
            {
                service.ShowMessage("Can't delete transaction.");
            }
        }
    }
}
