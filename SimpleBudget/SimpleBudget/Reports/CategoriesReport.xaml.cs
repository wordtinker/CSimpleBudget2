using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleBudget.Reports
{
    /// <summary>
    /// Interaction logic for CategoriesReport.xaml
    /// </summary>
    public partial class CategoriesReport : Window
    {
        public CategoriesReport()
        {
            InitializeComponent();
        }
        private void Bar_Click(object sender, MouseButtonEventArgs e)
        {
            // TODO
            //BarDataPoint bdp = (BarDataPoint)sender;
            //BudgetBar bar = (BudgetBar)bdp.DataContext;
            //((CategoriesReportViewModel)DataContext).UpdateTransactions(bar);
        }
    }
}
