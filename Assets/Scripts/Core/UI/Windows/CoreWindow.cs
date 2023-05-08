using System;
using Core.Games;
using Core.GameSpeedStateMachines.States;
using Core.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Windows
{
    public class CoreWindow : WindowPresenter
    {
        [Header("Game Speed")]
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _normalSpeedButton;
        [SerializeField] private Button _fastSpeedButton;
        [SerializeField] private Button _fastestSpeedButton;
        
        [Header("Windows Open Buttons")]
        [SerializeField] private Button _openResearchWindowButton;
        [SerializeField] private Button _openDevelopmentWindowButton;

        //TODO change text to custom number view or something like that
        [Header("Texts")]
        [SerializeField] private string _dateTextFormat = "dd.MM.yyyy";
        [SerializeField] private string _companyNameTextFormat = "{0}";
        [SerializeField] private string _moneyTextFormat = "{0}$";
        [SerializeField] private TextMeshProUGUI _dateTextObject;
        [SerializeField] private TextMeshProUGUI _companyNameTextObject;
        [SerializeField] private TextMeshProUGUI _moneyTextObject;
        
        private TimeService _timeService;
        private UIService _uiService;

        private void Awake()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
            _normalSpeedButton.onClick.AddListener(OnNormalSpeedButtonClicked);
            _fastSpeedButton.onClick.AddListener(OnFastSpeedButtonClicked);
            _fastestSpeedButton.onClick.AddListener(OnFastestSpeedButtonClicked);

            _openResearchWindowButton.onClick.AddListener(OnOpenResearchWindowButtonClicked);
            _openDevelopmentWindowButton.onClick.AddListener(OnOpenDevelopmentWindowButtonClicked);
        }

        [Inject]
        public void InjectDependencies(TimeService timeService, UIService uiService)
        {
            _timeService = timeService;
            _uiService = uiService;
            
            _timeService.DateTimeChanged += OnDateChanged;
            
            OnDateChanged(_timeService.CurrentDateTime);
        }
        public override void Show()
        {
            base.Show();
            _timeService.ChangeGameSpeedState<NormalGameSpeedState>();
        }
        public override void Hide()
        {
            base.Hide();
            _timeService.ChangeGameSpeedState<PauseGameSpeedState>();
        }
        
        #region GameSpeedCallbacks
        
        private void OnPauseButtonClicked()
        {
            _timeService.ChangeGameSpeedState<PauseGameSpeedState>();
        }
        private void OnNormalSpeedButtonClicked()
        {
            _timeService.ChangeGameSpeedState<NormalGameSpeedState>();
        }
        private void OnFastSpeedButtonClicked()
        {
            _timeService.ChangeGameSpeedState<FastGameSpeedState>();
        }
        private void OnFastestSpeedButtonClicked()
        {
            _timeService.ChangeGameSpeedState<FastestGameSpeedState>();
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

        #endregion
        
        #region OtherCallbacks
        
        private void OnDateChanged(DateTime date)
        {
            _dateTextObject.text = date.ToString(_dateTextFormat);
        }
        private void OnMoneyChanged(double money)
        {
            _moneyTextObject.text = string.Format(_moneyTextFormat, money);
        }
        
        #endregion
    }
}