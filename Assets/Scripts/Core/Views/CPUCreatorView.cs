using System;
using System.Collections.Generic;
using Core.CPU;
using Core.Custom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views
{
    public class CPUCreatorView : MonoBehaviour
    {
        public event Action<Processor> OnProcessorCreated;

        [SerializeField] private TextMeshProUGUI _coresTextObject;
        [SerializeField] private TextMeshProUGUI _cacheTextObject;
        [SerializeField] private TextMeshProUGUI _frequencyTextObject;
        [SerializeField] private Slider _coresSlider;
        [SerializeField] private Slider _cacheSlider;
        [SerializeField] private Slider _frequencySlider;
        [SerializeField] private TMP_Dropdown _frequencyTypeDropdown;
        [SerializeField] private Button _createCPUButton;
        
        [Header("Tests")]
        [SerializeField] private CustomInputField _testInputField;

        private int _minFrequency;
        private int _maxFrequency;

        private void Awake()
        {
            _testInputField.SetBounds(1000000, 4000000);
            _testInputField.SetChangeValue((4000000 - 1000000) / 10);
            
            _frequencyTypeDropdown.options = new List<TMP_Dropdown.OptionData> { new("KHz"), new("MHz"), new("GHz") };

            _coresSlider.onValueChanged.AddListener(OnCoresSliderChanged);
            _cacheSlider.onValueChanged.AddListener(OnCacheSliderChanged);
            _frequencySlider.onValueChanged.AddListener(OnFrequencySliderChanged);
            _frequencyTypeDropdown.onValueChanged.AddListener(OnFrequencyTypeChanged);

            SetCoresBounds(1, 4);
            SetCacheBounds(1, 15);
            SetFrequencyBounds(1000000, 4000000);
        }
        
        public void SetCoresBounds(int min, int max)
        {
            _coresSlider.minValue = min;
            _coresSlider.maxValue = max;
        }
        public void SetCacheBounds(int min, int max)
        {
            _cacheSlider.minValue = min;
            _cacheSlider.maxValue = max;
        }
        public void SetFrequencyBounds(int min, int max)
        {
            _frequencySlider.minValue = min;
            _frequencySlider.maxValue = max;
            
            _minFrequency = min;
            _maxFrequency = max;
        }

        private void OnFrequencyTypeChanged(int index)
        {
            switch (index)
            {
                case 0: // KHz
                    _frequencySlider.minValue = _minFrequency;
                    _frequencySlider.maxValue = _maxFrequency;
                    break;
                case 1: // MHz
                    _frequencySlider.minValue = Mathf.CeilToInt(_minFrequency / 1000);
                    _frequencySlider.maxValue = Mathf.CeilToInt(_maxFrequency / 1000);
                    break;
                case 2: // GHz
                    _frequencySlider.minValue = Mathf.CeilToInt(_minFrequency / 1000000);
                    _frequencySlider.maxValue = Mathf.CeilToInt(_maxFrequency / 1000000);
                    break;
            }
        }
        
        private void OnCoresSliderChanged(float value)
        {
            _coresTextObject.text = $"{value}";
        }
        private void OnCacheSliderChanged(float value)
        {
            _cacheTextObject.text = value % 1 != 0 ? $"{value:0.0}" : $"{value:0}";
        }
        private void OnFrequencySliderChanged(float value)
        {
            var formattedValue = value % 1 != 0 ? $"{value:0.0}" : $"{value:0}";
            
            _frequencyTextObject.text = $"{formattedValue} {_frequencyTypeDropdown.options[_frequencyTypeDropdown.value].text}";
        }
    }
}