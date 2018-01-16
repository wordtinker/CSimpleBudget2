using Models.Interfaces;

namespace Models.Elements
{
    /// <summary>
    /// Object that contains sum of all transaction
    /// and sum of all budget records for a given
    /// period(month and year) and category.
    /// </summary>
    internal class Spending : ISpending
    {
        // Category of the spending
        public ICategory Category { get; internal set; }
        // Sum of the planned budget records.
        public decimal Budget { get; internal set; }
        // Sum of the transactions.
        public decimal Value { get; internal set; }
        // Month of the spending
        public int Month { get; internal set; }
    }
}
