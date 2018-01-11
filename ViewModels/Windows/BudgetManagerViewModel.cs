using Models.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using ViewModels.Elements;
using ViewModels.Events;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    public class BudgetManagerViewModel : BindableBase
    {
        private IUIBudgetWindowService service;
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;
        private string selectedMonthName;
        private int selectedYear;

        public ObservableCollection<RecordItem> Records { get; }
        public List<string> Months { get; }
        public int SelectedMonth => DateTime.ParseExact(SelectedMonthName, "MMMM", CultureInfo.CurrentCulture).Month;
        public string SelectedMonthName
        {
            get => selectedMonthName;
            set
            {
                if (SetProperty(ref selectedMonthName, value))
                {
                    UpdateRecords();
                }
            }
        }
        public IEnumerable<int> Years
        {
            get
            {
                (int minYear, int maxYear) = dataProvider.GetActiveBudgetYears();
                return Enumerable.Range(minYear - 1, 5 + maxYear - minYear);
            }
        }
        public int SelectedYear
        {
            get => selectedYear;
            set
            {
                if (SetProperty(ref selectedYear, value))
                {
                    UpdateRecords();
                }
            }
        }
        /// <summary>
        /// Raises request for managing control to retrieve month and year 
        /// amd copy all budget records to selected month and year.
        /// </summary>
        public ICommand RequestCopyFrom { get; }

        //ctor
        public BudgetManagerViewModel(IUIBudgetWindowService service, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.service = service;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
            Months = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToList();
            selectedMonthName = DateTime.Now.ToString("MMMM");
            selectedYear = DateTime.Now.Year;
            Records = new ObservableCollection<RecordItem>();

            RequestCopyFrom = new DelegateCommand(() => service.RequestMonthAndYear(SelectedMonth, SelectedYear));

            UpdateRecords();
            eventAggregator.GetEvent<BudgetRecordAdded>().Subscribe(
                ri => Records.Add(ri), ThreadOption.PublisherThread, false,
                ri => ri.Month == SelectedMonth && ri.Year == SelectedYear);
        }
        /// <summary>
        /// Clears list of records and loads new list for selected month and year.
        /// </summary>
        private void UpdateRecords()
        {
            Records.Clear();
            foreach (IBudgetRecord rec in dataProvider.GetRecords(SelectedYear, SelectedMonth))
            {
                Records.Add(new RecordItem(rec));
            }
        }
        public void DeleteRecord(RecordItem recordItem)
        {
            dataProvider.DeleteRecord(recordItem.record);
            Records.Remove(recordItem);
            eventAggregator.GetEvent<BudgetRecordDeleted>().Publish(recordItem);
        }
        public void ShowRecordEditor()
        {
            service.ShowBudgetRecordEditor();
        }
        public void ShowRecordEditor(RecordItem recordItem)
        {
            service.ShowBudgetRecordEditor(recordItem);
        }
    }
}
