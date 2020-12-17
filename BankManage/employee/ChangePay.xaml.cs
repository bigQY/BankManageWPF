using System.Windows.Controls;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;

namespace BankManage.employee
{
    /// <summary>
    /// ChangePay.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePay : Page
    {
        EmployeeInfo SelectedEmployee;
        BankEntities context = new BankEntities();
        List<DisplaydEmployerSalary> ListOfEmployer= new List<DisplaydEmployerSalary>();
        public ChangePay()
        {
            InitializeComponent();
            updateDataGrid();
        }

        private void updateDataGrid()
        {
            var q = from t in context.EmployeeInfo
                    select t;
            foreach (var i in q)
            {
                ListOfEmployer.Add(new DisplaydEmployerSalary(i));
            }
            employee_DataGrid.ItemsSource = ListOfEmployer;
        }

        private void saveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int count = 0;
            foreach(var i in employee_DataGrid.ItemsSource)
            {
                EmployeeInfo employeeInfo = context.EmployeeInfo.Find((((DisplaydEmployerSalary)i).职工号));
                if(employeeInfo.salary != (((DisplaydEmployerSalary)i).工资))
                {
                    employeeInfo.salary = (((DisplaydEmployerSalary)i).工资);
                    context.SaveChanges();
                    count++;
                }
                
            }
            updateDataGrid();
            MessageBox.Show(count + "位员工的工资被修改");
        }

        private void cancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void employee_DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dialogHost.IsOpen = true;
            var q = from t in context.EmployeeInfo
                    where t.EmployeeNo== (((DisplaydEmployerSalary)employee_DataGrid.SelectedItem).职工号)
                    select t;
            SelectedEmployee =q.First();
            updateForm();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            EmployeeInfo employeeInfo = context.EmployeeInfo.Find(SelectedEmployee.EmployeeNo);
            employeeInfo.salary = decimal.Parse(detailSalary.Text);
            context.SaveChanges();
            MessageBox.Show("保存成功");
            dialogHost.IsOpen = false;

        }

        private void updateForm()
        {
            if (SelectedEmployee != null)
            {
                detailNo.Text = SelectedEmployee.EmployeeNo;
                detailName.Text = SelectedEmployee.EmployeeName;
                detailSex.Text = SelectedEmployee.sex;
                detailPhone.Text = SelectedEmployee.telphone;
                detailIDCard.Text = SelectedEmployee.idCard;
                detailDate.SelectedDate = SelectedEmployee.workDate;
                detailSalary.Text = SelectedEmployee.salary.ToString();

                if (SelectedEmployee.photo != null)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    MemoryStream ms = new MemoryStream(SelectedEmployee.photo);
                    bmi.StreamSource = ms;
                    bmi.EndInit();
                    detailPhoto.Source = bmi;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dialogHost.IsOpen = false;
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
