using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ProjectNMM.Model;

namespace ProjectNMM.UI
{
    public delegate void StartNewGame(GameType gameType);
    public delegate void OnClickEvent(object sender, MouseEventArgs e);
    public delegate void OnCancelEvent(object sender, CancelEventArgs e);
    public delegate void StandardMethod();
    public delegate bool StandardMethodWithBool();

    public struct UiDelegateCollection
    {
        public StartNewGame NewGame;
        public OnClickEvent EllipseClick;
        public OnCancelEvent CloseMainWindow;
        public StandardMethodWithBool SaveGame;
        public StandardMethod LoadGame;
        public StandardMethod Undo;
        public StandardMethod Redo;
        public StandardMethod ShowOptions;
        public StandardMethod ShowAbouts;
    };
}
