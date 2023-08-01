using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _acceptButton;

        private int _selectedDurationIndex;
        private List<AdData> _selectedAds = new();
        private List<AdView> _adViews = new();
        private UIFactory _uiFactory;
        private UIService _uiService;
        private MarketService _marketService;
        private MarketingSettings _marketingSettings;

        private void Awake()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _acceptButton.onClick.AddListener(OnAcceptButtonClicked);
        }

        [Inject]
        public void InjectDependencies(UIFactory uiFactory, UIService uiService, MarketService marketService,
            CoreSettings coreSettings)
        {
            _uiFactory = uiFactory;
            _uiService = uiService;
            _marketService = marketService;
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
            //TODO subtract money and  and close current window
            _uiService.ShowWindow<CoreWindow>();
        }
        private void OnDurationChanged(int durationIndex)
        {
            _selectedDurationIndex = durationIndex;
            UpdateInformation();
        }
        private void OnSelectionChanged(AdData adData, bool isSelected)
        {
            if(isSelected)
                _selectedAds.Add(adData);
            else
                _selectedAds.Remove(adData);
            _acceptButton.interactable = _selectedAds.Count > 0 && _marketService.CurrentPlayerProcessor != null;
            UpdateInformation();
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
        private void UpdateInformation()
        {
            var totalPrice = _selectedAds.Sum(ad => ad.Price) *
                             _marketingSettings.AdDurations[_selectedDurationIndex].PriceMultiplier;
            var formattedTotalPrice = $"{totalPrice:#,##0}".Replace(",", " ");
            _totalPriceTextObject.text = string.Format(_totalPriceFormat, Environment.NewLine, formattedTotalPrice);
        }
        #endregion
    }
}