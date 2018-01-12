using Models.Interfaces;
using System;
using System.Globalization;

namespace ViewModels.Elements
{
    /// <summary>
    /// Container for Spending object.
    /// </summary>
    public class BudgetBar
    {
        private ISpending spending;
        private CategoryNode category;

        // ctor
        public BudgetBar(ISpending spending)
        {
            this.spending = spending;
            this.category = new CategoryNode(spending.Category);
        }

        public string Name => category.FullName;
        public int Month => spending.Month;
        public string MonthName => DateTimeFormatInfo.CurrentInfo.MonthNames[spending.Month - 1];
        /// <summary>
        /// If overspent occured, spent is planned 
        /// budget.
        /// if not - spent is actual sum of transactions.
        /// </summary>
        public decimal Spent => spending.Value - Overspent;
        /// <summary>
        /// If overspent occured, sum to spend is 0m.
        /// If not - to spend is difference between budget and
        /// sum of transactions.
        /// </summary>
        public decimal ToSpend => spending.Budget - Spent;
        /// <summary>
        /// If sum of transactions is greater than budget
        /// overspent occurs.
        /// If not - it is 0m.
        /// </summary>
        public decimal Overspent => Math.Max(0m, spending.Value - spending.Budget);
    }
}
