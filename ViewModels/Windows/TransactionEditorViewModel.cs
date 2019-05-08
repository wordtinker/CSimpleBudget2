using Models.Interfaces;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModels.Elements;
using ViewModels.Events;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    public abstract class AbstractTransactionEditor
    {
        protected IEventAggregator eventAggregator;
        protected IDataProvider dataProvider;

        public IEnumerable<ICategoryNode> Categories =>
            from topCat in dataProvider.Categories
            from c in topCat.Children
            select new CategoryNode(c);
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Info { get; set; }
        public ICategoryNode Category { get; set; }

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
            if (dataProvider.AddTransaction(accountItem.account, Date, Amount, Info, Category.InnerCategory, out ITransaction newTr))
            {
                eventAggregator.GetEvent<TransactionAdded>().Publish(new TransactionItem(newTr));
            }
        }
    }
    public class OldTransactionEditorViewModel : AbstractTransactionEditor
    {
        private ITransactionItem transactionItem;

        public OldTransactionEditorViewModel(ITransactionItem transactionItem, IDataProvider dataProvider, IEventAggregator eventAggregator)
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
            if (dataProvider.UpdateTransaction(transactionItem.InnerTransaction, Date, Amount, Info, Category.InnerCategory))
            {
                TransactionItem old = new TransactionItem(transactionItem.InnerTransaction);
                // And in the viewModel
                transactionItem.Date = Date;
                transactionItem.Amount = Amount;
                transactionItem.Info = Info;
                transactionItem.Category = Category;
                eventAggregator.GetEvent<TransactionChanged>().Publish(
                    new TransactionChange
                    {
                        Old = old,
                        New = transactionItem
                    });
            }
        }
    }
}
