using Models.Interfaces;
using Prism.Mvvm;
using System;
using ViewModels.Interfaces;

namespace ViewModels.Elements
{
    /// <summary>
    /// Container for Transaction item.
    /// </summary>
    public class TransactionItem : BindableBase, ITransactionItem
    {
        private DateTime date;
        private decimal amount;
        private string info;
        private ICategoryNode catNode;

        public ITransaction InnerTransaction { get; private set; }

        public DateTime Date { get => date; set => SetProperty(ref date, value); }
        public decimal Amount { get => amount; set => SetProperty(ref amount, value); }
        public string Info { get => info; set => SetProperty(ref info, value); }
        public ICategoryNode Category { get => catNode; set => SetProperty(ref catNode, value); }
        public string Account => InnerTransaction.Account.Name;

        public TransactionItem(ITransaction transaction)
        {
            Date = transaction.Date;
            Amount = transaction.Amount;
            Info = transaction.Info;
            Category = new CategoryNode(transaction.Category);
            InnerTransaction = transaction;
        }
    }
}
