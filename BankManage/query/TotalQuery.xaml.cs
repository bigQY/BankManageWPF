using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankManage.query
{
    /// <summary>
    /// TotalQuery.xaml 的交互逻辑
    /// </summary>
    public partial class TotalQuery : Page
    {
        BankEntities context = new BankEntities();
        public TotalQuery()
        {
            InitializeComponent();
            this.Unloaded += TotalQuery_Unloaded;
        }

        void TotalQuery_Unloaded(object sender, RoutedEventArgs e)
        {
            context.Dispose();
        }
        //查询当前账号的所有记录信息
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var query = from t in context.MoneyInfo
                        where t.accountNo == txtID.Text
                        select t;
            datagrid1.ItemsSource = query.ToList();
        }
    }
}
