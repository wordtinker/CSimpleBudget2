using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleBudget.Windows
{
    /// <summary>
    /// Interaction logic for BudgetManager.xaml
    /// </summary>
    public partial class BudgetManager : Window
    {
        public BudgetManager()
        {
            InitializeComponent();
        }
        private void Record_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // TODO
            //DataGridRow dgr = (DataGridRow)sender;
            //RecordItem item = (RecordItem)dgr.DataContext;
            //((BudgetManagerViewModel)DataContext).ShowRecordEditor(item);
        }

        private void DeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            // TODO
            //MenuItem mi = (MenuItem)sender;
            //RecordItem item = (RecordItem)mi.DataContext;
            //if (!((BudgetManagerViewModel)this.DataContext).DeleteRecord(item))
            //{
            //    MessageBox.Show("Can't delete budget record.");
            //}
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO
            //((BudgetManagerViewModel)DataContext).ShowRecordEditor();
        }
    }
}
