using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankManage.other
{
    /// <summary>
    /// ChangeOperate.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeOperate : Page
    {
        BankEntities context = new BankEntities();
        public ChangeOperate()
        {
            InitializeComponent();
        }
        //更改密码
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var query = from t in context.LoginInfo
                        where t.Bno == this.txtAccount.Text
                        select t;
            if (query.Count() > 0)
            {
                var q = query.First();
                q.Password = this.txtNewPass.Password;
                try
                {
                    context.SaveChanges();
                    MessageBox.Show("更改密码成功！");
                }
                catch
                {
                    MessageBox.Show("更改密码失败！");
                }
            }
        }
        //取消更改
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.txtNewPass.Clear();
            this.txtPassConf.Clear();
        }
    }
}
