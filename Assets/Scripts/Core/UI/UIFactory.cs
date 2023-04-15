using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.UI
{
    public class UIFactory
    {
        private readonly Transform _canvasTransform;

        public UIFactory(Canvas canvas)
        {
            _canvasTransform = canvas.transform;
        }
        public List<WindowPresenter> CreateWindows(IEnumerable<WindowPresenter> windowPrefabs)
        {
            return windowPrefabs.Select(CreateWindow).ToList();
        }
        
        private WindowPresenter CreateWindow(WindowPresenter windowPrefab)
        {
            var window = Object.Instantiate(windowPrefab, _canvasTransform);
            window.Hide();
            return window;
        }
    }
}