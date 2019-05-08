using Models.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using ViewModels.Interfaces;

namespace ViewModels.Elements
{
    /// <summary>
    /// Container for Category object.
    /// </summary>
    public class CategoryNode : ICategoryNode
    {
        private readonly string separator = "--";
        public ICategory InnerCategory { get; private set; }
        // Child Category Nodes
        public ObservableCollection<ICategoryNode> Items { get; }
        public ICategoryNode Parent { get; set; }
        public string Title => InnerCategory.Name;
        public string FullName => string.Format("{0}{1}{2}", InnerCategory.Parent?.Name, separator, InnerCategory.Name);

        public CategoryNode(ICategory category)
        {
            InnerCategory = category;
            Items = new ObservableCollection<ICategoryNode>();
            foreach (ICategory item in category.Children.OrderBy(c => c.Name))
            {
                AddChild(new CategoryNode(item));
            }
        }
        public void AddChild(ICategoryNode node)
        {
            node.Parent = this;
            Items.Add(node);
        }
        public void RemoveChild(ICategoryNode node)
        {
            node.Parent = null;
            Items.Remove(node);
        }
        // Equals implementation
        public override bool Equals(object obj)
        {
            if (obj is ICategoryNode other)
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
