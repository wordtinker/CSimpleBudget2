using Models.Interfaces;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ViewModels.Elements;

namespace ViewModels.Reports
{
    public class CategoriesReportViewModel : BindableBase
    {
        private IDataProvider dataProvider;
        private CategoryNode selectedCategory;

        public MonthYearSelector Selector { get; }
        public IEnumerable<CategoryNode> Categories =>
            from topCat in dataProvider.Categories
            from c in topCat.Children
            select new CategoryNode(c);
        public CategoryNode SelectedCategory
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
        public ObservableCollection<TransactionItem> Transactions { get; } = new ObservableCollection<TransactionItem>();
        // ctor
        public CategoriesReportViewModel(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            Selector = new MonthYearSelector(dataProvider, -0, +0);
            Selector.PropertyChanged += (sender, e) => UpdateBars();
            Bars = new ObservableCollection<BudgetBar>();
            Transactions = new ObservableCollection<TransactionItem>();
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
                    if (spending.Category == SelectedCategory.category)
                    {
                        Bars.Add(new BudgetBar(spending));
                    }
                }
            }
        }
        public void UpdateTransactions(BudgetBar bar)
        {
            Transactions.Clear();
            foreach (var tr in dataProvider.GetTransactions(Selector.SelectedYear, bar.Month, SelectedCategory.category))
            {
                Transactions.Add(new TransactionItem(tr));
            }
        }
    }
}
