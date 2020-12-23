using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BankManage.other
{
    /// <summary>
    /// ChangeEmployee.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeEmployee : Page
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        BankEntities context = new BankEntities();
        public ChangeEmployee()
        {
            InitializeComponent();
        }
        //更改密码
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewPass.Password.Equals(txtPassConf.Password))
            {
                var query = from t in context.AccountInfo
                            where t.accountNo == this.txtAccount.Text
                            select t;
                if (query.Count() > 0)
                {
                    var q = query.First();
                    q.accountPass = this.txtNewPass.Password;
                    try
                    {
                        context.SaveChanges();
                        showError("更改密码成功！");
                    }
                    catch
                    {
                        showError("更改密码失败！");
                    }
                }
                else
                {
                    showError("账号不存在");
                }
            }
            else
            {
                showError("两次输入的密码不一致");
            }
        }
        //取消更改
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.txtNewPass.Clear();
            this.txtPassConf.Clear();
        }


        /// <summary>
        /// 计算密码强度
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns></returns>
        private static int PasswordStrength(string password)
        {
            int result = 0;
            //空字符串强度值为0
            if (password == "") return 0;
            //字符统计
            int iNum = 0, iLtt = 0, iSym = 0;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') iNum++;
                else if (c >= 'a' && c <= 'z') iLtt++;
                else if (c >= 'A' && c <= 'Z') iLtt++;
                else iSym++;
            }
            result = (iSym * 4) + iNum + (int)(iLtt * 2) + password.Length;
            if (iLtt == 0 && iSym == 0)
                result -= 10; //纯数字密码
            if (iNum == 0 && iLtt == 0)
                result -= 5; //纯符号密码
            if (iNum == 0 && iSym == 0)
                result -= 7;//纯字母密码
            if (password.Length <= 6)
                result -= 1; //长度不大于6的密码
            if (iLtt == 0)
                result += 5; //数字和符号构成的密码
            if (iSym == 0)
                result += 5; //数字和字母构成的密码
            if (iNum == 0)
                result += 5; //字母和符号构成的密码
            if (password.Length <= 10)
                result -= 3; //长度不大于10的密码
            if (iLtt != 0 && iNum != 0 && iSym != 0)
                result += 20;//由数字、字母、符号构成的密码
            result *= 2;
            if (result > 100)
            {
                result = 100;
            }
            else if (result < 0)
            {
                result = 0;
            }
            return result;
        }

        private void txtNewPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordStrength.Value = PasswordStrength(txtNewPass.Password);
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
            if (error_message.Content.ToString().Equals("两次输入的密码不一致"))
                this.txtPassConf.Clear();
            if (error_message.Content.ToString().Equals("账号不存在"))
            {
                txtAccount.Clear();
                txtNewPass.Clear();
                txtPassConf.Clear();
            }

        }
    }
}
