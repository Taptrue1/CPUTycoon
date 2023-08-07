using System;
using Core.CPU;
using Core.Datas;
using Core.GameSpeedStateMachines.States;
using Core.Services;
using Core.Technologies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Windows
{
    public class CoreWindow : WindowPresenter
    {
        [Header("Game Speed Buttons")]
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _normalSpeedButton;
        [SerializeField] private Button _fastSpeedButton;
        [SerializeField] private Button _fastestSpeedButton;
        
        [Header("Windows Open Buttons")]
        [SerializeField] private Button _openResearchWindowButton;
        [SerializeField] private Button _openDevelopmentWindowButton;
        [SerializeField] private Button _openOfficeWindowButton;
        [SerializeField] private Button _openMarketingWindowButton;

        //TODO change text to custom number view or something like that
        [Header("Texts")]
        [SerializeField] private string _dateTextFormat = "dd.MM.yyyy";
        [SerializeField] private TextMeshProUGUI _dateTextObject;
        [SerializeField] private TextMeshProUGUI _companyNameTextObject;
        [SerializeField] private TextMeshProUGUI _moneyTextObject;

        [Header("Other")]
        [SerializeField] private Transform _newsContainer;
        [SerializeField] private Transform _actionsContainer;
        [SerializeField] private CurrencyData _moneyCurrencyData;
        
        private TimeService _timeService;
        private UIService _uiService;
        private CurrencyService _currencyService;

        private void Awake()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
            _normalSpeedButton.onClick.AddListener(OnNormalSpeedButtonClicked);
            _fastSpeedButton.onClick.AddListener(OnFastSpeedButtonClicked);
            _fastestSpeedButton.onClick.AddListener(OnFastestSpeedButtonClicked);

            _openResearchWindowButton.onClick.AddListener(OnOpenResearchWindowButtonClicked);
            _openDevelopmentWindowButton.onClick.AddListener(OnOpenDevelopmentWindowButtonClicked);
            _openOfficeWindowButton.onClick.AddListener(OnOpenOfficeWindowButtonClicked);
            _openMarketingWindowButton.onClick.AddListener(OnOpenMarketingWindowButtonClicked);
        }

        [Inject]
        public void InjectDependencies(TimeService timeService, UIService uiService, CurrencyService currencyService)
        {
            _timeService = timeService;
            _uiService = uiService;
            _currencyService = currencyService;
            
            _timeService.DateTimeChanged += OnDateChanged;
            
            var money = _currencyService.GetCurrency(_moneyCurrencyData.Name);
            money.Changed += OnMoneyChanged;
            
            OnMoneyChanged(money.Value);
            OnDateChanged(_timeService.CurrentDate);
        }
        public override void Show()
        {
            base.Show();
            _timeService.SetLastGameSpeedState();
        }
        public override void Hide()
        {
            base.Hide();
            _timeService.SetGameSpeedState<PauseGameSpeedState>();
        }
        
        #region GameSpeedCallbacks
        
        private void OnPauseButtonClicked()
        {
            _timeService.SetGameSpeedState<PauseGameSpeedState>();
        }
        private void OnNormalSpeedButtonClicked()
        {
            _timeService.SetGameSpeedState<NormalGameSpeedState>();
        }
        private void OnFastSpeedButtonClicked()
        {
            _timeService.SetGameSpeedState<FastGameSpeedState>();
        }
        private void OnFastestSpeedButtonClicked()
        {
            _timeService.SetGameSpeedState<FastestGameSpeedState>();
        }
        
        #endregion
        
        #region WindowsCallbacks
        
        private void OnOpenResearchWindowButtonClicked()
        {
            _uiService.ShowWindow<ResearchWindow>();
        }
        private void OnOpenDevelopmentWindowButtonClicked()
        {
            _uiService.ShowWindow<DevelopmentWindow>();
        }
        private void OnOpenOfficeWindowButtonClicked()
        {
            _uiService.ShowWindow<OfficeWindow>();
        }
        private void OnOpenMarketingWindowButtonClicked()
        {
            _uiService.ShowWindow<MarketingWindow>();
        }

        #endregion
        
        #region OtherCallbacks
        
        private void OnDateChanged(DateTime date)
        {
            _dateTextObject.text = date.ToString(_dateTextFormat);
        }
        private void OnMoneyChanged(double money)
        {
            _moneyTextObject.text = $"${money:#,##0}".Replace(",", " ");;
        }
        private void OnNewsAppeared()
        {
            
        }
        private void OnResearchingTargetChanged(Technology technology)
        {
            if (technology == null)
            {
                //TODO delete researching action view
                return;
            }
            //TODO create researching action view
        }
        private void OnDevelopingTargetChanged(Processor processor)
        {
            if (processor == null)
            {
                //TODO delete developing action view
                return;
            }
            //TODO create developing action view
        }
        private void OnProcessorSelling()
        {
            
        }
        
        #endregion
    }
}