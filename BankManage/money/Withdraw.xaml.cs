using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using BankManage.common;
using BankManage.money.bank;
using System.Linq;

namespace BankManage.money
{
    /// <summary>
    /// Withdraw.xaml 的交互逻辑
    /// </summary>
    /// TODO 账户检查，并且设置定期取款金额
    public partial class Withdraw : Page
    {
        bool importantDecide = false;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public Withdraw()
        {
            InitializeComponent();
        }
        //取款
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
           
            OperateRecord page = new OperateRecord();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(page);
        }
        //取消取款
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            OperateRecord page = new OperateRecord();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(page);
        }

        private void checkAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            //check account
            string accountNo = txtAccountNo.Text;
            string accountPass = txtAccountPass.Password;

            string accountType;

            if (DataOperation.CheckAccountPassword(accountNo, accountPass))
            {
                btnOK.IsEnabled = true;
                BankCustom bankCustom = DataOperation.GetBankCustom(accountNo);
                accountType = bankCustom.account.accountType;
                txtAccountType.Text = accountType;
                txtAccountBalancee.Text = bankCustom.account.accountBalance+"";
                txtAccountLX.Visibility = Visibility.Hidden;
                if (accountType.Equals("定期存款"))
                {
                    txtMount.Visibility = Visibility.Hidden;
                    btnOK.Content = "全部取出";
                    BankEntities bankEntities = new BankEntities();
                    var q = from t in bankEntities.MoneyInfo
                            where t.accountNo == accountNo
                            select t.dealDate;
                    DateTime startTime = q.First().Date;
                    TimeSpan timeSpan = DateTime.Now - startTime;
                    if (timeSpan.Days < bankCustom.account.AccountFixed.promisedYear * 365)
                    {

                        //提取取款计算利息
                        double LX = 0;
                        LX = bankCustom.account.accountBalance * DataOperation.GetRate(RateType.定期提前支取);
                        txtAccountLX.Text = LX + "";
                        txtAccountLX.Visibility = Visibility.Visible;
                    }
                    else if (timeSpan.Days >= bankCustom.account.AccountFixed.promisedYear * 365)
                    {
                        //计算利息
                        RateType rateType;
                        switch (bankCustom.account.AccountFixed.promisedYear)
                        {
                            case 1:
                                rateType = RateType.定期1年;
                                break;
                            case 3:
                                rateType = RateType.定期3年;
                                break;
                            case 5:
                                rateType = RateType.定期5年;
                                break;
                            default:
                                rateType = RateType.定期1年;
                                MessageBox.Show("非法的年份");
                                break;
                        }
                        double LX = 0;
                        LX = bankCustom.account.accountBalance * DataOperation.GetRate(rateType);
                        LX += (LX + bankCustom.account.accountBalance) * DataOperation.GetRate(RateType.定期超期部分) * ((timeSpan.Days - (bankCustom.account.AccountFixed.promisedYear * 365)) / 30);
                        txtAccountLX.Text = LX + "";
                        txtAccountLX.Visibility = Visibility.Visible;
                    }
                }
                else if (accountType.Equals("零存整取"))
                {
                    BankEntities bankEntities = new BankEntities();
                    var q = from t in bankEntities.MoneyInfo
                            where t.accountNo == accountNo
                            select t.dealDate;
                    DateTime startTime = q.First().Date;
                    TimeSpan timeSpan = DateTime.Now - startTime;
                    if (timeSpan.Days < bankCustom.account.AccountFixed.promisedYear * 365)
                    {
                        showError("未到期，无法取出和计算利息");
                        btnOK.IsEnabled = false;
                    }
                    else if (timeSpan.Days >= bankCustom.account.AccountFixed.promisedYear * 365)
                    {
                        //计算利息
                        RateType rateType;
                        switch (bankCustom.account.AccountFixed.promisedYear)
                        {
                            case 1:
                                rateType = RateType.零存整取1年;
                                break;
                            case 3:
                                rateType = RateType.零存整取3年;
                                break;
                            case 5:
                                rateType = RateType.零存整取5年;
                                break;
                            default:
                                rateType = RateType.活期;
                                MessageBox.Show("非法的年份");
                                break;
                        }

                        var qt = from t in bankEntities.MoneyInfo
                                where t.accountNo == accountNo
                                where t.dealType=="零存整取违规"
                                select t.dealDate;
                        double LX = 0;
                        if (qt.Count() == 0)
                        {
                            LX = bankCustom.account.AccountFlex.promisedMoney * bankCustom.account.AccountFlex.promisedYear * 12 * DataOperation.GetRate(rateType);
                            LX += (LX + bankCustom.account.accountBalance) * DataOperation.GetRate(RateType.零存整取超期部分) * ((timeSpan.Days - (bankCustom.account.AccountFixed.promisedYear * 365)) / 30);
                           
                        }
                        else
                        {
                            LX = bankCustom.account.accountBalance *DataOperation.GetRate(RateType.零存整取违规);
                        }
                        txtAccountLX.Text = LX + "";
                        //bankCustom.InsertData("定期存款利息", LX);
                        //bankCustom.withdraw("定期存款支取", bankCustom.account.accountBalance);
                        txtAccountLX.Visibility = Visibility.Visible;

                    }
                }
                else
                {
                    btnOK.IsEnabled = true;
                }
            }
            else
            {
                showError("账号或密码错误");
            }
        }

        private void btnOK_Click_1(object sender, RoutedEventArgs e)
        {
            bool success = false;
            string accountNo = txtAccountNo.Text;
            string accountPass = txtAccountPass.Password;
            BankCustom bankCustom = DataOperation.GetBankCustom(accountNo);
            string accountType=bankCustom.account.accountType;
            if (accountType.Equals("定期存款"))
            {
                BankEntities bankEntities = new BankEntities();
                var q = from t in bankEntities.MoneyInfo
                        where t.accountNo == accountNo
                        select t.dealDate;
                DateTime startTime = q.First().Date;
                TimeSpan timeSpan = DateTime.Now - startTime;
                if (timeSpan.Days < bankCustom.account.AccountFixed.promisedYear * 365)
                {
                    importantMessage.Text = "您的定期存款目前还未到期，您确定要将其取出吗？";
                    dialogHost.IsOpen = true;
                }
                else if (timeSpan.Days >= bankCustom.account.AccountFixed.promisedYear * 365)
                {
                    //计算利息
                    RateType rateType;
                    switch (bankCustom.account.AccountFixed.promisedYear)
                    {
                        case 1:
                            rateType = RateType.定期1年;
                            break;
                        case 3:
                            rateType = RateType.定期3年;
                            break;
                        case 5:
                            rateType = RateType.定期5年;
                            break;
                        default:
                            rateType = RateType.定期1年;
                            MessageBox.Show("非法的年份");
                            break;
                    }
                    double LX = 0;
                    LX = bankCustom.account.accountBalance * DataOperation.GetRate(rateType);
                    LX += (LX + bankCustom.account.accountBalance) * DataOperation.GetRate(RateType.定期超期部分) * ((timeSpan.Days - (bankCustom.account.AccountFixed.promisedYear * 365)) / 30);
                    txtAccountLX.Text = LX + "";
                    bankCustom.InsertData("定期存款利息", LX);
                    bankCustom.InsertData("定期存款支取", -bankCustom.account.accountBalance);
                    txtAccountLX.Visibility = Visibility.Visible;
                    success = true;
                }
            }
            else if (accountType.Equals("零存整取"))
            {
                BankEntities bankEntities = new BankEntities();
                var q = from t in bankEntities.MoneyInfo
                        where t.accountNo == accountNo
                        select t.dealDate;
                DateTime startTime = q.First().Date;
                TimeSpan timeSpan = DateTime.Now - startTime;
                if (timeSpan.Days < bankCustom.account.AccountFixed.promisedYear * 365)
                {
                    showError("未到期，无法取出和计算利息");
                }
                else if (timeSpan.Days >= bankCustom.account.AccountFixed.promisedYear * 365)
                {
                    //计算利息
                    RateType rateType;
                    switch (bankCustom.account.AccountFixed.promisedYear)
                    {
                        case 1:
                            rateType = RateType.零存整取1年;
                            break;
                        case 3:
                            rateType = RateType.零存整取3年;
                            break;
                        case 5:
                            rateType = RateType.零存整取5年;
                            break;
                        default:
                            rateType = RateType.定期1年;
                            MessageBox.Show("非法的年份");
                            break;
                    }

                    var qt = from t in bankEntities.MoneyInfo
                             where t.accountNo == accountNo
                             where t.dealType == "零存整取违规"
                             select t.dealDate;
                    double LX = 0;
                    if (qt.Count() == 0)
                    {
                        LX = bankCustom.account.AccountFlex.promisedMoney * bankCustom.account.AccountFlex.promisedYear * 12 * DataOperation.GetRate(rateType);
                        LX += (LX + bankCustom.account.accountBalance) * DataOperation.GetRate(RateType.零存整取超期部分) * ((timeSpan.Days - (bankCustom.account.AccountFixed.promisedYear * 365)) / 30);

                    }
                    else
                    {
                        LX = bankCustom.account.accountBalance * DataOperation.GetRate(RateType.零存整取违规);
                    }
                    txtAccountLX.Text = LX + "";
                    //bankCustom.InsertData("定期存款利息", LX);
                    //bankCustom.withdraw("定期存款支取", bankCustom.account.accountBalance);
                    txtAccountLX.Visibility = Visibility.Visible;
                    bankCustom.InsertData("零存整取利息", LX);
                    bankCustom.InsertData("零存整取支取", -bankCustom.account.accountBalance);
                    success = true;
                }
            }
            else
            {
                bankCustom.withdraw("取款", double.Parse(txtMount.Text));
                success = true;
            }


            if (success)
            {
                OperateRecord page = new OperateRecord();
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(page);
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

        private void importantOKbtn_Click(object sender, RoutedEventArgs e)
        {
            BankCustom bankCustom = DataOperation.GetBankCustom(txtAccountNo.Text);
            importantDecide = true;
            if (importantDecide)
            {
                //提取取款计算利息
                double LX = 0;
                LX = bankCustom.account.accountBalance * DataOperation.GetRate(RateType.定期提前支取);
                txtAccountLX.Text = LX + "";
                txtAccountLX.Visibility = Visibility.Visible;
                bankCustom.InsertData("定期存款提前支取利息", LX);
                bankCustom.InsertData("定期存款提前支取", -bankCustom.account.accountBalance);
            }
            dialogHost.IsOpen = false;
            OperateRecord page = new OperateRecord();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(page);

        }

        private void importantNObtn_Click(object sender, RoutedEventArgs e)
        {
            importantDecide = false;
            dialogHost.IsOpen = false;
        }
    }
}
