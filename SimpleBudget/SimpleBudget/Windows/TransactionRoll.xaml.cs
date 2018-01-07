using System.Windows;
using System.Windows.Input;

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
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO 
            //((TransactionRollViewModel)DataContext).ShowTransactionEditor();
        }

        private void Transaction_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // TODO
            //DataGridRow dgr = (DataGridRow)sender;
            //TransactionItem item = (TransactionItem)dgr.DataContext;
            //((TransactionRollViewModel)DataContext).ShowTransactionEditor(item);
        }

        private void DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            // TODO
            //MenuItem mi = (MenuItem)sender;
            //TransactionItem item = (TransactionItem)mi.DataContext;
            //if (!((TransactionRollViewModel)this.DataContext).DeleteTransaction(item))
            //{
            //    MessageBox.Show("Can't delete transaction.");
            //}
        }
    }
}
