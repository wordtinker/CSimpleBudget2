using Models.Interfaces;
using System;
using System.Collections.ObjectModel;
using ViewModels.Elements;

namespace ViewModels.Windows
{
    public abstract class AbstractTransactionEditor
    {
        protected IDataProvider dataProvider;

        public ObservableCollection<CategoryNode> Categories { get; private set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Info { get; set; }
        public CategoryNode Category { get; set; }

        public AbstractTransactionEditor(IDataProvider dataProvider)
        {
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

        public NewTransactionEditorViewModel(AccountItem accountItem, IDataProvider dataProvider) : base(dataProvider)
        {
            this.accountItem = accountItem;
            Date = DateTime.Now;
        }

        public override void Save()
        {
            // Create new transaction.
            dataProvider.AddTransaction(accountItem.account, Date, Amount, Info, Category.category, out ITransaction newTr);
            // TODO event to transaction roll and main window
        }
    }
    public class OldTransactionEditorViewModel : AbstractTransactionEditor
    {
        private TransactionItem transactionItem;

        public OldTransactionEditorViewModel(TransactionItem transactionItem, IDataProvider dataProvider) : base(dataProvider)
        {
            // TODO !!!
            this.transactionItem = transactionItem;
            Date = transactionItem.Date;
            //Amount = transactionItem.Amount;
            Info = transactionItem.Info;
            //Category = transactionItem.Category;
        }

        public override void Save()
        {
            // Update transaction.
            dataProvider.UpdateTransaction(transactionItem.transaction, Date, Amount, Info, Category.category);
            // TODO event to main window and direct update of trItem
        }
    }
}
