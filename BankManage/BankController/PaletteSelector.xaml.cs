using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace BankManage
{
    /// <summary>
    /// PaletteSelector.xaml 的交互逻辑
    /// </summary>
    public partial class PaletteSelector : UserControl
    {
        public PaletteSelector()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var paletteHelper = new PaletteHelper();
            //Retrieve the app's existing theme
            ITheme theme = paletteHelper.GetTheme();

            //Change all of the primary colors to Red
            //theme.SetPrimaryColor(Colors.Red);

            //Change all of the secondary colors to Blue
            //theme.SetSecondaryColor(Colors.Blue);

            //You can also change a single color on the theme, and optionally set the corresponding foreground color
            //theme.PrimaryMid = new ColorPair(Colors.Brown, Colors.White);


            Button item = e.Source as Button;

            switch (item.Tag)
            {
                case "玉红":
                    theme.SetPrimaryColor(Color.FromRgb(192,72,81));
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
            MainSettings mainSettings = MainSettings.Default;
            MainSettings.Default.主题 = item.Tag.ToString();
            mainSettings.Save();
            paletteHelper.SetTheme(theme);

        }
    }
}
