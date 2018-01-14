using Models.Interfaces;

namespace Models.Elements
{
    // TODO Bindable? Test
    internal class Account : IAccount
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public bool Closed { get; set; }
        public bool Excluded { get; set; }
        public int Id { get; internal set; }
    }
}
