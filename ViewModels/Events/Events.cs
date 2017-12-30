using Prism.Events;
using ViewModels.Elements;

namespace ViewModels.Events
{
    public class AccountTypeAdded : PubSubEvent<AccTypeItem> { };
    public class AccountTypeDeleted : PubSubEvent<AccTypeItem> { };
}
