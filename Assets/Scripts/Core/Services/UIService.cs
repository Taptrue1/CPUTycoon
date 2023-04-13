using System.Collections.Generic;
using Core.UI;

namespace Core.Services
{
    public class UIService
    {
        private List<WindowPresenter> _windows;
        private readonly UIFactory _uiFactory;
        
        public UIService(UIFactory uiFactory, IEnumerable<WindowPresenter> windows)
        {
            _uiFactory = uiFactory;
            _windows = _uiFactory.CreateWindows(windows);
        }
    }
}