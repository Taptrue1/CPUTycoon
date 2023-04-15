using System.Collections.Generic;

namespace Core.UI.WindowsStateMachines
{
    public class WindowsStateMachine
    {
        private WindowPresenter _currentWindow;
        private readonly List<WindowPresenter> _windows;
        
        public WindowsStateMachine(List<WindowPresenter> windows)
        {
            _windows = windows;
        }
        public void SwitchWindow<TWindow>() where TWindow : WindowPresenter
        {
            var window = _windows.Find(x => x is TWindow);
            
            if(window == null)
                throw new System.Exception($"Window {typeof(TWindow)} not found in windows list");
            
            _currentWindow?.Hide();
            _currentWindow = window;
            _currentWindow.Show();
        }
    }
}