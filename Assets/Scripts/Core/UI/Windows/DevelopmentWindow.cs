using System.Collections.Generic;
using Core.Games;
using Core.Services;
using Core.UI.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.UI.Windows
{
    public class DevelopmentWindow : WindowPresenter
    {
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_InputField _priceInputField;

        [Header("Text Objects")]
        [SerializeField] private string _powerTextFormat = "Power: {0} Gflops";
        [SerializeField] private string _priceTextFormat = "Price: {0}$";
        [SerializeField] private TextMeshProUGUI _powerTextObject;
        [SerializeField] private TextMeshProUGUI _priceTextObject;
        
        [Header("Other")]
        [SerializeField] private Transform _technologiesContainer;
        [SerializeField] private Button _developButton;
        
        [Header("Locked View")]
        [SerializeField] private GameObject _lockedPanel;
        [SerializeField] private Button _exitButton;

        private Game _game;
        private UIService _uiService;
        private UIFactory _uiFactory;
        
        private List<TechnologyView> _technologyViews;

        private string _processorName;
        private int _processorPrice;

        private void Awake()
        {
            _nameInputField.onValueChanged.AddListener(OnNameInputFieldChanged);
            _priceInputField.onValueChanged.AddListener(OnPriceInputFieldChanged);
            _developButton.onClick.AddListener(OnExitButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        
        [Inject]
        public void InjectDependencies(Game game, UIService uiService, UIFactory uiFactory)
        {
            _game = game;
            _uiService = uiService;
            _uiFactory = uiFactory;
            _technologyViews = new();
        }

        #region Callbacks

        private void OnNameInputFieldChanged(string processorName)
        {
            _processorName = processorName;
        }
        private void OnPriceInputFieldChanged(string price)
        {
            if(price.Length < 1) return;
            _processorPrice = int.Parse(price);
        }
        private void OnExitButtonClicked()
        {
            _uiService.ShowWindow<CoreWindow>();
        }

        #endregion
    }
}