﻿using SimpleBudget.Windows;
using System.Windows;
using Unity;
using Unity.Resolution;
using ViewModels.Windows;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace SimpleBudget.Services
{
    class TransactionRollService : BaseWindowService, IUITransactionRollService
    {
        private Window rollWindow;

        public TransactionRollService(Window rollWindow)
        {
            this.rollWindow = rollWindow;
        }

        public void ShowTransactionEditor(TransactionItem transactionItem)
        {
            TransactionEditor window = new TransactionEditor
            {
                Owner = rollWindow,
                DataContext = App.Container.Resolve<OldTransactionEditorViewModel>(
                    new ParameterOverride("transactionItem", transactionItem))
            };
            window.ShowDialog();
        }

        public void ShowTransactionEditor(AccountItem accountItem)
        {
            TransactionEditor window = new TransactionEditor
            {
                Owner = rollWindow,
                DataContext = App.Container.Resolve<NewTransactionEditorViewModel>(
                    new ParameterOverride("accountItem", accountItem))
            };
            window.ShowDialog();
        }
    }
}
