using Models.Interfaces;
using Prism.Mvvm;
using System;
using ViewModels.Interfaces;

namespace ViewModels.Elements
{
    public class RecordItem : BindableBase, IRecordItem
    {
        private decimal amount;
        private ICategoryNode catNode;
        private BudgetType type;
        private int onDay;
        private int year;
        private int month;

        public IBudgetRecord InnerRecord { get; private set; }
        public decimal Amount { get => amount; set => SetProperty(ref amount, value); }
        public ICategoryNode Category { get => catNode; set => SetProperty(ref catNode, value); }
        public BudgetType Type
        {
            get => type;
            set
            {
                if (SetProperty(ref type, value))
                {
                    RaisePropertyChanged(nameof(TypeName));
                    RaisePropertyChanged(nameof(OnDayText));
                }
            }
        }
        public int OnDay
        {
            get => onDay;
            set
            {
                if (SetProperty(ref onDay, value))
                {
                    RaisePropertyChanged(nameof(OnDayText));
                }
            }
        }
        public int Year { get => year; set => SetProperty(ref year, value); }
        public int Month { get => month; set => SetProperty(ref month, value); }

        public string TypeName => Type.ToString();
        public string OnDayText
        {
            get
            {
                if (Type == BudgetType.Monthly || Type == BudgetType.Daily)
                {
                    return string.Empty;
                }
                if (Type == BudgetType.Weekly)
                {
                    return ((DayOfWeek)OnDay).ToString();
                }
                return OnDay.ToString();
            }
        }

        public RecordItem(IBudgetRecord record)
        {
            InnerRecord = record;
            Amount = record.Amount;
            Category = new CategoryNode(record.Category);
            Type = record.Type;
            OnDay = record.OnDay;
            Year = record.Year;
            Month = record.Month;
        }
    }
}
