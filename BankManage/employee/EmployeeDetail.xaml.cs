﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankManage.employee
{
    /// <summary>
    /// EmployeeDetail.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeDetail : Window
    {
        public EmployeeDetail()
        {
            InitializeComponent();
            object op;
            DataBridge.GetInstance().getDictionary().TryGetValue("employerOperate", out op);
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
