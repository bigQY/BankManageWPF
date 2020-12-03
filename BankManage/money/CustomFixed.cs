using BankManage.common;

namespace BankManage
{
    /// <summary>
    /// 定期存款
    /// </summary>
    public class CustomFixed : Custom
    {
        public RateType type { get; set; }
        /// 开户
        /// </summary>
        /// <param name="accountNumber">帐号</param>
        /// <param name="money">开户金额</param>
        public override void Create(string accountNumber, double money)
        {
            base.Create(accountNumber, money);
        }

        /// <summary>
        ///存款 
        /// </summary>
        public override void Diposit(string genType, double money)
        {
            base.Diposit("存款", money);
            //结算利息
            base.Diposit("结息", DataOperation.GetRate(RateType.定期1年) * money);
        }

        /// <summary>
        ///取款 
        /// </summary>
        /// <param name="money">取款金额</param>
        public override void Withdraw(double money)
        {
            if (!ValidBeforeWithdraw(money)) return;
            //计算利息
            double rate = DataOperation.GetRate(type) * AccountBalance;
            //添加利息
            AccountBalance += rate;
            //取款
            base.Withdraw(money);
        }
    }
}
