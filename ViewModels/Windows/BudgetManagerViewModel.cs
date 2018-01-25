using Models.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
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

        public ObservableCollection<RecordItem> Records { get; }
        public MonthYearSelector Selector { get; }
        public ICommand DeleteRecord { get; }
        public ICommand AddRecord { get; }
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
            Selector = new MonthYearSelector(dataProvider, -1, +3);
            Selector.PropertyChanged += (sender, e) => UpdateRecords();
            Records = new ObservableCollection<RecordItem>();

            AddRecord = new DelegateCommand(service.ShowBudgetRecordEditor);
            DeleteRecord = new DelegateCommand<object>(_DeleteRecord);
            RequestCopyFrom = new DelegateCommand(
                () => service.RequestMonthAndYear(Selector.SelectedMonth, Selector.SelectedYear));

            UpdateRecords();
            eventAggregator.GetEvent<BudgetRecordAdded>().Subscribe(
                ri => Records.Add(ri), ThreadOption.PublisherThread, false,
                ri => ri.Month == Selector.SelectedMonth && ri.Year == Selector.SelectedYear);
        }
        /// <summary>
        /// Clears list of records and loads new list for selected month and year.
        /// </summary>
        private void UpdateRecords()
        {
            Records.Clear();
            foreach (IBudgetRecord rec in
                dataProvider.GetRecords(Selector.SelectedYear, Selector.SelectedMonth))
            {
                Records.Add(new RecordItem(rec));
            }
        }
        private void _DeleteRecord(object parameter)
        {
            if (parameter is RecordItem item)
            {
                if (dataProvider.DeleteRecord(item.record))
                {
                    Records.Remove(item);
                    eventAggregator.GetEvent<BudgetRecordDeleted>().Publish(item);
                }
            }
        }
        public void ShowRecordEditor(RecordItem recordItem)
        {
            service.ShowBudgetRecordEditor(recordItem);
        }
    }
}
