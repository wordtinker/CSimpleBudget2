using Models.Interfaces;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ViewModels.Elements;
using ViewModels.Events;

namespace ViewModels.Windows
{
    public class BudgetManagerCopyRequestViewModel
    {
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;
        private int toMonth;
        private int toYear;

        public List<string> Months { get; } = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToList();
        /// <summary>
        /// This value is used as "return" value of copy request.
        /// </summary>
        public int SelectedMonth => DateTime.ParseExact(SelectedMonthName, "MMMM", CultureInfo.CurrentCulture).Month;
        public string SelectedMonthName { get; set; } = DateTime.Now.ToString("MMMM");
        public IEnumerable<int> Years
        {
            get
            {
                (int minYear, int maxYear) = dataProvider.GetActiveBudgetYears();
                return Enumerable.Range(minYear, 1 + maxYear - minYear);
            }
        }
        /// <summary>
        /// This value is used as "return" value of copy request.
        /// </summary>
        public int SelectedYear { get; set; } = DateTime.Now.Year;
        // ctor
        public BudgetManagerCopyRequestViewModel(int toMonth, int toYear, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.toMonth = toMonth;
            this.toYear = toYear;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
        }
        public void Copy()
        {
            foreach (IBudgetRecord rec in dataProvider.CopyRecords(SelectedMonth, SelectedYear, toMonth, toYear))
            {
                eventAggregator.GetEvent<BudgetRecordAdded>().Publish(new RecordItem(rec));
            }
        }
    }
}
