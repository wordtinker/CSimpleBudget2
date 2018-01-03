using System.Windows;
using System.Windows.Controls;
using ViewModels.Elements;
using ViewModels.Windows;

namespace SimpleBudget.Windows
{
    /// <summary>
    /// Interaction logic for CategoriesManager.xaml
    /// </summary>
    public partial class CategoriesManager : Window
    {
        public CategoriesManager()
        {
            InitializeComponent();
        }
        // TODO Later command
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = CategoryName.Text;
            CategoryNode parent = (CategoryNode)ParentCategory.SelectedItem;
            ((CategoriesManagerViewModel)this.DataContext).AddCategory(categoryName, parent);
        }
        // TODO Later command
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            CategoryNode node = (CategoryNode)mi.DataContext;
            ((CategoriesManagerViewModel)this.DataContext).DeleteCategory(node);
        }
    }
}
