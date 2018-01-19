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

        public MonthYearSelector Selector { get; }

        // ctor
        public BudgetManagerCopyRequestViewModel(int toMonth, int toYear, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.toMonth = toMonth;
            this.toYear = toYear;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
            Selector = new MonthYearSelector(dataProvider, -0, +0);
        }
        public void Copy()
        {
            foreach (IBudgetRecord rec in dataProvider.CopyRecords(
                Selector.SelectedMonth, Selector.SelectedYear, toMonth, toYear))
            {
                eventAggregator.GetEvent<BudgetRecordAdded>().Publish(new RecordItem(rec));
            }
        }
    }
}
