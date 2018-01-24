using Models.Interfaces;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ViewModels.Elements;
using ViewModels.Events;

namespace ViewModels.Windows
{
    public abstract class AbstractBudgetRecordEditorViewModel : BindableBase
    {
        protected IEventAggregator eventAggregator;
        protected IDataProvider dataProvider;
        protected BudgetType BudgetType;

        private bool monthly;
        private bool point;
        private bool daily;
        private bool weekly;
        private int onDay;

        public MonthYearSelector Selector { get; }
        public decimal Amount { get; set; }
        public IEnumerable<CategoryNode> Categories =>
            from topCat in dataProvider.Categories
            from c in topCat.Children
            select new CategoryNode(c); 
        public CategoryNode Category { get; set; }
        public int OnDay { get => onDay; set => SetProperty(ref onDay, value); }
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
        public IEnumerable<int> Days => Enumerable.Range(1, DateTime.DaysInMonth(Selector.SelectedYear, Selector.SelectedMonth));
        public AbstractBudgetRecordEditorViewModel(IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.dataProvider = dataProvider;
            Selector = new MonthYearSelector(dataProvider, -1, +3);
            Selector.PropertyChanged += (sender, e) => RaisePropertyChanged(nameof(Days));
            Category = Categories.First();
        }
        public abstract void Save();
    }

    public class NewBudgetRecordEditorViewModel : AbstractBudgetRecordEditorViewModel
    {
        public NewBudgetRecordEditorViewModel(IDataProvider dataProvider, IEventAggregator eventAggregator)
            : base(dataProvider, eventAggregator)
        {
            Monthly = true;
        }

        public override void Save()
        {
            if (dataProvider.AddBudgetRecord(Amount, Category.category, BudgetType, OnDay, Selector.SelectedYear, Selector.SelectedMonth, out IBudgetRecord newRecord))
            {
                eventAggregator.GetEvent<BudgetRecordAdded>().Publish(new RecordItem(newRecord));
            }
        }
    }

    public class OldBudgetRecordEditorViewModel : AbstractBudgetRecordEditorViewModel
    {
        private RecordItem recordItem;

        public OldBudgetRecordEditorViewModel(RecordItem recordItem, IDataProvider dataProvider, IEventAggregator eventAggregator)
            : base(dataProvider, eventAggregator)
        {
            this.recordItem = recordItem;
            Selector.SelectedYear = recordItem.Year;
            Selector.SelectedMonthName = DateTimeFormatInfo.CurrentInfo.MonthNames[recordItem.Month - 1];

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
            // update budget record in the model
            if (dataProvider.UpdateRecord(recordItem.record, Amount, Category.category, BudgetType, OnDay, Selector.SelectedMonth, Selector.SelectedYear))
            {
                RecordItem old = new RecordItem(recordItem.record);
                // and in the view model
                recordItem.Amount = Amount;
                recordItem.Category = Category;
                recordItem.Type = BudgetType;
                recordItem.OnDay = OnDay;
                recordItem.Month = Selector.SelectedMonth;
                recordItem.Year = Selector.SelectedYear;
                eventAggregator.GetEvent<BudgetRecordChanged>().Publish(
                    new BudgetRecordChange
                    {
                        Old = old,
                        New = recordItem
                    });
            }
        }
    }
}
