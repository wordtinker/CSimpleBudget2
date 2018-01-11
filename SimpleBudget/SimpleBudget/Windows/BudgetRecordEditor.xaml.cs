using System.Windows;
using ViewModels.Windows;

namespace SimpleBudget.Windows
{
    /// <summary>
    /// Interaction logic for BudgetRecordEditor.xaml
    /// </summary>
    public partial class BudgetRecordEditor : Window
    {
        public BudgetRecordEditor()
        {
            InitializeComponent();
        }
        // TODO Later command
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((AbstractBudgetRecordEditorViewModel)this.DataContext).Save();
            this.Close();
        }
    }
}
