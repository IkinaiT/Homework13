using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework13
{
    internal interface ITransfer<in T>
    {
        void Transfer(T t1, float cash);

        public event Action<BankCheck, BankCheck, float> OnTransaction;
    }
}