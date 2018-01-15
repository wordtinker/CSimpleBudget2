using Models.Interfaces;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ViewModels.Elements;
using ViewModels.Events;

namespace ViewModels.Windows
{
    public abstract class AbstractTransactionEditor
    {
        protected IEventAggregator eventAggregator;
        protected IDataProvider dataProvider;

        public IEnumerable<CategoryNode> Categories =>
            from topCat in dataProvider.Categories
            from c in topCat.Children
            select new CategoryNode(c);
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Info { get; set; }
        public CategoryNode Category { get; set; }

        public AbstractTransactionEditor(IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.dataProvider = dataProvider;
            Category = Categories.First();
        }
        public abstract void Save();
    }

    public class NewTransactionEditorViewModel : AbstractTransactionEditor
    {
        private AccountItem accountItem;

        public NewTransactionEditorViewModel(AccountItem accountItem, IDataProvider dataProvider, IEventAggregator eventAggregator)
            : base(dataProvider, eventAggregator)
        {
            this.accountItem = accountItem;
            Date = DateTime.Now;
        }

        public override void Save()
        {
            // Create new transaction.
            if (dataProvider.AddTransaction(accountItem.account, Date, Amount, Info, Category.category, out ITransaction newTr))
            {
                eventAggregator.GetEvent<TransactionAdded>().Publish(new TransactionItem(newTr));
            }
        }
    }
    public class OldTransactionEditorViewModel : AbstractTransactionEditor
    {
        private TransactionItem transactionItem;

        public OldTransactionEditorViewModel(TransactionItem transactionItem, IDataProvider dataProvider, IEventAggregator eventAggregator)
            : base(dataProvider, eventAggregator)
        {
            this.transactionItem = transactionItem;
            Date = transactionItem.Date;
            Amount = transactionItem.Amount;
            Info = transactionItem.Info;
            Category = transactionItem.Category;
        }

        public override void Save()
        {
            // Update transaction in the model
            if (dataProvider.UpdateTransaction(transactionItem.transaction, Date, Amount, Info, Category.category))
            {
                // And in the viewModel
                transactionItem.Date = Date;
                transactionItem.Amount = Amount;
                transactionItem.Info = Info;
                transactionItem.Category = Category;
                eventAggregator.GetEvent<TransactionChanged>().Publish(transactionItem);
            }
        }
    }
}
