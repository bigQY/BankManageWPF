using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BankManage.common;
using BankManage.money.bank;

namespace BankManage.money
{
    /// <summary>
    /// NewAccount.xaml 的交互逻辑
    /// </summary>
    public partial class NewAccount : Page
    {
        public NewAccount()
        {
            InitializeComponent();
            InitComboBox();
            InitErrorSign();
        }

        private void InitErrorSign()
        {
            err_1.Visibility = Visibility.Hidden;
            err_2.Visibility = Visibility.Hidden;
            err_3.Visibility = Visibility.Hidden;
            err_4.Visibility = Visibility.Hidden;
            err_6.Visibility = Visibility.Hidden;
        }

        private void InitComboBox()
        {
            string[] items = Enum.GetNames(typeof(MoneyAccountType));
            foreach (var s in items)
            {
                comboBoxAccountType.Items.Add(s);
            }
            comboBoxAccountType.SelectedIndex = 0;
        }
        //开户
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (checkInput())
            {
                if ((comboBoxAccountType.SelectedItem.ToString().Equals("活期存款") || promiseYear.SelectedIndex != -1))
                {
                    int y = (1 + 2 * promiseYear.SelectedIndex);
                    AccountInfo accountInfo = new AccountInfo();
                    accountInfo.accountNo= txtAccountNo.Text;
                    accountInfo.accountName = txtAccountName.Text;
                    accountInfo.accountPass = txtPass.Password;
                    accountInfo.accountType = comboBoxAccountType.SelectedItem.ToString();
                    accountInfo.accountBalance = double.Parse(txtMoney.Text);

                    switch (comboBoxAccountType.SelectedItem.ToString())
                    {
                        case "活期存款":
                            BankCustom custom = new BankCustom();
                            custom.create(accountInfo);
                            /*CustomChecking custom = (CustomChecking)DataOperation.CreateCustom("活期存款");
                            custom.AccountInfo.accountNo = this.txtAccountNo.Text;
                            custom.AccountInfo.IdCard = this.txtIDCard.Text;
                            custom.AccountInfo.accountName = this.txtAccountName.Text;
                            custom.AccountInfo.accountPass = this.txtPass.Password;
                            custom.Create(this.txtAccountNo.Text, double.Parse(this.txtMoney.Text), comboBoxAccountType.SelectedItem.ToString());*/
                            
                            break;
                        case "定期存款":
                            BankCustom custom1 = new BankCustom();
                            custom1.create(accountInfo,y);
                            /*CustomFixed customFixed = (CustomFixed)DataOperation.CreateCustom("定期存款");
                            customFixed.AccountInfo.accountNo = this.txtAccountNo.Text;
                            customFixed.AccountInfo.IdCard = this.txtIDCard.Text;
                            customFixed.AccountInfo.accountName = this.txtAccountName.Text;
                            customFixed.AccountInfo.accountPass = this.txtPass.Password;
                            customFixed.Create(this.txtAccountNo.Text, double.Parse(this.txtMoney.Text), y, comboBoxAccountType.SelectedItem.ToString());*/
                            break;
                        case "零存整取":
                            BankCustom custom2 = new BankCustom();
                            custom2.create(accountInfo, y, double.Parse(this.txtMoney.Text));
                            /*CustomFlex customFlex = (CustomFlex)DataOperation.CreateCustom("零存整取");
                            customFlex.AccountInfo.accountNo = this.txtAccountNo.Text;
                            customFlex.AccountInfo.IdCard = this.txtIDCard.Text;
                            customFlex.AccountInfo.accountName = this.txtAccountName.Text;
                            customFlex.AccountInfo.accountPass = this.txtPass.Password;
                            customFlex.Create(this.txtAccountNo.Text, double.Parse(this.txtMoney.Text), int.Parse(promiseMoneyText.Text), y);*/
                            break;
                    }
                    OperateRecord page = new OperateRecord();
                    NavigationService ns = NavigationService.GetNavigationService(this);
                    ns.Navigate(page);
                }
                else
                {
                    if (comboBoxAccountType.SelectedItem.ToString().Equals("定期存款"))
                    {
                        promiseMoneyLable.Visibility = Visibility.Hidden;
                        promiseMoneyText.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        promiseMoneyLable.Visibility = Visibility.Visible;
                        promiseMoneyText.Visibility = Visibility.Visible;
                        promiseMoneyText.Text = txtMoney.Text;
                    }
                    DialogHost.IsOpen = true;
                }
                //TODO IF写的太多了，需要优化
                /*if (comboBoxAccountType.SelectedItem.ToString().Equals("活期存款") || promiseYear.SelectedIndex != -1)
                {

                    Custom custom = DataOperation.CreateCustom(comboBoxAccountType.SelectedItem.ToString());
                    custom.AccountInfo.accountNo = this.txtAccountNo.Text;
                    custom.AccountInfo.IdCard = this.txtIDCard.Text;
                    custom.AccountInfo.accountName = this.txtAccountName.Text;
                    custom.AccountInfo.accountPass = this.txtPass.Password;
                    custom.Create(this.txtAccountNo.Text, double.Parse(this.txtMoney.Text), comboBoxAccountType.SelectedItem.ToString());
                    OperateRecord page = new OperateRecord();
                    NavigationService ns = NavigationService.GetNavigationService(this);
                    ns.Navigate(page);

                }
                else
                {
                    if (comboBoxAccountType.SelectedItem.ToString().Equals("定期存款"))
                    {
                        promiseMoneyText.Visibility = Visibility.Hidden;
                        promiseMoneyLable.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        promiseMoneyText.Visibility = Visibility.Visible;
                        promiseMoneyLable.Visibility = Visibility.Visible;
                        promiseMoneyText.Text = txtMoney.Text;
                    }
                    DialogHost.IsOpen = true;
                }*/
            }
        }
        //取消开户
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.txtAccountName.Clear();
            this.txtIDCard.Clear();
            this.txtAccountName.Clear();
            this.txtPass.Clear();
            this.txtMoney.Clear();
            OperateRecord page = new OperateRecord();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(page);
        }

        private void comboBoxAccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            promiseYear.SelectedIndex = -1;
            string s = comboBoxAccountType.SelectedItem.ToString();
            using (BankEntities c = new BankEntities())
            {
                var q = from t in c.AccountInfo
                        where t.accountType == s
                        select t;
                if (q.Count() > 0)
                {
                    this.txtAccountNo.Text = string.Format("{0}", int.Parse(q.Max(x => x.accountNo)) + 1);
                }
                else
                {
                    txtAccountNo.Text = string.Format("{0}00001", comboBoxAccountType.SelectedIndex + 1);
                }
            }
        }

        private bool checkInput()
        {
            InitErrorSign();
            bool result = true;
            string accountNoText = txtAccountNo.Text;
            string accountNameText = txtAccountName.Text;
            string iDCardText = txtIDCard.Text;
            string passText = txtPass.Password;
            double money = double.Parse(txtMoney.Text);
            string moneyText = txtMoney.Text;

            if (accountNoText.Length > 6 || !DataOperation.CheckAccountNo(accountNoText))
            {
                result = false;
                err_1.Visibility = Visibility.Visible;
            }
            if (accountNameText.Length > 20)
            {
                result = false;
                err_2.Visibility = Visibility.Visible;
            }
            if (!CheckIDCard18(iDCardText))
            {
                result = false;
                err_3.Visibility = Visibility.Visible;
            }
            if (passText.Length > 20)
            {
                result = false;
                err_4.Visibility = Visibility.Visible;
            }
            if (comboBoxAccountType.SelectedItem.ToString() == "零存整取")
            {
                if (money < 5)
                {
                    result = false;
                    err_6.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (money < 100)
                {
                    result = false;
                    err_6.Visibility = Visibility.Visible;
                }
            }
            return result;
        }
        /// <summary>
        /// 标准18位身份证验证
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        private bool CheckIDCard18(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = idNumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
        }
    }
}
