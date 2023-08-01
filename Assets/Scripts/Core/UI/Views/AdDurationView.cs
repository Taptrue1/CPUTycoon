using System;
using Core.Marketing;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views
{
    public class AdDurationView : MonoBehaviour
    {
        public event Action<int> DurationChanged;
        
        [SerializeField] private Slider _slider;
        [SerializeField] private DurationPointView[] _pointViews;
        [SerializeField] private TextMeshProUGUI _durationTextObject;

        [Header("Point Settings")]
        [SerializeField] private Sprite _selectedPoint;
        [SerializeField] private Sprite _unselectedPoint;

        private MarketingSettings _marketingSettings;
        private const int PointsCount = 5;

        private void Awake()
        {
            foreach (var pointView in _pointViews)
            {
                pointView.Clicked += OnPointClicked;
            }
        }
        
        public void Init(MarketingSettings marketingSettings)
        {
            _marketingSettings = marketingSettings;
            _slider.maxValue = PointsCount - 1;
        }
        public void ResetView()
        {
            SetPoint(0);
        }

        private void OnPointClicked(DurationPointView pointView)
        {
            var index = Array.IndexOf(_pointViews, pointView);
            SetPoint(index);
        }

        private void SetPoint(int index)
        {
            _slider.value = index;
            _durationTextObject.text = _marketingSettings.AdDurations[index].Name;
            for (var i = 0; i < _pointViews.Length; i++)
            {
                _pointViews[i].Image.sprite = i <= index ? _selectedPoint : _unselectedPoint;
            }
            DurationChanged?.Invoke(index);
        }
    }
}