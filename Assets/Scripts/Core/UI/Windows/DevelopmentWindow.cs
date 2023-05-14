using System;
using System.Collections.Generic;
using System.Linq;
using Core.CPU;
using Core.Games;
using Core.Services;
using Core.Technologies;
using Core.UI.Views;
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
        [SerializeField] private TextMeshProUGUI _lockReasonTextObject;

        [Header("Buttons")]
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _developButton;
        [SerializeField] private Button _lockViewExitButton;
        
        [Header("Other")]
        [SerializeField] private Transform _technologiesContainer;
        [SerializeField] private GameObject _lockedPanel;
        
        private Game _game;
        private UIService _uiService;
        private UIFactory _uiFactory;
        private Technology _techTreeRootNode;
        private TechnologiesSettings _technologiesSettings;
        private List<TechnologyView> _technologyViews;
        private List<TechProcessPair> _techProcessPairs;
        private List<FrequencyPair> _frequencyPairs;
        private List<FormFactorPair> _formFactorPairs;
        private List<RamPair> _ramPairs;
        private List<int> _bits;

        private const string ResearchLockReason = "You need to research first {0} technologies";
        private const string DevelopmentLockReason = "Processor is in development";

        private void Awake()
        {
            _developButton.onClick.AddListener(OnDevelopButtonClicked);
            _developButton.onClick.AddListener(OnExitButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _lockViewExitButton.onClick.AddListener(OnExitButtonClicked);
        }

        [Inject]
        public void InjectDependencies(Game game, UIService uiService, UIFactory uiFactory, CoreSettings coreSettings,
            Technology techTreeRootNode)
        {
            _game = game;
            _uiService = uiService;
            _uiFactory = uiFactory;
            _technologiesSettings = coreSettings.TechnologiesSettings;
            _techTreeRootNode = techTreeRootNode;
            
            _technologyViews = new();
            _techProcessPairs = new();
            _frequencyPairs = new();
            _formFactorPairs = new();
            _ramPairs = new();
            _bits = new();
        }
        public override void Show()
        {
            base.Show();
            
            var isAllChildsResearched = _techTreeRootNode.Children.All(child => child.IsResearched());
            var isProcessorInDevelopment = _game.ProcessorToDevelop != null;
            var canActivateLockView = !isAllChildsResearched || isProcessorInDevelopment;
            var reason = isAllChildsResearched ? DevelopmentLockReason : ResearchLockReason;
            SetLockView(canActivateLockView, string.Format(reason, _techTreeRootNode.Children.Count));
            
            if(canActivateLockView) return;
            
            _nameInputField.text = string.Empty;
            _priceInputField.text = string.Empty;
            
            UpdateAvailableSettings();
            SetupDropdowns();
        }
        
        #region Callbacks
        
        private void OnDevelopButtonClicked()
        {
            var techProcess = _techProcessPairs[_techProcessDropdown.value];
            var frequency = _frequencyPairs[_frequencyDropdown.value];
            var formFactor = _formFactorPairs[_formFactorDropdown.value];
            var ram = _ramPairs.Count == 0 ? null : _ramPairs[_ramDropdown.value];
            var bits = _bits.Count == 0 ? 0 : _bits[_bitsDropdown.value];
            
            var name = _nameInputField.text;
            var price = int.Parse(_priceInputField.text);
            var processor = new Processor(name, price, techProcess, frequency,
                formFactor, ram, bits);
            
            _game.SetProcessorToDevelop(processor);
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
            
            _techProcessDropdown.AddOptions(_techProcessPairs.ConvertAll(pair => pair.TechProcess + " " + pair.MesureUnit));
            _frequencyDropdown.AddOptions(_frequencyPairs.ConvertAll(pair => pair.Frequency + " " + pair.MesureUnit));
            _formFactorDropdown.AddOptions(_formFactorPairs.ConvertAll(pair => pair.FormFactorName));
            _ramDropdown.AddOptions(_ramPairs.ConvertAll(pair => pair.Ram + " " + pair.MesureUnit));
            _bitsDropdown.AddOptions(_bits.ConvertAll(bit => bit.ToString()));

            _techProcessDropdown.gameObject.SetActive(_techProcessPairs.Count != 0);
            _frequencyDropdown.gameObject.SetActive(_frequencyPairs.Count != 0);
            _formFactorDropdown.gameObject.SetActive(_formFactorPairs.Count != 0);
            _ramDropdown.gameObject.SetActive(_ramPairs.Count != 0);
            _bitsDropdown.gameObject.SetActive(_bits.Count != 0);
        }
        private void UpdateAvailableSettings()
        {
            _techProcessPairs.Clear();
            _frequencyPairs.Clear();
            _formFactorPairs.Clear();
            _ramPairs.Clear();
            _bits.Clear();
            
            var researchedTechnologies = GetResearchedTechnologies();
            foreach (var technology in researchedTechnologies)
                AddTechnologySetting(technology);
        }
        private void AddTechnologySetting(Technology technology)
        {
            switch (technology.Type)
            {
                case TechnologyType.TechProcess:
                    var techProcess = _technologiesSettings.TechProcesses[technology.TypeValue];
                    _techProcessPairs.Add(techProcess);
                    break;
                case TechnologyType.Frequency:
                    var frequency = _technologiesSettings.Frequencies[technology.TypeValue];
                    _frequencyPairs.Add(frequency);
                    break;
                case TechnologyType.FormFactor:
                    var formFactor = _technologiesSettings.FormFactors[technology.TypeValue];
                    _formFactorPairs.Add(formFactor);
                    break;
                case TechnologyType.Cache:
                    //TODO: Add cache
                    break;
                case TechnologyType.Ram:
                    var ram = _technologiesSettings.Ram[technology.TypeValue];
                    _ramPairs.Add(ram);
                    break;
                case TechnologyType.Bitness:
                    var bitness = _technologiesSettings.Bits[technology.TypeValue];
                    _bits.Add(bitness);
                    break;
                case TechnologyType.Architecture:
                    //TODO: Add architecture
                    break;
                case TechnologyType.InstructionSet:
                    //TODO: Add instruction set
                    break;
                case TechnologyType.Separate:
                    //TODO: Add separate
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
        
        private void SetLockView(bool active, string reason)
        {
            _lockedPanel.SetActive(active);
            _lockReasonTextObject.text = reason;
        }

        #endregion
    }
}