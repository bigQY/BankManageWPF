using System.Windows;
using System.Windows.Controls.Primitives;

namespace BankManage
{
    /// <summary>
    /// TESTWindow1.xaml 的交互逻辑
    /// </summary>
    public partial class TESTWindow1 : Window
    {
        public TESTWindow1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MaterialDesignThemes.Wpf.DialogHost();

            //MaterialDesignThemes.Wpf.DialogHost.Show();
        }

        

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            tabswitch.IsChecked = DrawerHost.IsLeftDrawerOpen;
        }
    }
}
