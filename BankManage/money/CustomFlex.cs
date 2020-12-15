using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankManage.common;

namespace BankManage.money
{
    /// <summary>
    /// 零存整取用户类
    /// </summary>
    class CustomFlex:Custom
    {
        int promisedYear = 0;
        double promisedMoney=0;
        /// 开户
        /// </summary>
        /// <param name="accountNumber">帐号</param>
        /// <param name="money">开户金额</param>
        public void Create(string accountNumber, double money,double promisedMoney=0,int promisedYear=1)
        {
            if (promisedYear < 1)
            {
                //TODO 提示
                return;
            }
            base.Create(accountNumber, money);
            this.promisedMoney = promisedMoney;
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
                base.Diposit("零存整取到期利息", DataOperation.GetRate(RateType.零存整取违规) * promisedMoney * 12 * promisedYear);
            }
            else
            {
                if (DateTime.Now.Year - dateTime.Year < promisedYear)
                { }
                else
                {
                    switch (promisedYear)
                    {
                        case 1:
                            base.Diposit("零存整取到期利息", DataOperation.GetRate(RateType.零存整取1年) * promisedMoney * 12 * promisedYear);
                            break;
                        case 3:
                            base.Diposit("零存整取到期利息", DataOperation.GetRate(RateType.零存整取3年) * promisedMoney * 12 * promisedYear);
                            break;
                        case 5:
                            base.Diposit("零存整取到期利息", DataOperation.GetRate(RateType.零存整取5年) * promisedMoney * 12 * promisedYear);
                            break;
                        default:
                            break;
                    }
                    if (DateTime.Now.Year - dateTime.Year > promisedYear)
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
