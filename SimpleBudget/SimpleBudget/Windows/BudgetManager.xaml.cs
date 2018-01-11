using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModels.Elements;
using ViewModels.Windows;

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
        // TODO Later command
        private void Record_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dgr = (DataGridRow)sender;
            RecordItem item = (RecordItem)dgr.DataContext;
            ((BudgetManagerViewModel)DataContext).ShowRecordEditor(item);
        }
        // TODO Later command
        private void DeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            RecordItem item = (RecordItem)mi.DataContext;
            ((BudgetManagerViewModel)this.DataContext).DeleteRecord(item);
        }
        // TODO Later command
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ((BudgetManagerViewModel)DataContext).ShowRecordEditor();
        }
    }
}
