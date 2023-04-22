using System.Collections;
using System.Collections.Generic;
using Core.UI;
using Core.UI.WindowsStateMachines;
using Zenject;

namespace Core.Services
{
    public class UIService
    {
        private WindowsStateMachine _windowsStateMachine;
        private readonly UIFactory _uiFactory;

        [Inject]
        public UIService(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }
        public void InitializeWindows<TWindow>(IEnumerable<WindowPresenter> windows) where TWindow : WindowPresenter
        {
            _windowsStateMachine = new(_uiFactory.CreateWindows(windows));
            _windowsStateMachine.SwitchWindow<TWindow>();
        }
        public void ShowWindow<TWindow>() where TWindow : WindowPresenter
        {
            _windowsStateMachine.SwitchWindow<TWindow>();
        }
    }
}