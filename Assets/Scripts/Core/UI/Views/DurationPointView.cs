using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views
{
    [RequireComponent(typeof(Button))]
    public class DurationPointView : MonoBehaviour
    {
        public event Action<DurationPointView> Clicked;
        public Image Image { get; private set; }

        private Button _button;

        private void Awake()
        {
            Image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => Clicked?.Invoke(this));
        }
    }
}