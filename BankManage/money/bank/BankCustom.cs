using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BankManage.common;

namespace BankManage.money.bank
{
    public class BankCustom
    {
        public BankEntities bankEntities = new BankEntities();
        public AccountInfo account;
        public BankCustom()
        {
            //account = new AccountInfo();
        }

        public BankCustom(AccountInfo accountInfo)
        {
            account = accountInfo;
            if (account == null)
            {
                MessageBox.Show("账号不存在");
            }
        }

        public BankCustom(string accountNo)
        {
            account = bankEntities.AccountInfo.Find(accountNo);
            if (account == null)
            {
                MessageBox.Show("账号不存在");
            }
        }

        public void create(AccountInfo accountInfo,int promiseYear=0,double primoiseMoney=0)
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
                MoneyInfo newMoneyInfo = new MoneyInfo();
                newMoneyInfo.accountNo = account.accountNo;
                newMoneyInfo.dealDate = DateTime.Now;
                newMoneyInfo.dealType = account.accountType+"开户";
                newMoneyInfo.dealMoney = account.accountBalance;
                newMoneyInfo.balance = account.accountBalance;
                try
                {
                    account = bankEntities.AccountInfo.Find(account.accountNo);
                    bankEntities.MoneyInfo.Add(newMoneyInfo);
                    bankEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("开户成功,但添加交易记录失败：");
                }
            }

            switch (account.accountType)
            {
                case "定期存款":
                    AccountFixed accountFixd = new AccountFixed();
                    accountFixd.promisedYear = promiseYear;
                    accountFixd.accountNo = account.accountNo;
                    try
                    {
                        bankEntities.AccountFixed.Add(accountFixd);
                        bankEntities.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    break;
                case "零存整取":
                    AccountFlex accountFlex = new AccountFlex();
                    accountFlex.accountNo = account.accountNo;
                    accountFlex.promisedYear = promiseYear;
                    accountFlex.promisedMoney = primoiseMoney;
                    try
                    {
                        bankEntities.AccountFlex.Add(accountFlex);
                        bankEntities.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    break;
            }

        }

        public void deposit(string dealType,double money)
        {
            switch (account.accountType)
            {
                case "活期存款":
                    InsertData(dealType, money);
                    break;
                case "定期存款":
                    if (account.accountBalance != 0)
                    {
                        MessageBox.Show("您还有未支取的定期存款，暂时无法再次存款");
                    }
                    else
                    {
                        InsertData(dealType, money);
                    }
                    break;
                case "零存整取":
                    //TODO timespan
                    if (DateTime.Now.Month - DataOperation.GetLastAutomaticWithdrawalTime(account.accountNo).Month >= 2)
                    {
                        InsertData("零存整取违规", account.AccountFlex.promisedMoney);
                    }
                    else
                    {
                        InsertData("零存整取第" + (DateTime.Now.Month - DataOperation.GetLastAutomaticWithdrawalTime(account.accountNo).Month) + "个月存款", account.AccountFlex.promisedMoney);
                    }
                    break;
                    
            }
            
        }

        public void withdraw(string dealType,double money)
        {
            if (money >= account.accountBalance)
                InsertData(dealType, -money);
            else
                MessageBox.Show("余额不足");
        }

        public void destory()
        {
            //TODO
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
                account = bankEntities.AccountInfo.Find(account.accountNo);
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
