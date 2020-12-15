using System.Windows;
using System.Windows.Controls.Primitives;
using MaterialDesignThemes.Wpf;

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
            DialogHost.Show("test");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.Show(new ToggleButton());
        }
    }
}
