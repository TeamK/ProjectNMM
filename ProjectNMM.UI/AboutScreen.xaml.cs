using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

namespace ProjectNMM.UI
{
    /// <summary>
    /// Interaction logic for AboutScreen.xaml
    /// </summary>
    public partial class AboutScreen : Window
    {
        /// <summary>
        /// Constructor for a new screen
        /// </summary>
        public AboutScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the hyperlink the standard browser
        /// </summary>
        private void HyperlinkLabel_OnClick(object sender, MouseEventArgs e)
        {
            Label label = (Label) sender;
            string url = "";
            

            switch (label.Name)
            {
                case "LblProjectSite":
                    url = "https://github.com/TeamK/ProjectNMM";
                    break;
                case "LblLicense":
                    url = "http://www.gnu.org/licenses/gpl-3.0";
                    break;
                case "LblIconSource":
                    url = "http://icomoon.io/";
                    break;
                case "LblIconLicense1":
                    url = "http://www.gnu.org/licenses/gpl.html";
                    break;
                case "LblIconLicense2":
                    url = "http://creativecommons.org/licenses/by/3.0/";
                    break;
            }

            if (!string.IsNullOrEmpty(url))
                Process.Start(url);
        }
    }
}
