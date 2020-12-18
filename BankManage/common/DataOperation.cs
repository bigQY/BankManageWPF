using System;
using System.Linq;
using BankManage.money;

namespace BankManage.common
{
    public class DataOperation
    {
        public static readonly string[] AccountType = { "活期存款", "定期存款", "零存整取" };
        /// <summary>
        /// 获取操作员姓名
        /// </summary>
        /// <param name="id">操作员编号</param>
        /// <returns></returns>
        public static string GetOperateName(string id)
        {
            using (BankEntities c = new BankEntities())
            {
                var q = from t in c.EmployeeInfo
                        where t.EmployeeNo == id
                        select t;
                if (q != null && q.Count() >= 1)
                {
                    return q.First().EmployeeName;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 根据存款类型创建存款用户
        /// </summary>
        /// <param name="accountType">存款类型</param>
        /// <returns></returns>
        public static Custom CreateCustom(string accountType)
        {
            Custom custom = null;
            switch (accountType)
            {
                case "活期存款":
                    custom = new CustomChecking();
                    break;
                case "定期存款":
                    custom = new CustomFixed();
                    break;
                case "零存整取":
                    custom = new CustomFlex();
                    break;
            }
            custom.AccountInfo.accountType = accountType;
            return custom;
        }

        /// <summary>
        /// 获取存款用户信息,并初始化余额
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public static Custom GetCustom(string accountNumber)
        {
            Custom custom = null;
            BankEntities c = new BankEntities();
            try
            {
                var query = from t in c.AccountInfo
                            where t.accountNo == accountNumber
                            select t;
                if (query.Count() > 0)
                {
                    var q = query.Single();
                    custom = CreateCustom(q.accountType);
                    custom.AccountInfo.accountNo = accountNumber;
                    custom.AccountInfo.accountName = q.accountName;
                    custom.AccountInfo.accountPass = q.accountPass;
                    custom.AccountInfo.IdCard = q.IdCard;
                }
            }
            catch
            {
                return null;
            }
            var qt = from t in c.MoneyInfo
                     where t.accountNo == accountNumber
                     select t;
            if (qt != null && qt.Count() > 0)
            {
                custom.AccountBalance = qt.Sum(x => x.dealMoney);
            }
            return custom;
        }

        /// <summary>
        /// 获取指定类别的利率
        /// </summary>
        /// <param name="rateType">利率类别</param>
        /// <returns>对应类别的利率值</returns>
        public static double GetRate(RateType rateType)
        {
            string type = rateType.ToString();
            BankEntities c = new BankEntities();
            var q = (from t in c.RateInfo
                     where t.rationType == type
                     select t.rationValue).Single();
            return q.Value;
        }
        /// <summary>
        /// 获得账户上一次交易发生的时间
        /// </summary>
        /// <param name="custom">所查询的账户</param>
        /// <returns>上一次交易发生的时间</returns>
        public static DateTime GetLastAutomaticWithdrawalTime(Custom custom)
        {
            BankEntities c = new BankEntities();
            var q = from t in c.MoneyInfo
                    where t.accountNo == custom.AccountInfo.accountNo
                    select t.dealDate;
            return q.First().Date;
        }
        /// <summary>
        /// 获取账户的开户时间
        /// </summary>
        /// <param name="custom">所查询的账户</param>
        /// <returns>获取账户的开户时间</returns>
        public static DateTime GetCreateAutomaticWithdrawalTime(Custom custom)
        {
            BankEntities c = new BankEntities();
            var q = from t in c.MoneyInfo
                    where t.accountNo == custom.AccountInfo.accountNo
                    where t.dealType=="开户"
                    select t.dealDate;
            return q.First().Date;
        }
        /// <summary>
        /// 判断零存整取账户是否违规过
        /// </summary>
        /// <param name="custom">所查询的账户</param>
        /// <returns>账户是否违规过</returns>
        public static bool GetCustomerIsBroken(Custom custom)
        {
            BankEntities c = new BankEntities();
            var q = from t in c.MoneyInfo
                    where t.accountNo == custom.AccountInfo.accountNo
                    where t.dealType == "零存整取违规"
                    select t;
            if (q.Count() > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取全部员工信息
        /// </summary>
        /// <returns></returns>
        public static IQueryable<EmployeeInfo> GetEmployeeInfos()
        {
            BankEntities context = new BankEntities();
            var query = from t in context.EmployeeInfo
                        select t;
            return query;
        }
        /// <summary>
        /// 检查是否以存在同样AccountNo的账户
        /// </summary>
        /// <param name="accountNo">所检查的账户AccountNo</param>
        /// <returns></returns>
        public static bool CheckAccountNo(string accountNo)
        {
            BankEntities c = new BankEntities();
            var q = from t in c.AccountInfo
                    where t.accountNo == accountNo
                    select t;
            if (q.Count() != 0)
            {
                return false;
            }
            return true;
        }

    }
}
