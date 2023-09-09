using Exceptions;

namespace CheckLibrary
{
    public class NotDeposite : BankCheck, IWithdraw<NotDeposite>
    {
        public NotDeposite(string userName) : base(userName)
        {
        }

        public event Action<BankCheck, float> OnWithdraw;

        public NotDeposite Withdraw(float money)
        {
            if(Cash < money)
                throw new NotEnoughMoneyException($"Недостаточно средств: {money - Cash}");
            else
                Cash -= money;

            OnWithdraw?.Invoke(this, money);
            return this;
        }
    }
}