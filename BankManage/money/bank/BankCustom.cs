using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BankManage.money.bank
{
    public class BankCustom
    {
        private BankEntities bankEntities = new BankEntities();
        private AccountInfo account;
        public BankCustom()
        {
            account = new AccountInfo();
        }

        public BankCustom(AccountInfo accountInfo)
        {
            account = accountInfo;
        }

        public BankCustom(string accountNo)
        {

        }

        public void create(AccountInfo accountInfo)
        {
            bool success;
            account = accountInfo;
            try
            {
                bankEntities.AccountInfo.Add(accountInfo);
                bankEntities.SaveChanges();
                success = true;
            }
            catch (Exception err)
            {
                success = false;
                MessageBox.Show("开户失败！" + err.Message);
            }
            if (success)
            {
                InsertData(account.accountType + "开户", accountInfo.accountBalance);
            }
        }

        public void InsertData(string genType, double money)
        {
            MoneyInfo newMoneyInfo = new MoneyInfo();
            newMoneyInfo.accountNo = account.accountNo;
            newMoneyInfo.dealDate = DateTime.Now;
            newMoneyInfo.dealType = genType;
            newMoneyInfo.dealMoney = money;
            newMoneyInfo.balance = account.accountBalance + money;
            try
            {
               bankEntities.AccountInfo.Find(account.accountNo).accountBalance+=money;
               bankEntities.MoneyInfo.Add(newMoneyInfo);
               bankEntities.SaveChanges();
            }
            catch
            {
                MessageBox.Show("添加交易记录失败：");
            }

        }

    }
}
