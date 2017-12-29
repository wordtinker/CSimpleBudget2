using System.Windows;
using System.Windows.Input;

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
        // TODO
        public void Account_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //DataGridRow dgr = (DataGridRow)sender;
            //AccountItem item = (AccountItem)dgr.DataContext;
            //MainWindowViewModel vm = (MainWindowViewModel)this.DataContext;
            //vm.ShowTransactionRoll(item);
        }
    }
}
