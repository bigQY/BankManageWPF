using System.Windows.Controls;
using System.Linq;
namespace BankManage.employee
{
    /// <summary>
    /// ChangePay.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePay : Page
    {
        BankEntities context = new BankEntities();
        public ChangePay()
        {
            InitializeComponent();
            var q = from t in context.EmployeeInfo
                    select new
                    {
                        职工号 = t.EmployeeNo,
                        姓名= t.EmployeeName,
                        工资=t.salary
                    };
            employee_DataGrid.ItemsSource = q.ToList();
        }

        private void saveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void cancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }

}
