using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BankManage.common;
using System.Windows.Threading;
using System;
using System.Linq;
using BankManage.money.bank;

namespace BankManage.money
{
    /// <summary>
    /// Deposit.xaml 的交互逻辑
    /// </summary>
    public partial class Deposit : Page
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public Deposit()
        {
            InitializeComponent();
        }
        //存款
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            double money = double.Parse(txtmount.Text);
            string accountNo = txtAccount.Text;
            string accountType;
            BankCustom bankCustom = DataOperation.GetBankCustom(accountNo);
            accountType = bankCustom.account.accountType;
            txtAccountType.Text = accountType;
            bankCustom.deposit(accountType, money);

            OperateRecord page = new OperateRecord();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(page);

        }
        //取消存款
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            OperateRecord page = new OperateRecord();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(page);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //check account
            string accountNo = txtAccount.Text;
            string accountPass = txtAccountPass.Text;

            string accountType;

            if (DataOperation.CheckAccountPassword(accountNo, accountPass))
            {
                BankCustom bankCustom = DataOperation.GetBankCustom(accountNo);
                accountType = bankCustom.account.accountType;
                txtAccountType.Text = accountType;
                if (accountType.Equals("定期存款"))
                {

                    if (bankCustom.account.accountBalance != 0)
                    {
                        showError("您还有一个未支取的定期存款");
                    }
                    else
                    {
                        txtmount.IsEnabled = true;
                        btnOk.IsEnabled = true;
                    }

                }
                else if (accountType.Equals("零存整取"))
                {
                    txtmount.Text = bankCustom.account.AccountFlex.promisedMoney + "";
                    btnOk.IsEnabled = true;
                }
                else
                {
                    txtmount.IsEnabled = true;
                    btnOk.IsEnabled = true;
                }
            }
            else
            {
                showError("账号或密码错误");
            }

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
