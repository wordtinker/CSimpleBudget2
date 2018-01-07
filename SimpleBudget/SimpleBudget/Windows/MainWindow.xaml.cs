using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModels.Elements;
using ViewModels.Windows;

namespace SimpleBudget.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // TODO Later command
        public void Account_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dgr = (DataGridRow)sender;
            IAccountItem item = (IAccountItem)dgr.DataContext;
            MainWindowViewModel vm = (MainWindowViewModel)this.DataContext;
            vm.ShowTransactionRoll(item);
        }
    }
}
