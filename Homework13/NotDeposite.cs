using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Homework13
{
    internal class NotDeposite : BankCheck, IWithdraw<NotDeposite>
    {
        public NotDeposite(string userName) : base(userName)
        {
        }

        public event Action<BankCheck, float> OnWithdraw;

        public NotDeposite Withdraw(float money)
        {
            Cash -= money;
            OnWithdraw?.Invoke(this, money);
            return this;
        }
    }
}