using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework13
{
    internal class Deposite : BankCheck, IWithdraw<Deposite>
    {
        public Deposite(string userName) : base(userName)
        {
        }

        public event Action<BankCheck, float> OnWithdraw;

        public Deposite Withdraw(float money)
        {
            Cash -= money;
            OnWithdraw?.Invoke(this, money);
            return this;
        }
    }
}