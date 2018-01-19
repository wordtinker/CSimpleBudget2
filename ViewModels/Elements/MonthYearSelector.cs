using Models.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ViewModels.Elements
{
    public class MonthYearSelector : BindableBase
    {
        private IDataProvider dataProvider;
        private string selectedMonthName;
        private int selectedYear;
        private int lowerShift;
        private int upperShift;

        public List<string> Months => DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToList();
        public string SelectedMonthName
        {
            get => selectedMonthName;
            set => SetProperty(ref selectedMonthName, value);
        }
        public int SelectedMonth => DateTime.ParseExact(SelectedMonthName, "MMMM", CultureInfo.CurrentCulture).Month;

        public IEnumerable<int> Years
        {
            get
            {
                (int minYear, int maxYear) = dataProvider.GetActiveBudgetYears();
                return Enumerable.Range(minYear + lowerShift, maxYear + 1 - minYear - lowerShift + upperShift);
            }
        }
        public int SelectedYear
        {
            get => selectedYear;
            set => SetProperty(ref selectedYear, value);
        }
        // ctor
        public MonthYearSelector(IDataProvider dataProvider, int lowerShift, int upperShift)
        {
            this.dataProvider = dataProvider;
            this.lowerShift = lowerShift;
            this.upperShift = upperShift;
            selectedMonthName = DateTime.Now.ToString("MMMM");
            selectedYear = DateTime.Now.Year;
        }
    }
}
