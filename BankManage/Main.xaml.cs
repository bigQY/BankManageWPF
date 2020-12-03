using System;
using System.Windows;
using System.Windows.Controls;

namespace BankManage
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.SourceInitialized += MainWindow_SourceInitialized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button item = e.Source as Button;
            if (item != null)
            {
                frame1.Source = new Uri(item.Tag.ToString(), UriKind.Relative);
            }
        }
        void MainWindow_SourceInitialized(object sender, System.EventArgs e)
        {
            //默认显示当前页面
            this.frame1.Source = new Uri("money/OperateRecord.xaml", UriKind.Relative);
            //启动登陆窗体
            LoginForm login = new LoginForm();
            login.ShowDialog();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            DrawerHost.IsLeftDrawerOpen = tabSwitch.IsChecked.Value;
        }

        private void DrawerHost_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            tabSwitch.IsChecked = DrawerHost.IsLeftDrawerOpen;
        }

        private void DrawerHost_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tabSwitch.IsChecked = DrawerHost.IsLeftDrawerOpen;
        }
    }
}
