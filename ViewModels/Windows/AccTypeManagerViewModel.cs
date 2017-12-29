using Models.Interfaces;
using Prism.Events;
using System.Collections.ObjectModel;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    // TODO Later add validation
    public class AccTypeManagerViewModel
    {
        private IUIBaseService serviceProvider;
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;

        public ObservableCollection<AccTypeItem> AccTypes { get; }

        public AccTypeManagerViewModel(IUIBaseService serviceProvider, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.serviceProvider = serviceProvider;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
            AccTypes = new ObservableCollection<AccTypeItem>();
            foreach (var item in dataProvider.GetAccountTypes())
            {
                AccTypes.Add(new AccTypeItem(item));
            }
        }
        public void AddAccType(string accTypeName)
        {
            if (!dataProvider.AddAccountType(accTypeName))
            {
                serviceProvider.ShowMessage("Can't add account type.");
            }
            else
            {
                AccTypes.Add(new AccTypeItem(accTypeName));
                // TODO event
            }
        }
        public void DeleteAccType(AccTypeItem item)
        {
            if (!dataProvider.DeleteAccountType(item.Name))
            {
                serviceProvider.ShowMessage("Can't delete account type.");
            }
            else
            {
                AccTypes.Remove(item);
                // TODO event
            }
        }
    }
}
