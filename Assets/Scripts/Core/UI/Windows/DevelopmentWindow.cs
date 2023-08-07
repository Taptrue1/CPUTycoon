using System;
using System.Collections.Generic;
using System.Linq;
using Core.CPU;
using Core.Datas;
using Core.Games;
using Core.Services;
using Core.Technologies;
using Settings;
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
        
        [Header("Dropdowns")]
        [SerializeField] private TMP_Dropdown _techProcessDropdown;
        [SerializeField] private TMP_Dropdown _frequencyDropdown;
        [SerializeField] private TMP_Dropdown _formFactorDropdown;
        [SerializeField] private TMP_Dropdown _ramDropdown;
        [SerializeField] private TMP_Dropdown _bitsDropdown;

        [Header("Text Objects")]
        [SerializeField] private TextMeshProUGUI _processorInfoTextObject;
        [SerializeField] private TextMeshProUGUI _lockReasonTextObject;

        [Header("Buttons")]
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _developButton;
        [SerializeField] private Button _lockViewExitButton;

        [Header("Other")]
        [SerializeField] private CurrencyData _moneyCurrencyData;
        //[SerializeField] private Transform _technologiesContainer;
        [SerializeField] private GameObject _lockedPanel;
        
        private Game _game;
        private UIService _uiService;
        private TeamService _teamService;
        private CurrencyService _currencyService;
        private Technology _techTreeRootNode; 
        //private TechnologiesSettings _technologiesSettings;

        private const string ResearchLockReason = "You need to research first {0} technologies";
        private const string DevelopmentLockReason = "Processor is in development";

        private void Awake()
        {
            _techProcessDropdown.onValueChanged.AddListener(OnDropdownChanged);
            _frequencyDropdown.onValueChanged.AddListener(OnDropdownChanged);
            _formFactorDropdown.onValueChanged.AddListener(OnDropdownChanged);
            _ramDropdown.onValueChanged.AddListener(OnDropdownChanged);
            _bitsDropdown.onValueChanged.AddListener(OnDropdownChanged);
            _developButton.onClick.AddListener(OnDevelopButtonClicked);
            _developButton.onClick.AddListener(OnExitButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _lockViewExitButton.onClick.AddListener(OnExitButtonClicked);
        }

        [Inject]
        public void InjectDependencies(Game game, UIService uiService, TeamService teamService,
            CurrencyService currencyService, CoreSettings coreSettings, Technology techTreeRootNode)
        {
            _game = game;
            _uiService = uiService;
            _teamService = teamService;
            _currencyService = currencyService;
            _techTreeRootNode = techTreeRootNode;
            //_technologiesSettings = coreSettings.TechnologiesSettings;
        }
        public override void Show()
        {
            base.Show();
            
            var isAllChildsResearched = _techTreeRootNode.Children.All(child => child.IsResearched());
            var isProcessorInDevelopment = _game.DevelopingTarget != null;
            var canActivateLockView = !isAllChildsResearched || isProcessorInDevelopment;
            var reason = isAllChildsResearched ? DevelopmentLockReason : ResearchLockReason;
            SetLockView(canActivateLockView, string.Format(reason, _techTreeRootNode.Children.Count));
            
            if(canActivateLockView) return;
            
            _nameInputField.text = string.Empty;
            _priceInputField.text = string.Empty;
            
            SetupDropdowns();
            UpdateInfo();
        }
        
        #region Callbacks

        private void OnDropdownChanged(int value)
        {
            UpdateInfo();
        }
        private void OnDevelopButtonClicked()
        {
            var name = _nameInputField.text;
            var sellPrice = int.Parse(_priceInputField.text);
            var selectedTechnologies = GetSelectedTechnologies();
            var developingPrice = selectedTechnologies.Sum(technology => technology.DevelopPrice);
            _currencyService.GetCurrency(_moneyCurrencyData.Name).Value -= developingPrice;
            _game.SetDevelopingTarget(new Processor(name, sellPrice, selectedTechnologies));
        }
        private void OnExitButtonClicked()
        {
            _uiService.ShowWindow<CoreWindow>();
        }

        #endregion
        
        #region Other

        private void SetupDropdowns()
        {
            _techProcessDropdown.ClearOptions();
            _frequencyDropdown.ClearOptions();
            _formFactorDropdown.ClearOptions();
            _ramDropdown.ClearOptions();
            _bitsDropdown.ClearOptions();
            
            foreach (var technology in GetResearchedTechnologies())
                AddSetting(technology);

            _techProcessDropdown.gameObject.SetActive(_techProcessDropdown.options.Count != 0);
            _frequencyDropdown.gameObject.SetActive(_frequencyDropdown.options.Count != 0);
            _formFactorDropdown.gameObject.SetActive(_formFactorDropdown.options.Count != 0);
            _ramDropdown.gameObject.SetActive(_ramDropdown.options.Count != 0);
            _bitsDropdown.gameObject.SetActive(_bitsDropdown.options.Count != 0);
        }
        private void AddSetting(Technology technology)
        {
            switch (technology.Type)
            {
                case TechnologyType.TechProcess:
                    _techProcessDropdown.AddOptions(new List<string> {technology.Name});
                    break;
                case TechnologyType.Frequency:
                    _frequencyDropdown.AddOptions(new List<string> {technology.Name});
                    break;
                case TechnologyType.FormFactor:
                    _formFactorDropdown.AddOptions(new List<string> {technology.Name});
                    break;
                case TechnologyType.Cache:
                    //TODO add cache
                    break;
                case TechnologyType.Ram:
                    _ramDropdown.AddOptions(new List<string> {technology.Name});
                    break;
                case TechnologyType.Bitness:
                    _bitsDropdown.AddOptions(new List<string> {technology.Name});
                    break;
                case TechnologyType.Architecture:
                    //TODO add architecture
                    break;
                case TechnologyType.InstructionSet:
                    //TODO add instruction set
                    break;
                case TechnologyType.Separate:
                    //TODO add separate
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void SetLockView(bool active, string reason)
        {
            _lockedPanel.SetActive(active);
            _lockReasonTextObject.text = reason;
        }
        private void UpdateInfo()
        {
            var totalDpGeneration = _teamService.HiredProgrammers.Sum(programmer => programmer.PointsGeneration);
            var selectedTechnologies = GetSelectedTechnologies();
            var totalProducePrice = selectedTechnologies.Sum(technology => technology.ProducePrice);
            var totalDevelopmentPrice = selectedTechnologies.Sum(technology => technology.DevelopPrice);
            var totalDevelopmentPoints = selectedTechnologies.Sum(technology => technology.DevelopPoints);
            var developmentDuration = Mathf.CeilToInt(totalDevelopmentPoints / totalDpGeneration);
            var canBuyDeveloping = _currencyService.GetCurrency(_moneyCurrencyData.Name).Value > totalDevelopmentPrice;

            _developButton.interactable = canBuyDeveloping && _nameInputField.text != string.Empty &&
                                          _priceInputField.text != string.Empty;
            _processorInfoTextObject.text = $"Produce price ${totalProducePrice}\n" +
                                            $"Development price ${totalDevelopmentPrice}\n" +
                                            $"Development takes {developmentDuration} days";
        }
        private List<Technology> GetResearchedTechnologies()
        {
            var researchedTechnologies = new List<Technology>();
            var currentTechnologies = new List<Technology> {_techTreeRootNode};
            while(currentTechnologies.Count > 0)
            {
                var newTechnologies = new List<Technology>();
                foreach (var technology in currentTechnologies)
                {
                    newTechnologies.AddRange(technology.Children);
                    if(technology.Name =="ROOT") continue;
                    if(technology.IsResearched() && !researchedTechnologies.Contains(technology)) 
                        researchedTechnologies.Add(technology);
                }
                currentTechnologies = newTechnologies;
            }
            return researchedTechnologies;
        }
        private List<Technology> GetSelectedTechnologies()
        {
            var selectedTechnologies = new List<Technology>()
            {
                GetTechnology(TechnologyType.TechProcess, _techProcessDropdown.value),
                GetTechnology(TechnologyType.Frequency, _frequencyDropdown.value),
                GetTechnology(TechnologyType.FormFactor, _formFactorDropdown.value),
                GetTechnology(TechnologyType.Ram, _ramDropdown.value),
                GetTechnology(TechnologyType.Bitness, _bitsDropdown.value)
                //TODO add other tech types
            };
            return selectedTechnologies.Where(tech => tech != null).ToList();
        }
        private Technology GetTechnology(TechnologyType type, int index)
        {
            var currentTechnologies = new List<Technology> {_techTreeRootNode};
            while(currentTechnologies.Count > 0)
            {
                var newTechnologies = new List<Technology>();
                foreach (var technology in currentTechnologies)
                {
                    if(technology.Type == type && technology.Index == index) return technology;
                    newTechnologies.AddRange(technology.Children);
                }
                currentTechnologies = newTechnologies;
            }
            return null;
        }
        
        #endregion
    }
}