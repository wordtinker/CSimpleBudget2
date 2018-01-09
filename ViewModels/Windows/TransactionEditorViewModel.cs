using Models.Interfaces;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using ViewModels.Elements;
using ViewModels.Events;

namespace ViewModels.Windows
{
    public abstract class AbstractTransactionEditor
    {
        protected IEventAggregator eventAggregator;
        protected IDataProvider dataProvider;

        public ObservableCollection<CategoryNode> Categories { get; private set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Info { get; set; }
        public CategoryNode Category { get; set; }

        public AbstractTransactionEditor(IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.dataProvider = dataProvider;
            Categories = new ObservableCollection<CategoryNode>();
            foreach (ICategory item in dataProvider.GetCategories())
            {
                foreach (ICategory child in item.Children)
                {
                    Categories.Add(new CategoryNode(child));
                }
            }
            Category = Categories[0];
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
            dataProvider.AddTransaction(accountItem.account, Date, Amount, Info, Category.category, out ITransaction newTr);
            eventAggregator.GetEvent<TransactionAdded>().Publish(new TransactionItem(newTr));
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
            dataProvider.UpdateTransaction(transactionItem.transaction, Date, Amount, Info, Category.category);
            // And in the viewModel
            transactionItem.Date = Date;
            transactionItem.Amount = Amount;
            transactionItem.Info = Info;
            transactionItem.Category = Category;
            eventAggregator.GetEvent<TransactionChanged>().Publish(transactionItem);
        }
    }
}
