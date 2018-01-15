using Models.Interfaces;

namespace Models.Elements
{
    internal class BudgetRecord : IBudgetRecord
    {
        public decimal Amount { get; set; }
        public ICategory Category { get; set; }
        public BudgetType Type { get; set; }
        public int OnDay { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Id { get; internal set; }
    }
}
