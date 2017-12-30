
namespace ViewModels.Elements
{
    /// <summary>
    /// Simple container for storing Account type object(string).
    /// </summary>
    public class AccTypeItem
    {
        public string Name { get; }

        public AccTypeItem(string name)
        {
            Name = name;
        }
        // Equals implementation
        public override bool Equals(object obj)
        {
            if (obj is AccTypeItem other)
            {
                return this.Name.Equals(other.Name);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
