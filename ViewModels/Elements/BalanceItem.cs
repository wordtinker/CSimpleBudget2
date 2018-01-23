using System;

namespace ViewModels.Elements
{
    public class BalanceItem
    {
        public DateTime Date { get; set; }
        public decimal Change { get; set; }
        public decimal Total { get; set; }
        public bool IsNegative => Total < 0;
        public string Origin { get; set; }
        public CategoryNode Category { get; set; }
    }
}
