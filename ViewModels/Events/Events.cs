using Prism.Events;
using ViewModels.Elements;

namespace ViewModels.Events
{
    public class TransactionChange
    {
        public TransactionItem Old { get; set; }
        public TransactionItem New { get; set; }
    }
    public class BudgetRecordChange
    {
        public RecordItem Old { get; set; }
        public RecordItem New { get; set; }
    }

    public class TransactionAdded : PubSubEvent<TransactionItem> { };
    public class TransactionDeleted : PubSubEvent<TransactionItem> { };
    public class TransactionChanged : PubSubEvent<TransactionChange> { };

    public class BudgetRecordAdded : PubSubEvent<RecordItem> { };
    public class BudgetRecordDeleted : PubSubEvent<RecordItem> { };
    public class BudgetRecordChanged : PubSubEvent<BudgetRecordChange> { };
}
