using Models.Interfaces;
using Prism.Events;

namespace ViewModels.Elements
{
    public interface IAccountItem
    {
        string Name { get; }
        string Type { get; set; }
        decimal Balance { get; }
        bool Closed { get; set; }
        bool Excluded { get; set; }

    }
    public class AccountItem : IAccountItem
    {
        private IDataProvider dataProvider;
        private IEventAggregator eventAggregator;
        internal IAccount account;

        public string Name => account.Name;
        public decimal Balance => account.Balance;
        public string Type
        {
            get => account.Type;
            set
            {
                account.Type = value;
                dataProvider.UpdateAccount(account);
            }
        }
        public bool Closed
        {
            get => account.Closed;
            set
            {
                account.Closed = value;
                dataProvider.UpdateAccount(account);
            }
        }
        public bool Excluded
        {
            get => account.Excluded;
            set
            {
                account.Excluded = value;
                dataProvider.UpdateAccount(account);
            }
        }
        public AccountItem(IAccount acc, IDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            this.account = acc;
            this.dataProvider = dataProvider;
            this.eventAggregator = eventAggregator;
        }
        // Equals implementation
        public override bool Equals(object obj)
        {
            if (obj is AccountItem other)
            {
                return this.account.Id.Equals(other.account.Id);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return this.account.Id.GetHashCode();
        }
    }
    /// <summary>
    /// Virtual accounts have no real representation
    /// in the core(DB) and are used for aggregating data
    /// of several real accounts.
    /// </summary>
    public class AccountAggregate : IAccountItem
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public bool Closed { get; set; }
        public bool Excluded { get; set; }
    }
}
