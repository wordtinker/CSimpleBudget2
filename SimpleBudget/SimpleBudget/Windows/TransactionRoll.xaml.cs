using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModels.Elements;
using ViewModels.Windows;

namespace SimpleBudget.Windows
{
    /// <summary>
    /// Interaction logic for TransactionRoll.xaml
    /// </summary>
    public partial class TransactionRoll : Window
    {
        public TransactionRoll()
        {
            InitializeComponent();
        }
        private void Transaction_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dgr = (DataGridRow)sender;
            TransactionItem item = (TransactionItem)dgr.DataContext;
            ((TransactionRollViewModel)DataContext).ShowTransactionEditor(item);
        }
    }
}
