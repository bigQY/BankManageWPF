using System;
using System.Collections.Generic;
using System.Drawing;
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
        byte[] pic;
        enum Operation { none,edit,remove,add,detail}
        Operation selectedOperation = Operation.none;
        public EmployeeInfo SelectedEmployee
        {
            get;
            set;
        }


        BankEntities context=new BankEntities();
        public List<EmployeeDataGridContext> dataGridContexts = new List<EmployeeDataGridContext>();
        public List<EmployeeDataGridContext> DataGridContexts
        {
            get { return dataGridContexts; }
            set { }
        }
        public EmployeeBase()
        {
            InitializeComponent();
            refreshData();

            //this.employee_DataGrid.ItemsSource = query.ToList();
        }

        private void refreshData()
        {
            var query = DataOperation.GetEmployeeInfos();
            dataGridContexts.Clear();
            foreach (var i in query)
            {
                dataGridContexts.Add(new EmployeeDataGridContext(i));
            }
            DataContext = this;
        }

        private void editButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

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
            refreshData();

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
            refreshData();

        }

        private void detailButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
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
                selectedOperation = Operation.detail;
                detailNo.IsEnabled = false;
                detailName.IsEnabled = false;
                detailSex.IsEnabled = false;
                detailPhone.IsEnabled = false;
                detailIDCard.IsEnabled = false;
                detailDate.IsEnabled = false;
                detailSalary.IsEnabled = false;

                dialogHost.IsOpen = true;

            }
            refreshData();

        }

        private void addEmployerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            selectedOperation = Operation.add;
            SelectedEmployee = new EmployeeInfo();
            detailNo.IsEnabled = true;
            dialogHost.IsOpen = true;
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
            refreshData();

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
                    //TODO 数据合法性检查,否则 报异常
                    EmployeeInfo employeeInfo = context.EmployeeInfo.Find(SelectedEmployee.EmployeeNo);
                    employeeInfo.EmployeeName=detailName.Text ;
                    employeeInfo.sex=detailSex.Text ;
                    employeeInfo.telphone=detailPhone.Text;
                    employeeInfo.idCard=detailIDCard.Text;
                    employeeInfo.workDate=detailDate.SelectedDate;
                    employeeInfo.salary = decimal.Parse(detailSalary.Text);
                    if(pic!=null)
                        employeeInfo.photo = pic;
                    context.SaveChanges();
                    break;
                   
                case Operation.add:
                    EmployeeInfo newEmployeeInfo = new EmployeeInfo();
                    newEmployeeInfo.EmployeeNo = detailNo.Text;
                    newEmployeeInfo.EmployeeName = detailName.Text;
                    newEmployeeInfo.sex = detailSex.Text;
                    newEmployeeInfo.telphone = detailPhone.Text;
                    newEmployeeInfo.idCard = detailIDCard.Text;
                    newEmployeeInfo.workDate = detailDate.SelectedDate;
                    newEmployeeInfo.photo =new byte[10];
                    newEmployeeInfo.salary = decimal.Parse(detailSalary.Text);
                    if (pic != null)
                        newEmployeeInfo.photo = pic;
                    context.EmployeeInfo.Add(newEmployeeInfo);
                    context.SaveChanges();
                    break;
                case Operation.detail:
                    detailNo.IsEnabled = true;
                    detailName.IsEnabled = true;
                    detailSex.IsEnabled = true;
                    detailPhone.IsEnabled = true;
                    detailIDCard.IsEnabled = true;
                    detailDate.IsEnabled = true;
                    detailSalary.IsEnabled = true;
                    break;
                case Operation.remove:
                    break;
                default:
                    break;
            }
            employee_DataGrid.ItemsSource = null;
            employee_DataGrid.ItemsSource = dataGridContexts;
            dialogHost.IsOpen = false;

        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            switch (selectedOperation)
            {
                case Operation.detail:
                    detailNo.IsEnabled = true;
                    detailName.IsEnabled = true;
                    detailSex.IsEnabled = true;
                    detailPhone.IsEnabled = true;
                    detailIDCard.IsEnabled = true;
                    detailDate.IsEnabled = true;
                    detailSalary.IsEnabled = true;
                    break;
                default:
                    break;
            }
            dialogHost.IsOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //选择照片
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".jpg";
            ofd.Filter = "图像文件|*.jpg";
            if (ofd.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(ofd.FileName,FileMode.Open,FileAccess.Read);
                BinaryReader reader = new BinaryReader(fileStream);

                pic = reader.ReadBytes((int)fileStream.Length);
                System.Drawing.Image img = System.Drawing.Image.FromStream(fileStream);
                Bitmap bmp = new System.Drawing.Bitmap(img);
                IntPtr hBitmap = bmp.GetHbitmap();
                System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                detailPhoto.Source = WpfBitmap;
            }
        }
    }
}
