using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace BankManage
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        int clickCount = 0;

        public static Frame frame;

        public Main()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.SourceInitialized += MainWindow_SourceInitialized;
            frame = frame1;
            loadSetting();
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
                    case "money/NewAccount.xaml":
                        helperFrame.Source = new Uri("BankHelper/Page1.xaml", UriKind.Relative);
                        break;
                    default:
                        helperFrame.Source = new Uri("BankHelper/NotFoundPageHelper.xaml", UriKind.Relative);
                        break;
                }
                //关于导航
                //aboutFrame.Source= new Uri("About/AboutPage.xaml", UriKind.Relative);
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
            frame1.Source = new Uri("About/AboutPage.xaml", UriKind.Relative);
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

        private void PackIcon_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            var paletteHelper = new PaletteHelper();
            //Retrieve the app's existing theme
            ITheme theme = paletteHelper.GetTheme();

            //Change the base theme to Dark
            theme.SetBaseTheme(Theme.Dark);
            //or theme.SetBaseTheme(Theme.Light);

            //Change all of the primary colors to Red
            //theme.SetPrimaryColor(Colors.Red);

            //Change all of the secondary colors to Blue
            //theme.SetSecondaryColor(Colors.Blue);

            //You can also change a single color on the theme, and optionally set the corresponding foreground color
            //theme.PrimaryMid = new ColorPair(Colors.Brown, Colors.White);

            //Change the app's current theme
            paletteHelper.SetTheme(theme);
        }

        private void ToggleButton_Click_1(object sender, RoutedEventArgs e)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            if(darkModeSwitch.IsChecked.Value)
                theme.SetBaseTheme(Theme.Dark);
            else
            {
                theme.SetBaseTheme(Theme.Light);
            }
            paletteHelper.SetTheme(theme);
        }

        private void Image_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            clickCount++;
            if (clickCount >= 10)
            {
                evalWorld.IsActive = true;
                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(321);
                dispatcherTimer.Tick += 派萌很生气;
                dispatcherTimer.Start();
            }
            
            
        }

        private void loadSetting()
        {
            MainSettings mainSettings = MainSettings.Default;
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            switch (mainSettings.主题)
            {
                case "玉红":
                    theme.SetPrimaryColor(Color.FromRgb(192, 72, 81));
                    break;
                case "靛青":
                    theme.SetPrimaryColor(Color.FromRgb(22, 97, 171));
                    break;
                case "竹绿":
                    theme.SetPrimaryColor(Color.FromRgb(27, 167, 132));
                    break;
                case "新禾绿":
                    theme.SetPrimaryColor(Color.FromRgb(210, 177, 22));
                    break;
                case "河豚灰":
                    theme.SetPrimaryColor(Color.FromRgb(57, 55, 51));
                    break;
                case "美人焦橙":
                    theme.SetPrimaryColor(Color.FromRgb(250, 126, 35));
                    break;
                case "榴子红":
                    theme.SetPrimaryColor(Color.FromRgb(241, 144, 140));
                    break;
                case "钢青":
                    theme.SetPrimaryColor(Color.FromRgb(20, 35, 52));
                    break;
                case "默认":
                    theme.SetPrimaryColor(Color.FromRgb(130, 58, 183));
                    break;

                default:
                    break;
            }
            paletteHelper.SetTheme(theme);
        }
        private void 派萌很生气(object sender, EventArgs e)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            if (DateTime.Now.Millisecond%2==0)
                theme.SetBaseTheme(Theme.Dark);
            else
            {
                theme.SetBaseTheme(Theme.Light);
            }
            paletteHelper.SetTheme(theme);
        }
    }
}
