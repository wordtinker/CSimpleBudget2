﻿using Models.Interfaces;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using ViewModels.Elements;
using ViewModels.Interfaces;

namespace ViewModels.Windows
{
    // TODO Later validation
    public class CategoriesManagerViewModel
    {
        private IUIBaseService serviceProvider;
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;

        public ObservableCollection<CategoryNode> Categories { get; }

        public CategoriesManagerViewModel(IUIBaseService serviceProvider, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.serviceProvider = serviceProvider;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;

            Categories = new ObservableCollection<CategoryNode>();
            foreach (ICategory item in dataProvider.GetCategories())
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
                    parent.Items.Add(node);
                }
                // TODO events?
            }
            else
            {
                serviceProvider.ShowMessage("Can't add category.");
            }
        }
        public void DeleteCategory(CategoryNode node)
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
                // TODO events?
            }
            else
            {
                serviceProvider.ShowMessage("Can't delete category.");
            }
        }
    }
}
