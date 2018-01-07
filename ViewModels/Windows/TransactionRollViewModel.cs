using Models.Interfaces;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    public class TransactionRollViewModel
    {
        private IUITransactionRollService service;
        private IAccount account;
        
        public TransactionRollViewModel(AccountItem accItem, IUITransactionRollService service)
        {
            this.service = service;
            this.account = accItem.account;
            // TODO !!!
            //Core.Instance.GetTransactions(account).ForEach((tr) =>
            //{
            //    Transactions.Add(new TransactionItem(tr));
            //});
        }
    }
}
