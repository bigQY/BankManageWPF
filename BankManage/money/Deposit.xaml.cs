using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BankManage.common;

namespace BankManage.money
{
    /// <summary>
    /// Deposit.xaml 的交互逻辑
    /// </summary>
    public partial class Deposit : Page
    {
        public Deposit()
        {
            InitializeComponent();
        }
        //存款
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Custom custom = DataOperation.GetCustom(this.txtAccount.Text);
            if (custom == null)
            {
                MessageBox.Show("帐号不存在！");
                return;
            }
            custom.MoneyInfo.accountNo = txtAccount.Text;
            custom.Diposit("存款", double.Parse(this.txtmount.Text));
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
    }
}
