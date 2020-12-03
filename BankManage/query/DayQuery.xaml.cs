using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankManage.query
{
    /// <summary>
    /// DayQuery.xaml 的交互逻辑
    /// 当日汇总
    /// </summary>
    public partial class DayQuery : Page
    {
        BankEntities context = new BankEntities();
        public DayQuery()
        {
            InitializeComponent();
            this.Unloaded += DayQuery_Unloaded;
        }

        void DayQuery_Unloaded(object sender, RoutedEventArgs e)
        {
            context.Dispose();
        }

        //加载汇总当天发生所有交易信息
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //查询当日发生的交易记录
            var query = from t in context.MoneyInfo
                        where t.dealDate.Year == DateTime.Now.Year && t.dealDate.Month == DateTime.Now.Month && t.dealDate.Day == DateTime.Now.Day
                        select t;
            this.datagrid1.ItemsSource = query.ToList();
            //查询当日的总收入金额
            var query1 = from v in query
                         where v.dealType == "开户" || v.dealType == "存款"
                         select v.dealMoney;
            //查询当日的总支出金额
            var query2 = from m in query
                         where m.dealType == "结息" || m.dealType == "取款"
                         select m.dealMoney;
            if (query1.Count() != 0 && query2.Count() != 0)
            {
                var s1 = query1.Sum();
                var s2 = query2.Sum();
                this.textTotal.Text = string.Format("当日汇总收入金额:{0},总支出金额{1}", s1, s2);
            }
            else
            {
                datagrid1.Visibility = Visibility.Hidden;
                this.textTotal.Text = "当日没有任何交易记录！";

            }


        }
    }
}
