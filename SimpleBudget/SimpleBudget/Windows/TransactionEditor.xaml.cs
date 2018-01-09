using System.Windows;
using ViewModels.Windows;

namespace SimpleBudget.Windows
{
    /// <summary>
    /// Interaction logic for TransactionEditor.xaml
    /// </summary>
    public partial class TransactionEditor : Window
    {
        public TransactionEditor()
        {
            InitializeComponent();
        }
        // TODO Later Command
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((AbstractTransactionEditor)this.DataContext).Save();
            this.Close();
        }
    }
}
