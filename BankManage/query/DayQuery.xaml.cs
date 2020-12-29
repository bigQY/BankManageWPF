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
            double s1=0, s2=0;
            //查询当日发生的交易记录
            var query = from t in context.MoneyInfo
                        select t;
            if (query.Count() != 0)
            {
                foreach (var item in query)
                {
                    if((DateTime.Now- item.dealDate).Days <= 1)
                    {
                        datagrid1.Items.Add(item);
                        if(item.dealType.Contains("开户") || item.dealType.Contains("存款"))
                        {
                            s1 += item.dealMoney;
                        }
                        if(item.dealType.Contains("结息") || item.dealType.Contains("取款"))
                        {
                            s2 += item.dealMoney;
                        }
                    }
                }
                this.textTotal.Text = string.Format("当日汇总收入金额:{0},总支出金额{1}", s1, s2);
                //this.datagrid1.ItemsSource = query.ToList();
            }
            else
            {
                datagrid1.Visibility = Visibility.Hidden;
                this.textTotal.Text = "当日没有任何交易记录！";

            }


        }
    }
}
