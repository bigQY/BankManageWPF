using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BankManage.common;

namespace BankManage.money
{
    /// <summary>
    /// 零存整取用户类
    /// </summary>
    class CustomFlex:Custom
    {
        int PromisedYear
        {
            get
            {
                BankEntities bankEntities = new BankEntities();
                var q = from t in bankEntities.AccountFlex
                        where t.accountNo == AccountInfo.accountNo
                        select t.promisedYear;
                if (q.Count() != 0)
                {
                    return q.First();
                }
                else
                {
                    return -1;
                }
            }
            set
            {

            }
        }
        double promisedMoney
        {
            get
            {
                BankEntities bankEntities = new BankEntities();
                var q = from t in bankEntities.AccountFlex
                        where t.accountNo == AccountInfo.accountNo
                        select t.promisedMoney;
                if (q.Count() != 0)
                {
                    return q.First();
                }
                else
                {
                    return -1;
                }
            }
            set
            {

            }
        }

        RateType RType { get; set; }
        /// 开户
        /// </summary>
        /// <param name="accountNumber">帐号</param>
        /// <param name="money">开户金额</param>
        public void Create(string accountNumber, double money, double promisedMoney = 5, int promisedYear = 1)
        {
            if (promisedYear < 1)
            {
                MessageBox.Show("存款年限太短");
            }
            base.Create(accountNumber, money);

            //零存整取

            AccountFlex account = new AccountFlex();
            account.promisedYear = promisedYear;
            account.accountNo = AccountInfo.accountNo;
            account.promisedMoney = money;
            try
            {
                context.AccountFlex.Add(account);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            switch (promisedYear)
            {
                case 1:
                    RType = RateType.零存整取1年;
                    break;
                case 3:
                    RType = RateType.零存整取3年;
                    break;
                case 5:
                    RType = RateType.零存整取5年;
                    break;
                default:
                    MessageBox.Show("非法的年份");
                    break;
            }
            
        }

        /// <summary>
        ///存款 
        /// </summary>
        public void Diposit(string genType)
        {
            
            if (isBroken())
            {
                base.Diposit("约定存款违规", promisedMoney);
            }
            else
            {
                base.Diposit("约定存款", promisedMoney);
            }

            
        }

        //TODO 违规


        /// <summary>
        ///取款 
        /// </summary>
        /// 
        public void Withdraw()
        {
            DateTime dateTime = DataOperation.GetCreateAutomaticWithdrawalTime(this);

            if (DataOperation.GetCustomerIsBroken(this))
            {
                base.Diposit("零存整取到期利息", DataOperation.GetRate(RateType.零存整取违规) * promisedMoney * 12 * PromisedYear);
            }
            else
            {
                if (DateTime.Now.Year - dateTime.Year < PromisedYear)
                { }
                else
                {
                    switch (PromisedYear)
                    {
                        case 1:
                            base.Diposit("零存整取到期利息", DataOperation.GetRate(RateType.零存整取1年) * promisedMoney * 12 * PromisedYear);
                            break;
                        case 3:
                            base.Diposit("零存整取到期利息", DataOperation.GetRate(RateType.零存整取3年) * promisedMoney * 12 * PromisedYear);
                            break;
                        case 5:
                            base.Diposit("零存整取到期利息", DataOperation.GetRate(RateType.零存整取5年) * promisedMoney * 12 * PromisedYear);
                            break;
                        default:
                            break;
                    }
                    if (DateTime.Now.Year - dateTime.Year > PromisedYear)
                    {
                        base.Diposit("零存整取超期利息", DataOperation.GetRate(RateType.零存整取超期部分) * AccountBalance);
                    }
                }
            }

            
            //取款
            base.Withdraw(AccountBalance);
        }

        private bool isBroken()
        {
            if(DateTime.Now.Month - DataOperation.GetLastAutomaticWithdrawalTime(this).Month >= 2)
            {
                return true;
            }
            return false;
        }

    }
}
