using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using BankManage.common;

namespace BankManage
{
    /// <summary>
    /// 定期存款
    /// </summary>
    public class CustomFixed : Custom
    {
        public int PromisedYear;
        public RateType RType { get; set; }
        /// 开户
        /// </summary>
        /// <param name="accountNumber">帐号</param>
        /// <param name="money">开户金额</param>
        public void Create(string accountNumber, double money, int promisedYear, string accountType= "定期存款")
        {
            this.PromisedYear = promisedYear;
            base.Create(accountNumber, money,accountType);
            //定期存款
            if (accountType == "定期存款")
            {
                AccountFixed account = new AccountFixed();
                account.promisedYear = PromisedYear;
                account.accountNo = AccountInfo.accountNo;
                try {
                    context.AccountFixed.Add(account);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            switch (promisedYear)
            {
                case 1:
                    RType = RateType.定期1年;
                    break;
                case 3:
                    RType = RateType.定期3年;
                    break;
                case 5:
                    RType = RateType.定期5年;
                    break;
                default:
                    MessageBox.Show("非法的年份");
                    break;
            }
        }

        /// <summary>
        ///存款 
        /// </summary>
        public override void Diposit(string genType, double money)
        {
            BankEntities bankEntities = new BankEntities();
            var q1 = from t in bankEntities.MoneyInfo
                    where t.accountNo == this.AccountInfo.accountNo
                    select t;
            var q2 = from i in bankEntities.MoneyInfo
                     where i.dealType == "定期存款到期支取"
                     select i;
            if (q1.Count() > 0)
            {
                if (q2.Count() == 0)
                {
                    MessageBox.Show("您还有一个未支取的定期存款");
                    return;
                }
            }
            base.Diposit("存款", money);
            //结算利息
            base.Diposit("结息", DataOperation.GetRate(RType) * money);
        }

        /// <summary>
        ///取款 
        /// </summary>
        /// <param name="money">取款金额</param>
        public override void Withdraw(double money)
        {
            InsertData("定期存款到期支取", -money);
        }
    }
}
