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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((AbstractBudgetRecordEditorViewModel)this.DataContext).Save();
            this.Close();
        }
    }
}
