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
        public static Frame frame;
        public Main()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.SourceInitialized += MainWindow_SourceInitialized;
            frame = frame1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tabSwitch.IsChecked = false;
            DrawerHost.IsLeftDrawerOpen=false;
            Button item = e.Source as Button;
            if (item != null)
            {
                //内容导航
                frame1.Source = new Uri(item.Tag.ToString(), UriKind.Relative);
                //帮助导航
                switch (item.Tag.ToString())
                {
                    case "":
                        break;
                    default:
                        helperFrame.Source = new Uri("BankHelper/NotFoundPageHelper.xaml", UriKind.Relative);
                        break;
                }
            }
        }
        void MainWindow_SourceInitialized(object sender, System.EventArgs e)
        {
            //默认显示当前页面
            this.frame1.Source = new Uri("other/WelcomePage.xaml", UriKind.Relative);
            //对应的帮助界面
            helperFrame.Source = new Uri("BankHelper/MainPageHelper.xaml", UriKind.Relative);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Show Help
            FrameDrawerHost.IsRightDrawerOpen = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Show About
            FrameDrawerHost.IsBottomDrawerOpen = true;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 返回frame对象，供子页面使用
        /// </summary>
        /// <returns></returns>
        public static Frame GetFrame()
        {
            return frame;
        }
    }
}
