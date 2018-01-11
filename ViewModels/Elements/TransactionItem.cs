using Models.Interfaces;
using Prism.Mvvm;
using System;

namespace ViewModels.Elements
{
    /// <summary>
    /// Container for Transaction item.
    /// </summary>
    public class TransactionItem : BindableBase
    {
        internal ITransaction transaction;
        private DateTime date;
        private decimal amount;
        private string info;
        private CategoryNode catNode;

        public DateTime Date { get => date; set => SetProperty(ref date, value); }
        public decimal Amount { get => amount; set => SetProperty(ref amount, value); }
        public string Info { get => info; set => SetProperty(ref info, value); }
        public CategoryNode Category { get => catNode; set => SetProperty(ref catNode, value); }

        // TODO for reports
        //public string Account
        //{
        //    get
        //    {
        //        return transaction.Account.Name;
        //    }
        //}

        public TransactionItem(ITransaction transaction)
        {
            Date = transaction.Date;
            Amount = transaction.Amount;
            Info = transaction.Info;
            Category = new CategoryNode(transaction.Category);
            this.transaction = transaction;
        }
    }
}
