using Models.Interfaces;
using System.Collections.ObjectModel;
using ViewModels.Elements;

namespace ViewModels.Reports
{
    public class BudgetReportViewModel
    {
        private IDataProvider dataProvider;

        public MonthYearSelector Selector { get; }
        public ObservableCollection<BudgetBar> Bars { get; }

        public BudgetReportViewModel(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            Bars = new ObservableCollection<BudgetBar>();
            Selector = new MonthYearSelector(dataProvider, -0, +0);
            Selector.PropertyChanged += (sender, e) => UpdateBars();
            UpdateBars();
        }

        private void UpdateBars()
        {
            Bars.Clear();
            foreach (var item in dataProvider.GetSpendings(Selector.SelectedYear, Selector.SelectedMonth))
            {
                Bars.Add(new BudgetBar(item));
            }
        }
    }
}
