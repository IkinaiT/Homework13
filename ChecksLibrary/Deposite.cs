using Exceptions;

namespace CheckLibrary
{
    public class Deposite : BankCheck, IWithdraw<Deposite>
    {
        public Deposite(string userName) : base(userName)
        {
        }

        public event Action<BankCheck, float> OnWithdraw;

        public Deposite Withdraw(float money)
        {
            if (Cash < money)
                throw new NotEnoughMoneyException($"Недостаточно средств: {money - Cash}");
            else
                Cash -= money;

            OnWithdraw?.Invoke(this, money);
            return this;
        }
    }
}