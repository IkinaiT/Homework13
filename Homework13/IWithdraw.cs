using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework13
{
    internal interface IWithdraw<out T>
        where T : BankCheck
    {
        T Withdraw(float money);

        public event Action<BankCheck, float> OnWithdraw;
    }
}