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
    /// PersonLoans.xaml 的交互逻辑
    /// </summary>
    public partial class PersonLoans : Page
    {
        int loanYear=-1;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        RateType rate = RateType.个人贷款;
        public PersonLoans()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double LX;
            if (checkInput())
            {
                LX = Double.Parse(txtMount.Text) * DataOperation.GetRate(rate);
                txtInterest.Text = "" + LX;
            }
            else
            {
                btnSubmit.IsEnabled = true;
            }

        }

        private void selectedYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double LX;
            if (checkInput())
            {
                LX = Double.Parse(txtMount.Text) * DataOperation.GetRate(rate)*12*loanYear;
                txtInterest.Text = "" + LX;
            }
            else
            {
                btnSubmit.IsEnabled = true;
            }
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
                        loanInfo.loanType = "个人贷款";
                        switch (selectedYear.SelectedIndex)
                        {
                            case 0:
                                loanInfo.loanYear = 1;
                                break;
                            case 1:
                                loanInfo.loanYear = 3;
                                break;
                            case 2:
                                loanInfo.loanYear = 5;
                                break;
                            case 3:
                                loanInfo.loanYear = 8;
                                break;
                            default:
                                showError("非法的年份");
                                break;
                        }
                        BankEntities bankEntities = new BankEntities();
                        bankEntities.LoanInfo.Add(loanInfo);
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
            if(txtAccount.Text.Length>6|| txtAccount.Text.Length == 0)
            {
                return false;
            }
            if (txtPass.Password.Length == 0)
            {
                return false;
            }
            if (txtMount.Text.Length == 0&&!Double.TryParse(txtMount.Text,out t))
            {
                return false;
            }
            if (selectedYear.SelectedIndex == -1)
            {
                return false;
            }

            switch (selectedYear.SelectedIndex)
            {
                case 0:
                   loanYear = 1;
                    break;
                case 1:
                    loanYear = 3;
                    break;
                case 2:
                    loanYear = 5;
                    break;
                case 3:
                    loanYear = 8;
                    break;
                default:
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
