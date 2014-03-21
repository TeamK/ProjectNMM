using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Windows;
using ProjectNMM.Model;

namespace ProjectNMM.UI
{
    class UiControlHandler
    {
        
        
        public UiControlHandler(StartScreen startScreen)
        {
            Application.Current.Dispatcher.Invoke(Action .Close());
        }
    }
}
