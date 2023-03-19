using System;
using TMPro;
using UnityEngine;
using Utils.CustomNumbers;
using Button = UnityEngine.UI.Button;

namespace Core.Custom
{
    public class CustomInputField : MonoBehaviour
    {
        [SerializeField] private Button _increaseButton;
        [SerializeField] private Button _decreaseButton;
        [SerializeField] private TextMeshProUGUI _valueTextObject;
        [SerializeField] private string _format = "{0}";
        
        private double _minValue;
        private double _maxValue;
        private double _changeValue;
        private CustomNumber<double> _currentValue;

        private void Awake()
        {
            _currentValue = new CustomNumber<double>();
            
            _currentValue.Changed += OnValueChanged;
            
            _increaseButton.onClick.AddListener(OnIncreaseButtonClicked);
            _decreaseButton.onClick.AddListener(OnDecreaseButtonClicked);
        }

        public void SetBounds(double min, double max)
        {
            _minValue = min;
            _maxValue = max;
            
            SetValue(_currentValue.Value);
        }
        public void SetChangeValue(double value)
        {
            _changeValue = value;
        }

        private void OnValueChanged(double value)
        {
            _valueTextObject.text = string.Format(_format, value);
        }
        private void OnIncreaseButtonClicked()
        {
            SetValue(_currentValue.Value + _changeValue);
        }
        private void OnDecreaseButtonClicked()
        {
            SetValue(_currentValue.Value - _changeValue);
        }

        private void SetValue(double value)
        {
            _currentValue.Value = Math.Clamp(value, _minValue, _maxValue);
        }
    }
}