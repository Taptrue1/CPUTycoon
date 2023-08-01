using System;
using Core.Marketing;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views
{
    [RequireComponent(typeof(Button))]
    public class AdView : MonoBehaviour
    {
        public event Action<AdData, bool> SelectionChanged;

        [Header("Scale Animation Settings")]
        [SerializeField] private float _defaultScale = 1;
        [SerializeField] private float _extendedScale = 1.2f;
        
        [Header("Dependencies")]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameTextObject;
        [SerializeField] private TextMeshProUGUI _priceTextObject;

        private bool _isSelected;
        private AdData _adData;
        private Button _button;
        private Tween _scaleTween;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        public void Init(AdData adData)
        {
            _adData = adData;
            _icon.sprite = adData.Icon;
            _nameTextObject.text = adData.Name;
            _priceTextObject.text = $"${adData.Price:#,##0}".Replace(",", " ");
        }
        public void ResetView()
        {
            _isSelected = false;
            Scale();
            SelectionChanged?.Invoke(_adData, _isSelected);
        }

        private void OnButtonClick()
        {
            _isSelected = !_isSelected;
            Scale();
            SelectionChanged?.Invoke(_adData, _isSelected);
        }
        private void Scale()
        {
            //_layoutElement.flexibleWidth = _isSelected ? _extendedScale : _defaultScale;
            //_layoutElement.flexibleHeight = _isSelected ? _extendedScale : _defaultScale;
        }
        
    }
}