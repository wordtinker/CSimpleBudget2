using Models.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    public class AccTypeManagerViewModel : BindableBase
    {
        private IUIBaseService serviceProvider;
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;

        public IEnumerable<AccTypeItem> AccTypes => from t in dataProvider.AccountTypes select new AccTypeItem(t);
        public ICommand DeleteAccountType { get; }

        public AccTypeManagerViewModel(IUIBaseService serviceProvider, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.serviceProvider = serviceProvider;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
            DeleteAccountType = new DelegateCommand<object>(_DeleteAccountType);
        }
        public void AddAccType(string accTypeName)
        {
            if (dataProvider.AddAccountType(accTypeName))
            {
                AccTypeItem newAccType = new AccTypeItem(accTypeName);
                RaisePropertyChanged(nameof(AccTypes));
            }
            else
            {
                serviceProvider.ShowMessage("Can't add account type.");
            }
        }
        private void _DeleteAccountType(object parameter)
        {
            if (parameter is AccTypeItem item)
            {
                if (dataProvider.DeleteAccountType(item.Name))
                {
                    RaisePropertyChanged(nameof(AccTypes));
                }
                else
                {
                    serviceProvider.ShowMessage("Can't delete account type.");
                }
            }
        }
    }
}
