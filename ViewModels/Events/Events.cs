using Prism.Events;
using ViewModels.Elements;

namespace ViewModels.Events
{
    public class AccountTypeAdded : PubSubEvent<AccTypeItem> { };
    public class AccountTypeDeleted : PubSubEvent<AccTypeItem> { };
    public class AccountAdded : PubSubEvent<AccountItem> { };
    public class AccountDeleted :  PubSubEvent<AccountItem> { };
    public class AccountChanged : PubSubEvent<AccountItem> { };
}
