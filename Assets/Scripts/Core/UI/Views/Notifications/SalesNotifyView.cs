using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Views.Notifications
{
    public class SalesNotifyView : MonoBehaviour
    {
        [SerializeField] private int _activeSalesSlidersCount;
        [SerializeField] private TextMeshProUGUI _titleTextObject;
        [SerializeField] private TextMeshProUGUI _descriptionTextObject;
        [SerializeField] private Transform _salesSlidersContainer;
        [SerializeField] private Slider _salesSliderPrefab;
        
        private List<Slider> _salesSliders;
        private const string ProfitFormat = "Profit: {0}";
        private const string SalesDurationFormat = "Days selling: {0}";
        private const string UnitsSoldFormat = "Units sold: {0}";

        public void MonthChanged()
        {
            _salesSliders.Add(Instantiate(_salesSliderPrefab, _salesSlidersContainer));
        }
    }
}