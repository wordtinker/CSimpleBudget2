using Models.Interfaces;
using Prism.Mvvm;
using System;

namespace ViewModels.Elements
{
    public class RecordItem : BindableBase
    {
        internal IBudgetRecord record;
        private decimal amount;
        private CategoryNode catNode;
        private BudgetType type;
        private int onDay;
        private int year;
        private int month;

        public decimal Amount { get => amount; set => SetProperty(ref amount, value); }
        public CategoryNode Category { get => catNode; set => SetProperty(ref catNode, value); }
        public BudgetType Type { get => type; set => SetProperty(ref type, value); }
        public int OnDay { get => onDay; set => SetProperty(ref onDay, value); }
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
                    return ((DayOfWeek)record.OnDay).ToString();
                }
                return record.OnDay.ToString();
            }
        }
        public RecordItem(IBudgetRecord record)
        {
            this.record = record;
            Amount = record.Amount;
            Category = new CategoryNode(record.Category);
            Type = record.Type;
            OnDay = record.OnDay;
            Year = record.Year;
            Month = record.Month;
        }
    }
}
