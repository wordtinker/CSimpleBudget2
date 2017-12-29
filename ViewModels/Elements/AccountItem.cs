using Models.Interfaces;

namespace ViewModels
{
    public interface IAccountItem
    {
        string Name { get; }
        string Type { get; }
        decimal Balance { get; }

    }
    // TODO
    public class AccountItem : IAccountItem
    {
        private IAccount account;

        public string Name => account.Name;
        // TODO
        public string Type => "stub";
        //public string Type
        //{
        //    get { return account.Type; }
        //    set
        //    {
        //        account.Type = value;
        //        Core.Instance.UpdateAccount(account);
        //    }
        //}
        public decimal Balance => account.Balance;

        public AccountItem(IAccount acc)
        {
            this.account = acc;
        }
    }
    public class AccountAggregate : IAccountItem
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
    }
}
