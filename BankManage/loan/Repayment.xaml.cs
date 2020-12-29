using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BankManage.common;
using BankManage.money.bank;

namespace BankManage.loan
{
    /// <summary>
    /// Repayment.xaml 的交互逻辑
    /// </summary>
    public partial class Repayment : Page
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        RateType rate;
        public Repayment()
        {
            InitializeComponent();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            //check account
            string accountNo = txtAccountNo.Text;
            string accountPass = txtAccountPass.Password;

            string accountType;

            if (DataOperation.CheckAccountPassword(accountNo, accountPass))
            {
                btnOK.IsEnabled = true;
                BankCustom bankCustom = DataOperation.GetBankCustom(accountNo);
                if (bankCustom.account.LoanInfo.Count == 0)
                {
                    showError("未找到贷款记录！");
                    return;
                }

                txtAccountType.Text = bankCustom.account.LoanInfo.FirstOrDefault().loanType;
                switch (txtAccountType.Text)
                {
                    case "个人贷款":
                        rate = RateType.个人贷款;
                        break;
                    case "企业贷款":
                        rate = RateType.企业贷款;
                        break;
                    case "助学贷款":
                        rate = RateType.助学贷款;
                        break;
                }

                TimeSpan timeSpan = DateTime.Now - bankCustom.account.LoanInfo.FirstOrDefault().loanDate;
                double LX;
                if (txtAccountType.Text.Equals("助学贷款")){
                    timeSpan = DateTime.Now - bankCustom.account.StudentLoan.FirstOrDefault().graduateDate;
                    if (timeSpan.Days <= 0)
                    {
                        LX = bankCustom.account.LoanInfo.FirstOrDefault().loanMoney;
                    }
                }
                LX =(bankCustom.account.LoanInfo.FirstOrDefault().loanMoney * DataOperation.GetRate(rate)*timeSpan.Days/30)+ bankCustom.account.LoanInfo.FirstOrDefault().loanMoney;
                txtAccountMount.Text = LX + "";
                btnOK.IsEnabled = true;
            }
            else
            {
                showError("账号或密码错误");
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            BankEntities bankEntities = new BankEntities();
            string accountNo = txtAccountNo.Text;
            string accountPass = txtAccountPass.Password;

            var q = from t in bankEntities.LoanInfo
                    where t.accountNo == accountNo
                    select t;

            LoanInfo loan;
            loan = bankEntities.LoanInfo.Find(q.FirstOrDefault().Id);
            bankEntities.LoanInfo.Remove(loan);
            bankEntities.SaveChanges();
            showError("还款成功");
            
        }

        private void showError(string message)
        {
            dispatcherTimer.Tick += new EventHandler(closeError);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
            error_message.Content = message;
            error_message_body.IsActive = true;
        }

        private void closeError(object sender, EventArgs e)
        {
            closeError();
            dispatcherTimer.Stop();
        }

        private void closeError()
        {
            error_message_body.IsActive = false;
        }

        private void error_message_ActionClick(object sender, RoutedEventArgs e)
        {
            closeError();
        }
    }
}
