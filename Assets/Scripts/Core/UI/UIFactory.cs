using System.Collections.Generic;
using System.Linq;
using Core.UI.Views;
using Settings;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class UIFactory
    {
        private readonly UISettings _uiSettings;
        private readonly DiContainer _container;
        private readonly Transform _canvasTransform;

        [Inject]
        public UIFactory(DiContainer container, Canvas canvas, CoreSettings coreSettings)
        {
            _container = container;
            _canvasTransform = canvas.transform;
            _uiSettings = coreSettings.UISettings;
        }
        public List<WindowPresenter> CreateWindows(IEnumerable<WindowPresenter> windowPrefabs)
        {
            return windowPrefabs.Select(CreateWindow).ToList();
        }
        public TechnologyView CreateTechnologyView(Transform parent)
        {
            return Object.Instantiate(_uiSettings.TechnologyView, parent);
        }
        
        private WindowPresenter CreateWindow(WindowPresenter windowPrefab)
        {
            var window = Object.Instantiate(windowPrefab, _canvasTransform);
            window.gameObject.SetActive(false);
            _container.Inject(window);
            return window;
        }
    }
}