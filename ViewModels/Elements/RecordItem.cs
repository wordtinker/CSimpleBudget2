﻿using Models.Interfaces;
using System;

namespace ViewModels.Elements
{
    public class RecordItem
    {
        internal IBudgetRecord record;

        public decimal Amount { get { return record.Amount; } }
        public CategoryNode Category { get { return new CategoryNode(record.Category); } }
        public string TypeName { get { return record.Type.ToString(); } }
        public string OnDayText
        {
            get
            {
                if (record.Type == BudgetType.Monthly || record.Type == BudgetType.Daily)
                {
                    return string.Empty;
                }
                if (record.Type == BudgetType.Weekly)
                {
                    return ((DayOfWeek)record.OnDay).ToString();
                }
                return record.OnDay.ToString();
            }
        }
        public RecordItem(IBudgetRecord record)
        {
            this.record = record;
        }
    }
}
