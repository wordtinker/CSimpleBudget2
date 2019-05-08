using Models.Interfaces;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Reports
{
    public class CategoriesReportViewModel : BindableBase
    {
        private IDataProvider dataProvider;
        private ICategoryNode selectedCategory;

        public MonthYearSelector Selector { get; }
        public IEnumerable<ICategoryNode> Categories =>
            from topCat in dataProvider.Categories
            from c in topCat.Children
            select new CategoryNode(c);
        public ICategoryNode SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (SetProperty(ref selectedCategory, value))
                {
                    UpdateBars();
                }
            }
        }
        public ObservableCollection<BudgetBar> Bars { get; }
        public ObservableCollection<ITransactionItem> Transactions { get; }
        // ctor
        public CategoriesReportViewModel(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            Selector = new MonthYearSelector(dataProvider, -0, +0);
            Selector.PropertyChanged += (sender, e) => UpdateBars();
            Bars = new ObservableCollection<BudgetBar>();
            Transactions = new ObservableCollection<ITransactionItem>();
            SelectedCategory = Categories.First();
            UpdateBars();
        }

        private void UpdateBars()
        {
            Transactions.Clear();
            Bars.Clear();
            for (int month = 1; month < 13; month++)
            {
                foreach (var spending in dataProvider.GetSpendings(Selector.SelectedYear, month))
                {
                    if (spending.Category == SelectedCategory.InnerCategory)
                    {
                        Bars.Add(new BudgetBar(spending));
                    }
                }
            }
        }
        public void UpdateTransactions(BudgetBar bar)
        {
            Transactions.Clear();
            foreach (var tr in dataProvider.GetTransactions(Selector.SelectedYear, bar.Month, SelectedCategory.InnerCategory))
            {
                Transactions.Add(new TransactionItem(tr));
            }
        }
    }
}
