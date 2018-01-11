using System.Windows;
using ViewModels.Windows;

namespace SimpleBudget.Windows
{
    /// <summary>
    /// Interaction logic for BudgetManagerCopyRequest.xaml
    /// </summary>
    public partial class BudgetManagerCopyRequest : Window
    {
        public BudgetManagerCopyRequest()
        {
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ((BudgetManagerCopyRequestViewModel)this.DataContext).Copy();
            this.Close();
        }
    }
}
