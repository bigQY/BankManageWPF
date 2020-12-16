using System.Windows.Controls;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

namespace BankManage.employee
{
    /// <summary>
    /// ChangePay.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePay : Page
    {
        BankEntities context = new BankEntities();
        List<DisplaydEmployerSalary> ListOfEmployer= new List<DisplaydEmployerSalary>();
        public ChangePay()
        {
            InitializeComponent();
            var q = from t in context.EmployeeInfo
                    select t;
            foreach(var i in q)
            {
                ListOfEmployer.Add(new DisplaydEmployerSalary(i));
            }
            employee_DataGrid.ItemsSource = ListOfEmployer;
        }

        private void saveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void cancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void employee_DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show(((DisplaydEmployerSalary)employee_DataGrid.SelectedItem).姓名);
        }
    }

    public class DisplaydEmployerSalary
    {
        public string 职工号 { get; set; }
        public string 姓名 { get; set; }
        public decimal 工资 { get; set; }

        public DisplaydEmployerSalary(EmployeeInfo t)
        {
            职工号 = t.EmployeeNo;
            姓名 = t.EmployeeName;
            工资 = t.salary.GetValueOrDefault();
        }

    }

}
