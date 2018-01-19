using System.Windows;
using System.Windows.Controls;
using ViewModels.Elements;
using ViewModels.Windows;

namespace SimpleBudget.Windows
{
    /// <summary>
    /// Interaction logic for AccTypeManager.xaml
    /// </summary>
    public partial class AccTypeManager : Window
    {
        public AccTypeManager()
        {
            InitializeComponent();
        }
        private void DeleteAccountType_Click(object sender, RoutedEventArgs e)
        {
            // TODO Later command
            MenuItem mi = (MenuItem)sender;
            AccTypeItem item = (AccTypeItem)mi.DataContext;
            ((AccTypeManagerViewModel)this.DataContext).DeleteAccType(item);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO Later command
            string accTypeName = AccTypeName.Text;
            ((AccTypeManagerViewModel)this.DataContext).AddAccType(accTypeName);
        }
    }
}
