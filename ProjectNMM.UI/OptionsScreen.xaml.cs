using System.Windows;
using ProjectNMM.Model;
using ProjectNMM.UI.Properties;

namespace ProjectNMM.UI
{
    /// <summary>
    /// Interaction logic for OptionsScreen.xaml
    /// </summary>
    public partial class OptionsScreen : Window
    {
        /// <summary>
        /// Creates a new screen
        /// </summary>
        public OptionsScreen()
        {
            InitializeComponent();

            TxtDefaultDescription.Text = Settings.Default.DefaultDescription;
        }

        /// <summary>
        /// Event for "Save" button, saves settings
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.DefaultDescription =
                ModelHelpFunctions.RemoveSpecialCharacters(TxtDefaultDescription.Text);

            LblStatus.Content = "Die Optionen wurden erfolgreich gespeichert.";
        }
    }
}
