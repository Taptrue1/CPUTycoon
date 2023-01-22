using System;
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
        [SerializeField] private Button _increaseTechProcessButton;
        [SerializeField] private Button _decreaseTechProcessButton;
        [SerializeField] private TextMeshProUGUI _techProcessTextObject;
        
        [Header("Cache")]
        [SerializeField] private Utils.Bounds _cacheBounds;
        [SerializeField] private Button _increaseCacheButton;
        [SerializeField] private Button _decreaseCacheButton;
        [SerializeField] private TextMeshProUGUI _cacheTextObject;

        [Header("Frequency")] 
        [SerializeField] private Utils.Bounds _frequencyBounds;
        [SerializeField] private Button _increaseFrequencyButton;
        [SerializeField] private Button _decreaseFrequencyButton;
        [SerializeField] private TextMeshProUGUI _frequencyTextObject;

        private CustomNumber<float> _currentCache;
        private CustomNumber<int> _currentFrequency;
        private CustomNumber<int> _currentTechProcess;
        private int _frequencyChangeStep;
        private float _cacheChangeStep;

        private void Awake()
        {
            _currentTechProcess = new CustomNumber<int>() { Value = Mathf.CeilToInt(_techProcessBounds.Min) };
            _currentCache = new CustomNumber<float>() { Value = _cacheBounds.Min };
            _currentFrequency = new CustomNumber<int>() { Value = Mathf.CeilToInt(_frequencyBounds.Min) };
            _frequencyChangeStep = Mathf.CeilToInt((_frequencyBounds.Max - _frequencyBounds.Min) / 10);
            _cacheChangeStep = (_cacheBounds.Max - _cacheBounds.Min) / 10;

            _currentTechProcess.Changed += OnTechProcessChanged;
            _currentCache.Changed += OnCacheChanged;
            _currentFrequency.Changed += OnFrequencyChanged;

            _createProcessButton.onClick.AddListener(OnCreateProcessButtonClick);
            _increaseTechProcessButton.onClick.AddListener(OnIncreaseTechProcessButtonClick);
            _decreaseTechProcessButton.onClick.AddListener(OnDecreaseTechProcessButtonClick);
            _increaseCacheButton.onClick.AddListener(OnIncreaseCacheButtonClick);
            _decreaseCacheButton.onClick.AddListener(OnDecreaseCacheButtonClick);
            _increaseFrequencyButton.onClick.AddListener(OnIncreaseFrequencyButtonClick);
            _decreaseFrequencyButton.onClick.AddListener(OnDecreaseFrequencyButtonClick);

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
        private void OnIncreaseTechProcessButtonClick()
        {
            _currentTechProcess.Value = Mathf.Clamp(_currentTechProcess.Value + 1, (int)_techProcessBounds.Min, (int)_techProcessBounds.Max);
        }
        private void OnDecreaseTechProcessButtonClick()
        {
            _currentTechProcess.Value = Mathf.Clamp(_currentTechProcess.Value - 1, (int)_techProcessBounds.Min, (int)_techProcessBounds.Max);
        }
        private void OnIncreaseCacheButtonClick()
        {
            _currentCache.Value = Mathf.Clamp(_currentCache.Value + _cacheChangeStep, _cacheBounds.Min, _cacheBounds.Max);
        }
        private void OnDecreaseCacheButtonClick()
        {
            _currentCache.Value = Mathf.Clamp(_currentCache.Value - _cacheChangeStep, _cacheBounds.Min, _cacheBounds.Max);
        }
        private void OnIncreaseFrequencyButtonClick()
        {
            _currentFrequency.Value = Mathf.Clamp(_currentFrequency.Value + _frequencyChangeStep, (int)_frequencyBounds.Min, (int)_frequencyBounds.Max);
        }
        private void OnDecreaseFrequencyButtonClick()
        {
            _currentFrequency.Value = Mathf.Clamp(_currentFrequency.Value - _frequencyChangeStep, (int)_frequencyBounds.Min, (int)_frequencyBounds.Max);
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