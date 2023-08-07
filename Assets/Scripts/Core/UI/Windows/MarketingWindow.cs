using System;
using System.Collections.Generic;
using System.Linq;
using Core.Datas;
using Core.Marketing;
using Core.Services;
using Core.UI.Views;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Windows
{
    public class MarketingWindow : WindowPresenter
    {
        [Header("Settings")]
        [SerializeField] private string _totalPriceFormat = "Total Price{0}${1}";
        [Header("Dependencies")]
        [SerializeField] private Transform _adViewsContainer;
        [SerializeField] private AdDurationView _adDurationView;
        [SerializeField] private TextMeshProUGUI _totalPriceTextObject;
        [SerializeField] private CurrencyData _moneyCurrencyData;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _acceptButton;

        private int _selectedDurationIndex;
        private double _adsTotalPrice;
        private List<AdData> _selectedAds = new();
        private List<AdView> _adViews = new();
        private UIFactory _uiFactory;
        private UIService _uiService;
        private MarketService _marketService;
        private CurrencyService _currencyService;
        private MarketingSettings _marketingSettings;

        private void Awake()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _acceptButton.onClick.AddListener(OnAcceptButtonClicked);
        }

        [Inject]
        public void InjectDependencies(UIFactory uiFactory, UIService uiService, MarketService marketService,
            CurrencyService currencyService, CoreSettings coreSettings)
        {
            _uiFactory = uiFactory;
            _uiService = uiService;
            _marketService = marketService;
            _currencyService = currencyService;
            _marketingSettings = coreSettings.MarketingSettings;
            _adDurationView.Init(_marketingSettings);
            _adDurationView.DurationChanged += OnDurationChanged;
            SpawnAdViews();
        }
        public override void Show()
        {
            base.Show();
            foreach(var adView in _adViews)
                adView.ResetView();
            _adDurationView.ResetView();
        }

        #region Callbacks
        private void OnCloseButtonClicked()
        {
            _uiService.ShowWindow<CoreWindow>();
        }
        private void OnAcceptButtonClicked()
        {
            var totalSalesBonus = _selectedAds.Sum(ad => ad.SalesMultiplier);
            _currencyService.GetCurrency(_moneyCurrencyData.Name).Value -= _adsTotalPrice;
            _marketService.ActivateAdBonus(totalSalesBonus, _marketingSettings.AdDurations[_selectedDurationIndex].Duration);
            _uiService.ShowWindow<CoreWindow>();
        }
        private void OnDurationChanged(int durationIndex)
        {
            _selectedDurationIndex = durationIndex;
            UpdateTotalPrice();
            UpdateInformation();
        }
        private void OnSelectionChanged(AdData adData, bool isSelected)
        {
            if(isSelected)
                _selectedAds.Add(adData);
            else
                _selectedAds.Remove(adData);
            UpdateTotalPrice();
            UpdateInformation();
            _acceptButton.interactable = _selectedAds.Count > 0 && _marketService.PlayerProduct != null &&
                                         _currencyService.GetCurrency(_moneyCurrencyData.Name).Value >= _adsTotalPrice;
        }
        #endregion
        
        #region Other
        private void SpawnAdViews()
        {
            foreach(var adData in _marketingSettings.Ads)
            {
                var adView = _uiFactory.CreateAdView(_adViewsContainer);
                adView.Init(adData);
                adView.SelectionChanged += OnSelectionChanged;
                _adViews.Add(adView);
            }
        }
        private void UpdateTotalPrice()
        {
            _adsTotalPrice = _selectedAds.Sum(ad => ad.Price) *
                             _marketingSettings.AdDurations[_selectedDurationIndex].PriceMultiplier;
        }
        private void UpdateInformation()
        {
            var formattedTotalPrice = $"{_adsTotalPrice:#,##0}".Replace(",", " ");
            _totalPriceTextObject.text = string.Format(_totalPriceFormat, Environment.NewLine, formattedTotalPrice);
        }
        #endregion
    }
}