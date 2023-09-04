using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Homework13
{


    internal class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Коллекция всех счетов
        /// </summary>
        public ObservableCollection<BankCheck> BankChecks { get; set; }

        public ObservableCollection<string> LogList { get; set; }

        private event Action<BankCheck> OnCreateUser;
        private event Action<BankCheck> OnDeleteUser;

        /// <summary>
        /// Выбранный в ListBox'е пользователь
        /// </summary>
        private BankCheck selectedCheck;
        public BankCheck SelectedCheck
        {
            get { return selectedCheck; }
            set
            {
                selectedCheck = value;
                OnPropertyChanged("SelectedCheck");
            }
        }

        /// <summary>
        /// Выбранный в ComboBox'е пользователь
        /// </summary>
        private BankCheck selectedCheckTransaction;
        public BankCheck SelectedCheckTransaction
        {
            get { return selectedCheckTransaction; }
            set
            {
                selectedCheckTransaction = value;
                OnPropertyChanged("SelectedCheckTransaction");
            }
        }

        /// <summary>
        /// Кол-во переволимых/снимаемых денег
        /// </summary>
        private float _cash;
        public float Cash
        {
            get { return _cash; }
            set
            {
                _cash = value;
                CanTransaction = SelectedCheck.Cash > Cash && SelectedCheck != null && SelectedCheckTransaction != null;
                OnPropertyChanged("Cash");
            }
        }

        /// <summary>
        /// Возможно ли совершить перевод
        /// </summary>
        private bool canTransaction;
        public bool CanTransaction
        {
            get { return canTransaction; }
            set
            {
                canTransaction = value;
                OnPropertyChanged("CanTransaction");
            }
        }

        /// <summary>
        /// Итератор заполнения коллекции
        /// </summary>
        private int i = 0;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainViewModel()
        {
            OnCreateUser += OnCreate;
            OnDeleteUser += OnDelete;
            

            BankChecks = new ObservableCollection<BankCheck>();
            LogList = new ObservableCollection<string>();
            for (; i < 10; i++)
            {
                AddUser();
            }
        }

        /// <summary>
        /// Команда добавления счета
        /// </summary>
        private RelayCommand addChechCommand;
        public RelayCommand AddCheckCommand
        {
            get
            {
                return addChechCommand ??
                    (addChechCommand = new RelayCommand(obj =>
                    {
                        AddUser();
                        i++;
                    }));
            }
        }

        /// <summary>
        /// Команда удаления счета
        /// </summary>
        private RelayCommand deleteCheckCommand;
        public RelayCommand DeleteCheckCommand
        {
            get
            {
                return deleteCheckCommand ??
                    (deleteCheckCommand = new RelayCommand(
                        obj =>
                        {
                            OnDeleteUser?.Invoke(SelectedCheck);
                            BankChecks.Remove(SelectedCheck);
                        },
                        obj => SelectedCheck != null));
            }
        }

        /// <summary>
        /// Команда перевода между счетами
        /// </summary>
        private RelayCommand transactionCommand;
        public RelayCommand TransactionCommand
        {
            get
            {
                return transactionCommand ??
                    (transactionCommand = new RelayCommand(obj =>
                    {
                        ITransfer<BankCheck> t;
                        ITransfer<Deposite> td = SelectedCheck as Deposite;
                        ITransfer<NotDeposite> tnd = SelectedCheck as NotDeposite;
                        if (td != null)
                            t = td as BankCheck;
                        else
                            t = tnd as BankCheck;

                        t.Transfer(SelectedCheckTransaction as BankCheck, Cash);

                    },
                    obj => SelectedCheck != null &&
                           SelectedCheckTransaction != null &&
                           SelectedCheck != SelectedCheckTransaction &&
                           Cash > 0));
            }
        }

        /// <summary>
        /// Команда снятия денег со счета
        /// </summary>
        private RelayCommand withdrawChechCommand;
        public RelayCommand WithdrawCommand
        {
            get
            {
                return withdrawChechCommand ??
                    (withdrawChechCommand = new RelayCommand(obj =>
                    {
                        IWithdraw<Deposite> scd = SelectedCheck as Deposite;
                        IWithdraw<NotDeposite> scn = SelectedCheck as NotDeposite;
                        IWithdraw<BankCheck> withdraw;

                        if (scn != null)
                            withdraw = scn;
                        else
                            withdraw = scd;

                        withdraw.Withdraw(Cash);
                    }));
            }
        }

        /// <summary>
        /// MVVM 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Метод добавления пользователя
        /// </summary>
        private void AddUser()
        {
            if (i % 2 != 0)
                BankChecks.Add(new NotDeposite("NDUsername" + i));
            else
                BankChecks.Add(new Deposite("DUsername" + i));

            if (BankChecks[i] is IWithdraw<BankCheck> bc)
                bc.OnWithdraw += OnWidhdraw;

            BankChecks[i].OnTransaction += OnTransaction;
            OnCreateUser?.Invoke(BankChecks[i]);
        }

        /// <summary>
        /// Лог при снятии денег
        /// </summary>
        /// <param name="user">Кто снял</param>
        /// <param name="cash">Сколько снял</param>
        private void OnWidhdraw(BankCheck user, float cash)
        {
            LogList.Add($"{user.UserName} снимает {cash} у.е.");
        }

        /// <summary>
        /// Лог при переводе денег
        /// </summary>
        /// <param name="from">Кто перевел</param>
        /// <param name="to">Кому перевел</param>
        /// <param name="cash">Сколько перевел</param>
        private void OnTransaction(BankCheck from, BankCheck to, float cash)
        {
            LogList.Add($"{from.UserName} переводит {to.UserName} {cash} у.е.");
        }

        private void OnCreate(BankCheck user)
        {
            LogList.Add($"Пользователь {user.UserName} создан");
        }

        private void OnDelete(BankCheck user)
        {
            LogList.Add($"Пользователь {user.UserName} удален");
        }
    }
}