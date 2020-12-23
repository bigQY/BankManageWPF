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

namespace BankManage.query
{
    /// <summary>
    /// AdvanceQuery.xaml 的交互逻辑
    /// </summary>
    public partial class AdvanceQuery : Page
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public AdvanceQuery()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            BankEntities bankEntities = new BankEntities();
            switch (searchPoint.SelectedIndex)
            {
                //身份证
                case 0:
                    if (searchMethod.SelectedIndex == 0)
                    {
                        var qId = from tId in bankEntities.AccountInfo
                                  where tId.IdCard.Contains(searchText.Text)
                                  select new
                                  {
                                      账号 = tId.accountNo,
                                      姓名 = tId.accountName,
                                      身份证号 = tId.IdCard,
                                      存款类型 = tId.accountType,
                                      余额 = tId.accountBalance
                                  };
                        dataGrid.ItemsSource = qId.ToList();
                    }
                    else 
                    {
                        var qId = from tId in bankEntities.AccountInfo
                                  where tId.IdCard.Equals(searchText.Text)
                                  select new
                                  {
                                      账号 = tId.accountNo,
                                      姓名 = tId.accountName,
                                      身份证号 = tId.IdCard,
                                      存款类型 = tId.accountType,
                                      余额 = tId.accountBalance
                                  };
                        dataGrid.ItemsSource = qId.ToList();
                    }
                    break;
                //账号
                case 1:
                    if (searchMethod.SelectedIndex == 0)
                    {
                        var qAccount = from tId in bankEntities.AccountInfo
                                       where tId.accountNo.Contains(searchText.Text)
                                       select new
                                       {
                                           账号 = tId.accountNo,
                                           姓名 = tId.accountName,
                                           身份证号 = tId.IdCard,
                                           存款类型 = tId.accountType,
                                           余额 = tId.accountBalance
                                       };
                        dataGrid.ItemsSource = qAccount.ToList();
                    }
                    else
                    {
                        var qAccount = from tId in bankEntities.AccountInfo
                                       where tId.accountNo.Equals(searchText.Text)
                                       select new
                                       {
                                           账号 = tId.accountNo,
                                           姓名 = tId.accountName,
                                           身份证号 = tId.IdCard,
                                           存款类型 = tId.accountType,
                                           余额 = tId.accountBalance
                                       };
                        dataGrid.ItemsSource = qAccount.ToList();
                    }
                    break;
                //姓名
                case 2:
                    if (searchMethod.SelectedIndex == 0)
                    {
                        var qName = from tId in bankEntities.AccountInfo
                                    where tId.accountName.Contains(searchText.Text)
                                    select new
                                    {
                                        账号 = tId.accountNo,
                                        姓名 = tId.accountName,
                                        身份证号 = tId.IdCard,
                                        存款类型 = tId.accountType,
                                        余额 = tId.accountBalance
                                    };
                        dataGrid.ItemsSource = qName.ToList();
                    }
                    else
                    {
                        var qName = from tId in bankEntities.AccountInfo
                                    where tId.accountName.Equals(searchText.Text)
                                    select new
                                    {
                                        账号 = tId.accountNo,
                                        姓名 = tId.accountName,
                                        身份证号 = tId.IdCard,
                                        存款类型 = tId.accountType,
                                        余额 = tId.accountBalance
                                    };
                        dataGrid.ItemsSource = qName.ToList();
                    }
                    break;
                //任意关键字
                case 3:
                    if (searchMethod.SelectedIndex == 0)
                    {
                        var q = from tId in bankEntities.AccountInfo
                                where tId.accountName.Contains(searchText.Text) || tId.accountNo.Contains(searchText.Text) || tId.IdCard.Contains(searchText.Text)
                                select new
                                {
                                    账号 = tId.accountNo,
                                    姓名 = tId.accountName,
                                    身份证号 = tId.IdCard,
                                    存款类型 = tId.accountType,
                                    余额 = tId.accountBalance
                                };
                        dataGrid.ItemsSource = q.ToList();
                    }
                    else
                    {
                        var q = from tId in bankEntities.AccountInfo
                                where tId.accountName.Equals(searchText.Text) || tId.accountNo.Equals(searchText.Text) || tId.IdCard.Equals(searchText.Text)
                                select new
                                {
                                    账号 = tId.accountNo,
                                    姓名 = tId.accountName,
                                    身份证号 = tId.IdCard,
                                    存款类型 = tId.accountType,
                                    余额 = tId.accountBalance
                                };
                        dataGrid.ItemsSource = q.ToList();
                    }
                    break;
                //
                default:
                    showError("未选择关键字");
                    break;
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
