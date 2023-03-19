using System;
using System.Globalization;
using Core.Technologies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views
{
    public class TechnologyView : MonoBehaviour
    {
        public Action<Technology> ButtonClicked;
        
        [SerializeField] private TextMeshProUGUI _nameTextObject;
        [SerializeField] private TextMeshProUGUI _descriptionTextObject;
        [SerializeField] private TextMeshProUGUI _levelTextObject;
        [SerializeField] private Button _button;
        
        private Technology _technology;

        public void Init(Technology technology)
        {
            _technology = technology;
            _nameTextObject.text = technology.Name;
            _descriptionTextObject.text = technology.Description;
            _levelTextObject.text = technology.Level.ToString(CultureInfo.CurrentCulture);
            
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            ButtonClicked?.Invoke(_technology);
        }
    }
}