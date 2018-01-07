using Models.Interfaces;
using System;

namespace ViewModels.Elements
{
    /// <summary>
    /// Container for Transaction item.
    /// </summary>
    public class TransactionItem
    {
        internal ITransaction transaction;

        public DateTime Date
        {
            get { return transaction.Date; }
        }

        public string Value
        {
            get { return string.Format("{0:0.00}", transaction.Amount); }
        }

        public string Info
        {
            get { return transaction.Info; }
        }

        public string Category
        {
            get
            {
                ICategory category = transaction.Category;
                return string.Format("{0}--{1}", category.Parent.Name, category.Name);
            }
        }

        public string Account
        {
            get
            {
                return transaction.Account.Name;
            }
        }

        public TransactionItem(ITransaction tr)
        {
            this.transaction = tr;
        }
    }
}
