using Models.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;

namespace ViewModels.Elements
{
    /// <summary>
    /// Container for Category object.
    /// </summary>
    public class CategoryNode
    {
        private string separator = "--";
        internal ICategory category;
        // Child Category Nodes
        public ObservableCollection<CategoryNode> Items { get; }
        public CategoryNode Parent { get; set; }
        public string Title => category.Name;
        public string FullName => string.Format("{0}{1}{2}", category.Parent?.Name, separator, category.Name);

        public CategoryNode(ICategory category)
        {
            this.category = category;
            Items = new ObservableCollection<CategoryNode>();
            foreach (ICategory item in category.Children.OrderBy(c => c.Name))
            {
                AddChild(new CategoryNode(item));
            }
        }
        public void AddChild(CategoryNode node)
        {
            node.Parent = this;
            Items.Add(node);
        }
        public void RemoveChild(CategoryNode node)
        {
            node.Parent = null;
            Items.Remove(node);
        }
        // Equals implementation
        public override bool Equals(object obj)
        {
            if (obj is CategoryNode other)
            {
                return object.Equals(this.Parent, other.Parent) && this.Title.Equals(other.Title);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return $"{this.Parent}{this.Title}".GetHashCode();
        }
    }
}
