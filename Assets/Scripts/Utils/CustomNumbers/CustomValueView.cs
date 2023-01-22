using TMPro;
using UnityEngine;

namespace Utils.CustomNumbers
{
    class CustomValueView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textObject;
        [SerializeField] private string _format;

        private ICustomNumber _customNumber;

        public void Init(ICustomNumber customNumber)
        {
            _customNumber = customNumber;
            _customNumber.Changed += OnCustomValueChanged;
            
            OnCustomValueChanged();
        }

        private void OnCustomValueChanged()
        {
            _textObject.text = _customNumber.GetFormatedString(_format);
        }
    }
}