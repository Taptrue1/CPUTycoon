using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.CustomNumbers;

namespace Core.Custom
{
    public class CustomInputField : MonoBehaviour
    {
        [SerializeField] private Button _increaseButton;
        [SerializeField] private Button _decreaseButton;
        [SerializeField] private TextMeshProUGUI _valueTextObject;
        [SerializeField] private string _valueTextFormat = "{0:0.00}";

        private Utils.Bounds _bounds;
        private CustomNumber<float> _customNumber;
        private float _changeStep;
        
        
        public void Init(CustomNumber<float> customNumber,  Utils.Bounds bounds, float changeStep)
        {
            _bounds = bounds;
            _customNumber = customNumber;
            _changeStep = changeStep;

            _customNumber.Value = _bounds.Min;
            
            _customNumber.Changed += UpdateText;
            
            _increaseButton.onClick.RemoveAllListeners();
            _decreaseButton.onClick.RemoveAllListeners();
            _increaseButton.onClick.AddListener(OnIncreaseButtonClick);
            _decreaseButton.onClick.AddListener(OnDecreaseButtonClick);
            
            UpdateText(_customNumber.Value);
        }
        
        private void OnIncreaseButtonClick()
        {
            _customNumber.Value = Mathf.Clamp(_customNumber.Value + _changeStep, _bounds.Min, _bounds.Max);
        }
        private void OnDecreaseButtonClick()
        {
            _customNumber.Value = Mathf.Clamp(_customNumber.Value - _changeStep, _bounds.Min, _bounds.Max);
        }
        private void UpdateText(float value)
        {
            _valueTextObject.text = string.Format(_valueTextFormat, value);
        }
    }
}