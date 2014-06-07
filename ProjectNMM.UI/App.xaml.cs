using System.Windows;

namespace ProjectNMM.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private UiControl _uiControl;
        
        /// <summary>
        /// Standard start point for program
        /// </summary>
        public App()
        {
            _uiControl = new UiControl();

            _uiControl.StartGame();
        }
    }
}
