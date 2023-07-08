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
        public Transform InputPoint => _viewInputPoint;
        public Transform OutputPoint => _viewOutputPoint;

        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameTextObject;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _unselectedColor;
        [SerializeField] private Transform _viewInputPoint;
        [SerializeField] private Transform _viewOutputPoint;

        private bool _isInited;
        private Button _button;
        private Technology _technology;
        
        public void Init(Technology technology, Sprite icon)
        {
            if(_isInited) throw new Exception("Try to init already inited TechnologyView");

            _isInited = true;
            _icon.sprite = icon;
            _technology = technology;
            _nameTextObject.text = technology.Name;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
            
            if (_technology.IsResearched()) 
                SetResearched();
        }
        public void UpdateView(Technology selectedTechnology)
        {
            SetSelected(_technology == selectedTechnology);
            if (_technology.IsResearched())
                SetResearched();
        }

        private void OnButtonClicked()
        {
            Selected?.Invoke(_technology);
        }
        
        private void SetSelected(bool isSelected)
        {
            _button.image.color = isSelected ? _selectedColor : _unselectedColor;
        }
        private void SetResearched()
        {
            _button.interactable = false;
        }
    }
}