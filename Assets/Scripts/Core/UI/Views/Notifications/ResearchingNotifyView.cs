using Core.Datas;
using Core.Technologies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views.Notifications
{
    public class ResearchingNotifyView : MonoBehaviour
    {
        [SerializeField] private Image _iconImageObject;
        [SerializeField] private TextMeshProUGUI _titleTextObject;
        [SerializeField] private Slider _researchingProgressSlider;
        [SerializeField] private CurrencyData _rpCurrencyData;

        public void Init(Technology technology)
        {
            _iconImageObject.sprite = null;
            _titleTextObject.text = technology.Name;
        }
    }
}