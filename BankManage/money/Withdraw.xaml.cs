using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BankManage.common;

namespace BankManage.money
{
    /// <summary>
    /// Withdraw.xaml 的交互逻辑
    /// </summary>
    public partial class Withdraw : Page
    {
        public Withdraw()
        {
            InitializeComponent();
        }
        //取款
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Custom custom = DataOperation.GetCustom(this.txtAccount.Text);
            if (custom.AccountInfo.accountPass != this.txtPassword.Password)
            {
                MessageBox.Show("密码不正确");
                return;
            }
            custom.Withdraw(double.Parse(this.txtmount.Text));
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
    }
}
