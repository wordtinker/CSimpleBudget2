using Models.Interfaces;
using System;

namespace ViewModels.Elements
{
    /// <summary>
    /// Container for Transaction item.
    /// </summary>
    public class TransactionItem
    {
        internal ITransaction tr;

        public DateTime Date
        {
            get { return tr.Date; }
        }

        public string Value
        {
            get { return string.Format("{0:0.00}", tr.Amount); }
        }

        public string Info
        {
            get { return tr.Info; }
        }

        public string Category
        {
            get
            {
                ICategory category = tr.Category;
                return string.Format("{0}--{1}", category.Parent.Name, category.Name);
            }
        }

        public string Account
        {
            get
            {
                return tr.Account.Name;
            }
        }

        public TransactionItem(ITransaction tr)
        {
            this.tr = tr;
        }
    }
}
