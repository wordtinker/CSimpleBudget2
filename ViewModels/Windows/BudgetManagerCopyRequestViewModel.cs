using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ViewModels.Windows
{
    public class BudgetManagerCopyRequestViewModel
    {
        private IDataProvider dataProvider;

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
        public BudgetManagerCopyRequestViewModel(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }
    }
}
