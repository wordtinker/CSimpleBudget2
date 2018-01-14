﻿using Prism.Events;
using ViewModels.Elements;

namespace ViewModels.Events
{
    public class TransactionAdded : PubSubEvent<TransactionItem> { };
    public class TransactionDeleted : PubSubEvent<TransactionItem> { };
    public class TransactionChanged : PubSubEvent<TransactionItem> { };

    public class BudgetRecordAdded : PubSubEvent<RecordItem> { };
    public class BudgetRecordDeleted : PubSubEvent<RecordItem> { };
    public class BudgetRecordChanged : PubSubEvent<RecordItem> { };
}
