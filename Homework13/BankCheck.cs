using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Homework13
{
    internal class BankCheck : INotifyPropertyChanged, ITransfer<BankCheck>
    {
        /// <summary>
        /// Глобальный счетчик ID
        /// </summary>
        static private int ID = 0;

        /// <summary>
        /// Текущий ID
        /// </summary>
        private int currentID;
        public int CurrentID
        {
            get
            {
                return currentID;
            }
            private set
            {
                currentID = value;
                OnPropertyChanged("CurrentID");
            }
        }

        /// <summary>
        /// Метод получения текущего ID
        /// </summary>
        public string GetID
        {
            get
            {
                string s = String.Format("{0:d16}", CurrentID);
                return s;
            }
        }

        /// <summary>
        /// Имя пользователя счета
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Деньги на счету
        /// </summary>
        private float cash;
        public float Cash
        {
            get
            {
                return cash;
            }
            set
            {
                cash = value;
                OnPropertyChanged("Cash");
            }
        }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        public BankCheck(string userName)
        {
            UserName = userName;
            currentID = ID;
            Cash = 10000;
            BankCheck.ID++;
        }

        /// <summary>
        /// MVVM
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<BankCheck, BankCheck, float> OnTransaction;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Метод перевода денег между счетами
        /// </summary>
        /// <param name="t1">Куда переводим</param>
        /// <param name="cash">Сумма</param>
        public void Transfer(BankCheck t1, float cash)
        {
            t1.Cash += cash;
            this.Cash -= cash;
            OnTransaction?.Invoke(this, t1, cash);
        }
    }
}