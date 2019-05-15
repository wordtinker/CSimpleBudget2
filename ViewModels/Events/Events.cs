using Prism.Events;
using ViewModels.Interfaces;

namespace ViewModels.Events
{
    public class TransactionChange
    {
        public ITransactionItem Old { get; set; }
        public ITransactionItem New { get; set; }
    }
    public class BudgetRecordChange
    {
        public IRecordItem Old { get; set; }
        public IRecordItem New { get; set; }
    }

    public class TransactionAdded : PubSubEvent<ITransactionItem> { };
    public class TransactionDeleted : PubSubEvent<ITransactionItem> { };
    public class TransactionChanged : PubSubEvent<TransactionChange> { };

    public class BudgetRecordAdded : PubSubEvent<IRecordItem> { };
    public class BudgetRecordDeleted : PubSubEvent<IRecordItem> { };
    public class BudgetRecordChanged : PubSubEvent<BudgetRecordChange> { };
}
