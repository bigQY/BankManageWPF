using System;
using System.Linq;
using System.Windows;

namespace BankManage
{
    /// <summary>
    /// 保存客户发生的最新业务信息
    /// </summary>
    public class Custom
    {
        /// <summary>
        /// 存款客户的帐户基本信息
        /// </summary>
        public AccountInfo AccountInfo { get; set; }
        /// <summary>
        /// 存款发生信息
        /// </summary>
        public MoneyInfo MoneyInfo { 
            get 
            {
                if (AccountInfo != null)
                {
                    BankEntities c = new BankEntities();
                    var q = from t in c.MoneyInfo
                            where t.accountNo == AccountInfo.accountNo
                            select t;
                    return q.First();
                }
                else
                {
                    return null;
                }
            }
            set { }
        }
        /// <summary>
        /// 帐户余额(不可设置，只能从数据库读入)
        /// </summary>
        public double AccountBalance {
            get
            {
                BankEntities bankEntities = new BankEntities();
                var q = from t in bankEntities.MoneyInfo
                        where t.accountNo == AccountInfo.accountNo
                        select t.balance;
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
        public Custom()
        {
            AccountInfo = new AccountInfo();
        }

        protected BankEntities context = new BankEntities();
        /// 开户
        /// </summary>
        /// <param name="accountNumber">帐号</param>
        /// <param name="money">开户金额</param>
        public virtual void Create(string accountNumber, double money,string accountType="活期存款")
        {
            bool success = false;
            //插入到数据库
            try
            {
                context.AccountInfo.Add(AccountInfo);
                context.SaveChanges();
                success = true;
            }
            catch (Exception err)
            {
                success = false;
                MessageBox.Show("开户失败！" + err.Message);
            }
            if (success)
            {
                this.AccountInfo.accountType = accountType;
                InsertData(accountType+"开户", money);
            }
            

        }

        /// <summary>
        ///存款 
        /// </summary>
        /// <param name="genType">类型</param>
        /// <param name="money">金额</param>
        public virtual void Diposit(string genType, double money)
        {
            if (money <= 0)
            {
                throw new Exception("存款金额不能为零或负值");
            }
            else
            {
                //修改余额
                InsertData(genType, money);
            }
        }

        /// <summary>
        ///检查是否允许取款，允许返回true，否则返回false
        /// </summary>
        /// <param name="money">取款金额</param>
        public bool ValidBeforeWithdraw(double money)
        {
            if (money <= 0)
            {
                MessageBox.Show("取款金额不能为零或负值");
                return false;
            }
            if (money > AccountBalance)
            {
                MessageBox.Show("取款数不能比余额大");
                return false;
            }
            return true;
        }

        /// <summary>
        ///取款 
        /// </summary>
        /// <param name="money">取款金额</param>
        public virtual void Withdraw(double money)
        {
            InsertData("取款", -money);
        }

        /// <summary>
        /// 在表中添加新记录
        /// </summary>
        /// <param name="genType">发生类别</param>
        /// <param name="money">发生金额</param>
        public void InsertData(string genType, double money)
        {
            MoneyInfo newMoneyInfo = new MoneyInfo();
            newMoneyInfo.accountNo= AccountInfo.accountNo; 
            newMoneyInfo.dealDate = DateTime.Now;
            newMoneyInfo.dealType = genType;
            newMoneyInfo.dealMoney = money;
            newMoneyInfo.balance = MoneyInfo.balance+money;
            /*MoneyInfo.accountNo = this.AccountInfo.accountNo;
            MoneyInfo.dealDate = DateTime.Now;
            MoneyInfo.dealType = genType;
            MoneyInfo.dealMoney = money;
            MoneyInfo.balance += money;*/

            try
            {
                context.MoneyInfo.Add(newMoneyInfo);
                context.SaveChanges();
            }
            catch
            {
                MessageBox.Show("添加交易记录失败：");
            }

        }
    }
}
