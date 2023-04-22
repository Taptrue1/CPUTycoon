using System;
using Core.Technologies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views
{
    [RequireComponent(typeof(Button))]
    public class TechnologyView : MonoBehaviour
    {
        public event Action<Technology> Selected;
        
        public Technology Technology => _technology;

        [SerializeField] private TextMeshProUGUI _nameTextObject;
        [SerializeField] private TextMeshProUGUI _levelTextObject;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;

        private Button _button;
        private Technology _technology;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }
        
        public void SetTechnology(Technology technology)
        {
            _technology = technology;
            _nameTextObject.text = technology.Name;
            _levelTextObject.text = technology.Level.Value.ToString();

            _technology.Level.Changed += OnTechnologyLevelChanged;
        }
        public void SetSelected(bool isSelected)
        {
            _button.image.color = isSelected ? _selectedColor : _unselectedColor;
        }

        private void OnButtonClicked()
        {
            Selected?.Invoke(_technology);
        }
        private void OnTechnologyLevelChanged()
        {
            _levelTextObject.text = _technology.Level.Value.ToString();
        }
    }
}