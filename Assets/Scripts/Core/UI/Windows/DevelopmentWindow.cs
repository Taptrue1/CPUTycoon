using System.Collections.Generic;
using System.Linq;
using Core.CPU;
using Core.Games;
using Core.Services;
using Core.Technologies;
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

        private List<Technology> _selectedTechnologies;
        private List<TechnologyView> _technologyViews;

        private string _processorName;
        private int _processorPrice;

        private void Awake()
        {
            _nameInputField.onValueChanged.AddListener(OnNameInputFieldChanged);
            _priceInputField.onValueChanged.AddListener(OnPriceInputFieldChanged);
            _developButton.onClick.AddListener(OnDevelopButtonClicked);
            _developButton.onClick.AddListener(OnExitButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        
        [Inject]
        public void InjectDependencies(Game game, UIService uiService, UIFactory uiFactory)
        {
            _game = game;
            _uiService = uiService;
            _uiFactory = uiFactory;
            
            _selectedTechnologies = new();
            _technologyViews = new();
            
            SpawnTechnologiesView();
        }
        public override void Show()
        {
            base.Show();
            
            _selectedTechnologies.Clear();

            if (IsWindowUnlocked())
            {
                _lockedPanel.SetActive(false);
                ActivateTechnologiesView();
            }
            else
            {
                _lockedPanel.SetActive(true);
            }
        }
        public override void Hide()
        {
            base.Hide();
            
            _processorName = null;
            _processorPrice = 0;
            _nameInputField.text = "";
            _priceInputField.text = "0";
            _powerTextObject.text = "";
            _priceTextObject.text = "";
            
            _selectedTechnologies.Clear();
            foreach (var technologyView in _technologyViews)
            {
                technologyView.gameObject.SetActive(false);
                technologyView.SetSelected(false);
            }
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
        private void OnDevelopButtonClicked()
        {
            if (_processorName == null || _processorPrice == 0)
            {
                Debug.LogWarning("Name is null or price is 0");
                return;
            }
            
            var processor = new Processor(_processorName, _processorPrice, _selectedTechnologies);
            _game.Company.DevelopProcessor(processor);
            Debug.Log("Cpu created");
        }
        private void OnExitButtonClicked()
        {
            _uiService.ShowWindow<CoreWindow>();
        }
        private void OnSelected(Technology technology)
        {
            _selectedTechnologies.Add(technology);
            _technologyViews.Find(techView => techView.Technology == technology).SetSelected(true);
            UpdatePowerText();
            UpdatePriceText();
        }
        
        #endregion

        #region Other
        
        private void UpdatePowerText()
        {
            var totalPower = _selectedTechnologies.Sum(tech => tech.Power);
            _powerTextObject.text = string.Format(_powerTextFormat, totalPower);
        }
        private void UpdatePriceText()
        {
            var totalPrice = _selectedTechnologies.Sum(tech => tech.DevelopmentPointsPrice);
            _priceTextObject.text = string.Format(_priceTextFormat, totalPrice);
        }
        private void ActivateTechnologiesView()
        {
            var unlockedTechnologies = _game.Technologies.FindAll(technology => technology.Level.Value > 0);
            
            for (var i = 0; i < unlockedTechnologies.Count; i++)
            {
                _technologyViews[i].gameObject.SetActive(true);
                _technologyViews[i].SetTechnology(unlockedTechnologies[i]);
                _technologyViews[i].Selected += OnSelected;
            }
        }
        private void SpawnTechnologiesView()
        {
            foreach(var technology in _game.Technologies)
            {
                var technologyView = _uiFactory.CreateTechnologyView(_technologiesContainer);
                _technologyViews.Add(technologyView);
            }
        }
        private bool IsWindowUnlocked()
        {
            var firstTechnology = _game.Technologies[0];
            var secondTechnology = _game.Technologies[1];
            var thirdTechnology = _game.Technologies[2];
            
            return firstTechnology.Level.Value > 0 && secondTechnology.Level.Value > 0 && thirdTechnology.Level.Value > 0;
        }
        
        #endregion
    }
}