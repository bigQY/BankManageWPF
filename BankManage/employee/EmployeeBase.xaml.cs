﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BankManage.common;

namespace BankManage.employee
{
    /// <summary>
    /// EmployeeBase.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeBase : Page
    {
        enum Operation { none,edit,remove,add,detail}
        Operation selectedOperation = Operation.none;
        public EmployeeInfo SelectedEmployee
        {
            get;
            set;
        }


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

            var query = DataOperation.GetEmployeeInfos();
            foreach (var i in query)
            {
                dataGridContexts.Add(new EmployeeDataGridContext(i));
            }
            DataContext = this;

            //this.employee_DataGrid.ItemsSource = query.ToList();
        }

        private void editButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            context = new BankEntities();
            string selectedID = "-1";
            foreach (var i in dataGridContexts)
            {
                if (i.选择)
                {
                    selectedID = i.职工号;
                }
            }
            if (selectedID.Equals("-1"))
            {
                MessageBox.Show("未选择员工");
            }
            else
            {
                var q = from t in context.EmployeeInfo
                        where t.EmployeeNo == selectedID
                        select t;
                SelectedEmployee = q.First();
                updateForm();
                dialogHost.IsOpen = true;
                selectedOperation = Operation.edit;
            }
            

            //putSelectedEmployerToDataBridge();
            //DataBridge.GetInstance().getDictionary().Add("employerOperate", "edit");
            //new EmployeeDetail().Show();
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
            foreach (var i in deletedDataGridContexts)
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

        public class EmployeeDataGridContext
        {
            private EmployeeInfo employeeInfo;

            public EmployeeInfo GetEmployeeInfo()
            {
                return employeeInfo;
            }
            public EmployeeDataGridContext(EmployeeInfo employeeInfo)
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
                get
                {
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

        private void Button_Click_Ok(object sender, RoutedEventArgs e)
        {
            switch (selectedOperation)
            {
                case Operation.edit:
                    //TODO 数据合法性检查
                    EmployeeInfo employeeInfo = context.EmployeeInfo.Find(SelectedEmployee.EmployeeNo);
                    employeeInfo.EmployeeName=detailName.Text ;
                    employeeInfo.sex=detailSex.Text ;
                    employeeInfo.telphone=detailPhone.Text;
                    employeeInfo.idCard=detailIDCard.Text;
                    employeeInfo.workDate=detailDate.SelectedDate;
                    context.SaveChanges();
                    break;
                   
                case Operation.add:
                case Operation.detail:
                case Operation.remove:
                default:
                    break;
            }
            employee_DataGrid.ItemsSource = null;
            employee_DataGrid.ItemsSource = dataGridContexts;
            dialogHost.IsOpen = false;
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            dialogHost.IsOpen = false;
        }
    }
}
