using Models.Interfaces;
using System;
using System.Collections.ObjectModel;
using ViewModels.Elements;

namespace ViewModels.Reports
{
    public class BalanceReportViewModel
    {
        private IDataProvider dataProvider;

        public MonthYearSelector Selector { get; }
        public ObservableCollection<BalanceItem> BalanceRecords { get; }

        public BalanceReportViewModel(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            BalanceRecords = new ObservableCollection<BalanceItem>();
            Selector = new MonthYearSelector(dataProvider, -0, +0);
            Selector.PropertyChanged += (sender, e) => UpdateBalance();
            UpdateBalance();
        }

        private void UpdateBalance()
        {
            BalanceRecords.Clear();

            int year = Selector.SelectedYear;
            int month = Selector.SelectedMonth;
            // Add starting balance row
            var (balance, lastTransactionDate) = dataProvider.GetBalanceToDate(year, month);
            BalanceRecords.Add(new BalanceItem
            {
                Date = lastTransactionDate,
                Change = 0m,
                Total = balance,
                Origin = "Balance",
                Category = null
            });
            // Add all transactions for a selected period
            foreach (ITransaction tr in dataProvider.GetTransactions(year, month))
            {
                // filter out transaction before the last transaction date
                if (tr.Date > lastTransactionDate)
                {
                    balance += tr.Amount;
                    BalanceRecords.Add(new BalanceItem
                    {
                        Date = tr.Date,
                        Change = tr.Amount,
                        Total = balance,
                        Origin = "Transaction",
                        Category = (new CategoryNode(tr.Category))
                    });
                }
            }
            // TODO !!! !
        }
    }
}
