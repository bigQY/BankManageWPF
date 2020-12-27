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

namespace BankManage.other
{
    /// <summary>
    /// BankReportLoss.xaml 的交互逻辑
    /// </summary>
    public partial class BankReportLoss : Page
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public BankReportLoss()
        {
            InitializeComponent();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string accountNo = txtAccountNo.Text;
            string accountPass = txtPass.Password;


            if (DataOperation.CheckAccountPassword(accountNo, accountPass))
            {
                DataOperation.BlackAccount(accountNo, "自主挂失");
                showError("挂失成功");
            }
            else
            {
                showError("以挂失或密码错误");
            }

        } 
    }
}
