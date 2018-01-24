using Models.Interfaces;
using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    public class CategoriesManagerViewModel
    {
        private IUIBaseService serviceProvider;
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;

        public ObservableCollection<CategoryNode> Categories { get; }
        public ICommand DeleteCategory { get; }

        public CategoriesManagerViewModel(IUIBaseService serviceProvider, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.serviceProvider = serviceProvider;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;

            DeleteCategory = new DelegateCommand<object>(_DeleteCategory);
            Categories = new ObservableCollection<CategoryNode>();
            foreach (ICategory item in dataProvider.Categories)
            {
                Categories.Add(new CategoryNode(item));
            }
        }
        public void AddCategory(string categoryName, CategoryNode parent)
        {
            if(dataProvider.AddCategory(categoryName, parent?.category, out ICategory newCategory))
            {
                var node = new CategoryNode(newCategory);
                if (parent == null)
                {
                    Categories.Add(node);
                }
                else
                {
                    parent.AddChild(node);
                }
            }
            else
            {
                serviceProvider.ShowMessage("Can't add category.");
            }
        }
        private void _DeleteCategory(object parameter)
        {
            if (parameter is CategoryNode node)
            {
                ICategory category = node.category;
                if (dataProvider.DeleteCategory(category))
                {
                    if (node.Parent == null)
                    {
                        Categories.Remove(node);
                    }
                    else
                    {
                        node.Parent.RemoveChild(node);
                    }
                }
                else
                {
                    serviceProvider.ShowMessage("Can't delete category.");
                }
            }
        }
    }
}
