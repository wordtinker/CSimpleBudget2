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
        // TODO Later command
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ((TransactionRollViewModel)DataContext).ShowTransactionEditor();
        }
        // TODO Later command
        private void Transaction_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dgr = (DataGridRow)sender;
            TransactionItem item = (TransactionItem)dgr.DataContext;
            ((TransactionRollViewModel)DataContext).ShowTransactionEditor(item);
        }
        // TODO Later command
        private void DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            TransactionItem item = (TransactionItem)mi.DataContext;
            ((TransactionRollViewModel)this.DataContext).DeleteTransaction(item);
        }
    }
}
