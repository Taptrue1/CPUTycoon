using System.Collections.Generic;
using Core.UI;
using Core.UI.WindowsStateMachines;

namespace Core.Services
{
    public class UIService
    {
        private readonly UIFactory _uiFactory;
        private readonly WindowsStateMachine _windowsStateMachine;
        
        public UIService(UIFactory uiFactory, IEnumerable<WindowPresenter> windows)
        {
            _uiFactory = uiFactory;
            _windowsStateMachine = new(_uiFactory.CreateWindows(windows));
        }
        
        public void ShowWindow<TWindow>() where TWindow : WindowPresenter
        {
            _windowsStateMachine.SwitchWindow<TWindow>();
        }
    }
}