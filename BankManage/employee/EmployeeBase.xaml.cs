using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace BankManage.employee
{
    /// <summary>
    /// EmployeeBase.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeBase : Page
    {
        BankEntities context;
        public List<EmployeeDataGridContext> dataGridContexts = new List<EmployeeDataGridContext>();
        public List<EmployeeDataGridContext> DataGridContexts
        {
            get { return dataGridContexts; }
            set { }
        }
        public EmployeeBase()
        {
            InitializeComponent();
            
            context = new BankEntities();
            var query = from t in context.EmployeeInfo
                        select t;
            foreach( var i in query)
            {
                dataGridContexts.Add(new EmployeeDataGridContext(i));
            }
            DataContext = this;
            //this.employee_DataGrid.ItemsSource = query.ToList();
        }

        private void editButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            putSelectedEmployerToDataBridge();
            DataBridge.GetInstance().getDictionary().Add("employerOperate", "edit");
            new EmployeeDetail().Show();
        }

        private void putSelectedEmployerToDataBridge()
        {
            foreach (var i in dataGridContexts)
            {
                if (i.选择)
                {
                    DataBridge.GetInstance().getDictionary().Add("seletedEmployer", i);
                    break;
                }
            }
        }

        private void deleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            List<EmployeeDataGridContext> deletedDataGridContexts = new List<EmployeeDataGridContext>();
            foreach (var i in dataGridContexts)
            {
                if (i.选择)
                {
                    try
                    {
                        context.EmployeeInfo.Remove(i.GetEmployeeInfo());
                        deletedDataGridContexts.Add(i);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            foreach(var i in deletedDataGridContexts)
            {
                dataGridContexts.Remove(i);
            }
            employee_DataGrid.ItemsSource = null;
            employee_DataGrid.ItemsSource = dataGridContexts;
            context.SaveChanges();
        }

        private void detailButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            putSelectedEmployerToDataBridge();
            DataBridge.GetInstance().getDictionary().Add("employerOperate", "detail");
            new EmployeeDetail().Show();

        }

        private void addEmployerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataBridge.GetInstance().getDictionary().Add("employerOperate", "add");
            new EmployeeDetail().Show();

        }
    }

    public class EmployeeDataGridContext {
        private EmployeeInfo employeeInfo;

        public EmployeeInfo GetEmployeeInfo()
        {
            return employeeInfo;
        }
        public EmployeeDataGridContext(EmployeeInfo  employeeInfo)
        {
            this.employeeInfo = employeeInfo;
        }

        public bool 选择
        {
            get;
            set;
        }
        public string 职工号
        {
            get
            {
                return employeeInfo.EmployeeNo;
            }
            set { }
        }

        public string 姓名
        {
            get {
                return employeeInfo.EmployeeName;
            }
            set { }
        }

        public string 性别
        {
            get
            {
                return employeeInfo.sex;
            }
            set { }
        }

        public string 入职日期
        {
            get
            {
                DateTime dataTime = (DateTime)employeeInfo.workDate;
                return dataTime.ToLongDateString();
            }
            set { }
        }



    }
}
