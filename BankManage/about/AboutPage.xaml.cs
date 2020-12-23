using System;
using System.Collections.Generic;
using System.IO;
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

namespace BankManage.about
{
    /// <summary>
    /// AboutPage.xaml 的交互逻辑
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
            initLicense("1");
        }

        private void initLicense(string name)
        {
            {
                string textFile = @".\about\LICENSE\1.txt";
                FileStream fs;
                if (File.Exists(textFile))
                {
                    fs = new FileStream(textFile, FileMode.Open, FileAccess.Read);
                    using (fs)
                    {
                        TextRange text = new TextRange(EF6RTB.Document.ContentStart, EF6RTB.Document.ContentEnd);
                        TextRange text2 = new TextRange(WAGRTB.Document.ContentStart, WAGRTB.Document.ContentEnd);

                        text.Load(fs, DataFormats.Text);
                        text2.Load(fs, DataFormats.Text);
                    }
                }
            }
            {
                string textFile = @".\about\LICENSE\2.txt";
                FileStream fs;
                if (File.Exists(textFile))
                {
                    fs = new FileStream(textFile, FileMode.Open, FileAccess.Read);
                    using (fs)
                    {
                        TextRange text = new TextRange(MDIXTRTB.Document.ContentStart, MDIXTRTB.Document.ContentEnd);

                        text.Load(fs, DataFormats.Text);
                    }
                }
            }
        }
    }
}
