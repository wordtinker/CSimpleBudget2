using System.Windows;
using ViewModels.Interfaces;

namespace SimpleBudget.Services
{
    class TransactionRollService : IUITransactionRollService
    {
        private Window rollWindow;

        public TransactionRollService(Window rollWindow)
        {
            this.rollWindow = rollWindow;
        }
    }
}
