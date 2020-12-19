using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BankManage.common;
using System.Windows.Threading;
using System;
using System.Linq;

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
            string accountNo = txtAccount.Text;
            string accountPass = txtAccountPass.Text;
            string accountType = txtAccountType.Text;
            switch (accountType)
            {
                case "活期存款":
                    Custom custom = DataOperation.GetCustom(this.txtAccount.Text);
                    if (custom == null)
                    {
                        MessageBox.Show("帐号不存在！");
                        return;
                    }
                    //custom.MoneyInfo.accountNo = txtAccount.Text;
                    custom.Diposit("存款", double.Parse(this.txtmount.Text));
                    break;
                case "定期存款":
                    CustomFixed custom1 = DataOperation.GetCustomFixed(txtAccount.Text);
                    if (custom1 == null)
                    {
                        MessageBox.Show("帐号不存在！");
                        return;
                    }
                    //custom1.MoneyInfo.accountNo = txtAccount.Text;
                    custom1.Diposit("存款", double.Parse(this.txtmount.Text));
                    break;
                case "零存整取":
                    CustomFlex custom2 = DataOperation.GetCustomFlex(txtAccount.Text);
                    if (custom2 == null)
                    {
                        MessageBox.Show("帐号不存在！");
                        return;
                    }
                    //custom2.MoneyInfo.accountNo = txtAccount.Text;
                    custom2.Diposit();
                    break;
            }

            
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

            string accountType = DataOperation.GetAccountType(accountNo);
            if (accountType.Equals("NONE"))
            {
                showError("未查找到相关账号");
            }
            else
            {
                if (DataOperation.CheckAccountPassword(accountNo, accountPass))
                {
                    txtAccountType.Text = accountType;
                    if (accountType.Equals("定期存款"))
                    {
                        BankEntities bankEntities = new BankEntities();
                        var q1 = from t in bankEntities.MoneyInfo
                                 where t.accountNo == accountNo
                                 select t;
                        var q2 = from i in bankEntities.MoneyInfo
                                 where i.dealType == "定期存款支取"
                                 select i;
                        if (q1.Count() > 0)
                        {
                            if (q2.Count() == 0)
                            {
                                showError("您还有一个未支取的定期存款");
                            }
                            else
                            {
                                txtmount.IsEnabled = true;
                                btnOk.IsEnabled = true;
                            }
                        }
                    }
                    else if (accountType.Equals("零存整取"))
                    {
                        BankEntities bankEntities = new BankEntities();
                        var q = from t in bankEntities.AccountFlex
                                where t.accountNo == accountNo
                                select t.promisedMoney;
                        if (q.Count() == 1)
                        {
                            txtmount.Text = q.First() + "";
                            btnOk.IsEnabled = true;
                        }

                    }
                    else
                    {
                        txtmount.IsEnabled = true;
                        btnOk.IsEnabled = true;
                    }
                }
                else
                {
                    showError("密码错误");
                }
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
    }
}
