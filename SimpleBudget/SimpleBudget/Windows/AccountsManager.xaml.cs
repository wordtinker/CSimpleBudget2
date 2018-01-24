using System.Windows;
using System.Windows.Controls;
using ViewModels.Elements;
using ViewModels.Windows;

namespace SimpleBudget.Windows
{
    /// <summary>
    /// Interaction logic for AccountsManager.xaml
    /// </summary>
    public partial class AccountsManager : Window
    {
        public AccountsManager()
        {
            InitializeComponent();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO Later command
            string accName = AccName.Text;
            ((AccountsManagerViewModel)this.DataContext).AddAccount(accName);
        }
    }
}
