using System;
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
        public List<EmployeeDataGridContext> dataGridContexts = new List<EmployeeDataGridContext>();
        public List<EmployeeDataGridContext> DataGridContexts
        {
            get { return dataGridContexts; }
            set { }
        }
        public EmployeeBase()
        {
            InitializeComponent();
            
            BankEntities context = new BankEntities();
            var query = from t in context.EmployeeInfo
                        select t;
            foreach( var i in query)
            {
                dataGridContexts.Add(new EmployeeDataGridContext(i));
            }
            DataContext = this;
            //this.employee_DataGrid.ItemsSource = query.ToList();
        }
    }

    public class EmployeeDataGridContext {
        private EmployeeInfo employeeInfo;
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
