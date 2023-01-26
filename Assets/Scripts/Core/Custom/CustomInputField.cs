using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Custom
{
    public class CustomInputField : MonoBehaviour
    {
        public event Action<float> ValueChanged;
        public float ChangeStep;

        [SerializeField] private Button _increaseButton;
        [SerializeField] private Button _decreaseButton;

        private void Awake()
        {
            _increaseButton.onClick.AddListener(OnIncreaseButtonClick);
            _decreaseButton.onClick.AddListener(OnDecreaseButtonClick);
        }
        
        private void OnIncreaseButtonClick()
        {
            ValueChanged?.Invoke(ChangeStep);
        }
        private void OnDecreaseButtonClick()
        {
            ValueChanged?.Invoke(-ChangeStep);
        }
    }
}