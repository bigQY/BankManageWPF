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
using BankManage.money;
using BankManage.money.bank;

namespace BankManage.other
{
    /// <summary>
    /// EducationLoans.xaml 的交互逻辑
    /// </summary>
    public partial class EducationLoans : Page
    {
        int loanYear = 4;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        RateType rate = RateType.助学贷款;
        public EducationLoans()
        {
            InitializeComponent();
            txtRate.Text = DataOperation.GetRate(rate)+"";
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool success = false;
            if (checkInput())
            {
                //check account
                string accountNo = txtAccount.Text;
                string accountPass = txtPass.Password;
                string accountType;


                if (DataOperation.CheckAccountPassword(accountNo, accountPass))
                {
                    BankCustom bankCustom = DataOperation.GetBankCustom(accountNo);
                    accountType = bankCustom.account.accountType;
                    if (accountType.Equals("定期存款") || accountType.Equals("零存整取"))
                    {
                        showError("只有定期存款账户才能贷款");
                    }
                    else
                    {
                        LoanInfo loanInfo = new LoanInfo();
                        loanInfo.accountNo = bankCustom.account.accountNo;
                        loanInfo.loanDate = DateTime.Now;
                        loanInfo.loanMoney = double.Parse(txtMount.Text);
                        loanInfo.loanType = "助学贷款";
                        loanInfo.loanYear = 4;
                        BankEntities bankEntities = new BankEntities();
                        bankEntities.LoanInfo.Add(loanInfo);

                        StudentLoan studentLoan = new StudentLoan();
                        studentLoan.accountNo = bankCustom.account.accountNo;
                        studentLoan.graduateDate = (DateTime)graduateTime.SelectedDate;
                        bankEntities.StudentLoan.Add(studentLoan);

                        bankEntities.SaveChanges();
                        success = true;
                    }
                }
                else
                {
                    showError("账号不存在或密码错误");
                }
            }

            if (success)
            {
                OperateRecord page = new OperateRecord();
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(page);
            }
        }

        private bool checkInput()
        {
            double t;
            if (txtAccount.Text.Length > 6 || txtAccount.Text.Length == 0)
            {
                return false;
            }
            if (txtPass.Password.Length == 0)
            {
                return false;
            }
            if (txtMount.Text.Length == 0 && !Double.TryParse(txtMount.Text, out t))
            {
                return false;
            }

            return true;
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
