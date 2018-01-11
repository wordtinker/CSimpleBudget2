using Models.Interfaces;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using ViewModels.Elements;

namespace ViewModels.Windows
{
    public abstract class AbstractBudgetRecordEditorViewModel
    {
        protected IEventAggregator eventAggregator;
        protected IDataProvider dataProvider;

        private BudgetType BudgetType;
        private bool monthly;
        private bool point;
        private bool daily;
        private bool weekly;

        public IEnumerable<int> Years
        {
            get
            {
                (int minYear, int maxYear) = dataProvider.GetActiveBudgetYears();
                return Enumerable.Range(minYear - 1, 5 + maxYear - minYear);
            }
        }
        public int Year { get; set; }
        public List<string> Months { get; }
        public string MonthName { get; set; }
        // TODO
        //public int Month => DateTime.ParseExact(SelectedMonthName, "MMMM", CultureInfo.CurrentCulture).Month;
        public decimal Amount { get; set; }
        public ObservableCollection<CategoryNode> Categories { get; private set; }
        public CategoryNode Category { get; set; }
        public int OnDay { get; set; }
        public bool Monthly
        {
            get { return monthly; }
            set
            {
                monthly = value;
                if (monthly)
                {
                    BudgetType = BudgetType.Monthly;
                    OnDay = 0;
                }
            }
        }
        public bool Point
        {
            get { return point; }
            set
            {
                point = value;
                if (point)
                {
                    BudgetType = BudgetType.Point;
                    OnDay = 1;
                }
            }
        }
        public bool Daily
        {
            get { return daily; }
            set
            {
                daily = value;
                if (daily)
                {
                    BudgetType = BudgetType.Daily;
                    OnDay = 0;
                }
            }
        }
        public bool Weekly
        {
            get { return weekly; }
            set
            {
                weekly = value;
                if (weekly)
                {
                    BudgetType = BudgetType.Weekly;
                    OnDay = 0;
                }
            }
        }
        public IEnumerable<string> DaysOfWeek => Enum.GetNames(typeof(DayOfWeek));
        // TODO days of the selected month
        public IEnumerable<int> Days => Enumerable.Range(1, 30);
        public AbstractBudgetRecordEditorViewModel(IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.dataProvider = dataProvider;

            Months = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToList();
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

    public class NewBudgetRecordEditorViewModel : AbstractBudgetRecordEditorViewModel
    {
        public NewBudgetRecordEditorViewModel(IDataProvider dataProvider, IEventAggregator eventAggregator)
            : base(dataProvider, eventAggregator)
        {
            Year = DateTime.Now.Year;
            MonthName = DateTimeFormatInfo.CurrentInfo.MonthNames[DateTime.Now.Month - 1];
            Monthly = true;
        }

        public override void Save()
        {
            // TODO !!!
            //BudgetRecord newRecord;
            //if (Core.Instance.AddRecord(
            //    vm.Amount, vm.Category.category,
            //    vm.BudgetType, vm.OnDay,
            //    vm.Month, vm.Year, out newRecord))
            //{
            //    if (newRecord.Month == SelectedMonth && newRecord.Year == SelectedYear)
            //    {
            //        Records.Add(new RecordItem(newRecord));
            //    }
            //}
            // TODO event and filter
        }
    }

    public class OldBudgetRecordEditorViewModel : AbstractBudgetRecordEditorViewModel
    {
        private RecordItem recordItem;

        public OldBudgetRecordEditorViewModel(RecordItem recordItem, IDataProvider dataProvider, IEventAggregator eventAggregator)
            : base(dataProvider, eventAggregator)
        {
            this.recordItem = recordItem;
            Year = recordItem.Year;
            MonthName = DateTimeFormatInfo.CurrentInfo.MonthNames[recordItem.Month - 1];

            Amount = recordItem.Amount;
            Category = recordItem.Category;

            switch (recordItem.Type)
            {
                case BudgetType.Monthly:
                    Monthly = true;
                    break;
                case BudgetType.Point:
                    Point = true;
                    OnDay = recordItem.OnDay;
                    break;
                case BudgetType.Daily:
                    Daily = true;
                    break;
                case BudgetType.Weekly:
                    Weekly = true;
                    OnDay = recordItem.OnDay;
                    break;
                default:
                    break;
            }
        }

        public override void Save()
        {
            // TODO !!!
            //if (Core.Instance.UpdateRecord(
            //        item.record, vm.Amount, vm.Category.category,
            //        vm.BudgetType, vm.OnDay,
            //        vm.Month, vm.Year))
            //{
            //    if (vm.Month != SelectedMonth || vm.Year != selectedYear)
            //    {
            //        Records.Remove(item);
            //    }
            //}
            // TODO event and filter
        }
    }
}
