using Models.Interfaces;
using System.Collections.Generic;

namespace Models.Elements
{
    // TODO Bindable ?
    internal class Category : ICategory
    {
        private List<ICategory> children;

        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public ICategory Parent { get; internal set; }
        public IEnumerable<ICategory> Children => children;

        public Category()
        {
            children = new List<ICategory>();
        }
        internal void AddChild(ICategory child)
        {
            children.Add(child);
        }
        internal void RemoveChild(ICategory child)
        {
            children.Remove(child);
        }
    }
}
