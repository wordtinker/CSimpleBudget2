using Models.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    public class CategoriesManagerViewModel : BindableBase
    {
        private IUIBaseService serviceProvider;
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;
        private string name;

        public ObservableCollection<CategoryNode> Categories { get; }
        public CategoryNode SelectedCategory { get; set; }
        public ICommand DeleteCategory { get; }
        public ICommand AddCategory { get; }
        public string Name { get => name; set => SetProperty(ref name, value); }

        public CategoriesManagerViewModel(IUIBaseService serviceProvider, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.serviceProvider = serviceProvider;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;

            DeleteCategory = new DelegateCommand<object>(_DeleteCategory);
            AddCategory = new DelegateCommand(_AddCategory);
            Categories = new ObservableCollection<CategoryNode>();
            foreach (ICategory item in dataProvider.Categories)
            {
                Categories.Add(new CategoryNode(item));
            }
        }
        private void _AddCategory()
        {
            if (dataProvider.AddCategory(Name, SelectedCategory?.category, out ICategory newCategory))
            {
                var node = new CategoryNode(newCategory);
                if (SelectedCategory == null)
                {
                    Categories.Add(node);
                }
                else
                {
                    SelectedCategory.AddChild(node);
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
