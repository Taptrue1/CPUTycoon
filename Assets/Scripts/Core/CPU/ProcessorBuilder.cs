using System;
using Core.Custom;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Utils.CustomNumbers;

namespace Core.CPU
{
    public class ProcessorBuilder : MonoBehaviour
    {
        public Action<Processor> ProcessorCreated;
        
        [SerializeField] private GameObject _processorBuilderPanel;
        [SerializeField] private Button _createProcessButton;
        
        [Header("TechProcess")]
        [SerializeField] private Utils.Bounds _techProcessBounds;
        [SerializeField] private CustomInputField _techProcessInputField;
        [SerializeField] private TextMeshProUGUI _techProcessTextObject;
        
        [Header("Cache")]
        [SerializeField] private Utils.Bounds _cacheBounds;
        [SerializeField] private CustomInputField _cacheInputField;
        [SerializeField] private TextMeshProUGUI _cacheTextObject;

        [Header("Frequency")] 
        [SerializeField] private Utils.Bounds _frequencyBounds;
        [SerializeField] private CustomInputField _frequencyInputField;
        [SerializeField] private TextMeshProUGUI _frequencyTextObject;

        private CustomNumber<float> _currentCache;
        private CustomNumber<int> _currentFrequency;
        private CustomNumber<int> _currentTechProcess;

        private void Awake()
        {
            _currentTechProcess = new CustomNumber<int>() { Value = Mathf.CeilToInt(_techProcessBounds.Min) };
            _currentCache = new CustomNumber<float>() { Value = _cacheBounds.Min };
            _currentFrequency = new CustomNumber<int>() { Value = Mathf.CeilToInt(_frequencyBounds.Min) };

            _techProcessInputField.ChangeStep = 1;
            _frequencyInputField.ChangeStep = Mathf.CeilToInt((_frequencyBounds.Max - _frequencyBounds.Min) / 10);
            _cacheInputField.ChangeStep = (_cacheBounds.Max - _cacheBounds.Min) / 10;
            
            _currentTechProcess.Changed += OnTechProcessChanged;
            _currentCache.Changed += OnCacheChanged;
            _currentFrequency.Changed += OnFrequencyChanged;
            _techProcessInputField.ValueChanged += OnTechProcessInputFieldValueChanged;
            _cacheInputField.ValueChanged += OnCacheInputFieldValueChanged;
            _frequencyInputField.ValueChanged += OnFrequencyInputFieldValueChanged;

            _createProcessButton.onClick.AddListener(OnCreateProcessButtonClick);

            OnTechProcessChanged(_currentTechProcess.Value);
            OnCacheChanged(_currentCache.Value);
            OnFrequencyChanged(_currentFrequency.Value);
        }

        public void Activate()
        {
            _processorBuilderPanel.SetActive(true);
        }

        private void OnCreateProcessButtonClick()
        {
            var processor = new Processor(_currentTechProcess.Value, _currentFrequency.Value, (int)_currentCache.Value);
            
            _processorBuilderPanel.SetActive(false);
            
            ProcessorCreated?.Invoke(processor);
        }
        private void OnTechProcessInputFieldValueChanged(float value)
        {
            _currentTechProcess.Value = Mathf.Clamp(_currentTechProcess.Value + Mathf.CeilToInt(value),
                (int)_techProcessBounds.Min, (int)_techProcessBounds.Max);
        }
        private void OnCacheInputFieldValueChanged(float value)
        {
            _currentCache.Value = Mathf.Clamp(_currentCache.Value + value, _cacheBounds.Min, _cacheBounds.Max);
        }
        private void OnFrequencyInputFieldValueChanged(float value)
        {
            _currentFrequency.Value = Mathf.Clamp(_currentFrequency.Value + Mathf.CeilToInt(value),
                (int)_frequencyBounds.Min, (int)_frequencyBounds.Max);
        }
        private void OnTechProcessChanged(int value)
        {
            _techProcessTextObject.text = $"{value}";
        }
        private void OnCacheChanged(float value)
        {
            _cacheTextObject.text = $"{value:0.0}";
        }
        private void OnFrequencyChanged(int value)
        {
            _frequencyTextObject.text = $"{value:0.0}";
        }
    }
}