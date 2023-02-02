using System;
using Core.Custom;
using UnityEngine;
using UnityEngine.UI;
using Utils.CustomNumbers;

namespace Core.CPU
{
    public class ProcessorBuilderView : MonoBehaviour
    {
        public event Action CreateButtonClicked;

        [SerializeField] private Button _createProcessorButton;
        
        [Header("TechProcess")]
        [SerializeField] private Utils.Bounds _techProcessBounds;
        [SerializeField] private CustomInputField _techProcessInputField;

        [Header("Cache")]
        [SerializeField] private Utils.Bounds _cacheBounds;
        [SerializeField] private CustomInputField _cacheInputField;

        [Header("Frequency")] 
        [SerializeField] private Utils.Bounds _frequencyBounds;
        [SerializeField] private CustomInputField _frequencyInputField;

        public void Init(CustomNumber<float> techProcess, CustomNumber<float> cache, CustomNumber<float> frequency)
        {
            var techProcessChangeStep = 1;
            var cacheChangeStep = (int)(_cacheBounds.Max - _cacheBounds.Min) / 10;
            var frequencyChangeStep = (int)(_frequencyBounds.Max - _frequencyBounds.Min) / 10;
            
            _techProcessInputField.Init(techProcess, _techProcessBounds, techProcessChangeStep);
            _cacheInputField.Init(cache, _cacheBounds, cacheChangeStep);
            _frequencyInputField.Init(frequency, _frequencyBounds, frequencyChangeStep);
            
            _createProcessorButton.onClick.RemoveAllListeners();
            _createProcessorButton.onClick.AddListener(OnCreateProcessorButtonClick);
        }
        
        private void OnCreateProcessorButtonClick()
        {
            CreateButtonClicked?.Invoke();
        }
    }
}