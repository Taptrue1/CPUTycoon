using System;
using TMPro;
using UnityEngine;

namespace Core.Custom
{
    public class CustomDateTimeView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textObject;
        [SerializeField] private string _format = "dd.MM.yyyy";

        public void Init(CustomDateTime dateTime)
        {
            dateTime.DateTimeChanged += OnDateTimeChanged;
            
            OnDateTimeChanged(dateTime.DateTime);
        }

        private void OnDateTimeChanged(DateTime dateTime)
        {
            _textObject.text = dateTime.ToString(_format);
        }
    }
}