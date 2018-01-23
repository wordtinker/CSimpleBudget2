using Models.Interfaces;
using System;
using System.Collections.ObjectModel;
using ViewModels.Elements;

namespace ViewModels.Reports
{
    public class BalanceReportViewModel
    {
        private IDataProvider dataProvider;
        private IPredictor predictor;

        public MonthYearSelector Selector { get; }
        public ObservableCollection<BalanceItem> BalanceRecords { get; }

        public BalanceReportViewModel(IDataProvider dataProvider, IPredictor predictor)
        {
            this.dataProvider = dataProvider;
            this.predictor = predictor;
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
            // Add all predictors for a selected period.
            DateTime actualDate = DateTime.Today;
            DateTime futureDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            // Repeat for every month before selected
            while (actualDate <= futureDate)
            {
                foreach (var (date, amount, category) in predictor.Predict(actualDate.Year, actualDate.Month))
                {
                    balance += amount;
                    BalanceRecords.Add(new BalanceItem
                    {
                        Date = date,
                        Change = amount,
                        Total = balance,
                        Origin = "Prediction",
                        Category = (new CategoryNode(category))
                    });
                }
                actualDate = actualDate.AddMonths(1);
            }
        }
    }
}
