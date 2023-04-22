using System.Collections.Generic;
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
    public class ResearchWindow : WindowPresenter
    {
        [Header("Technologies")]
        [SerializeField] private Transform _technologiesContainer;

        [Header("Texts")]
        [SerializeField] private string _researchPointsTextFormat = "{0}/day";
        [SerializeField] private string _researchPointsPriceTextFormat = "{0}/day";
        [SerializeField] private TextMeshProUGUI _researchPointsTextObject;
        [SerializeField] private TextMeshProUGUI _researchPointsPriceTextObject;
        
        [Header("Other")]
        [SerializeField] private Slider _researchPointsSlider;
        [SerializeField] private Button _exitButton;

        private Game _game;
        private UIService _uiService;
        private UIFactory _uiFactory;
        
        private List<TechnologyView> _technologyViews;
        
        private void Awake()
        {
            //TODO Change this to get from Company max and min values
            _researchPointsSlider.minValue = 1;
            _researchPointsSlider.maxValue = 100;
            
            _researchPointsSlider.onValueChanged.AddListener(OnSliderValueChanged);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }
       
        [Inject]
        public void InjectDependencies(Game game, UIService uiService, UIFactory uiFactory)
        {
            _game = game;
            _uiService = uiService;
            _uiFactory = uiFactory;

            _researchPointsSlider.value = _game.Company.ResearchPoints.Value;
            _technologyViews = SpawnTechnologyViews(_game.Technologies);
        }
        
        public override void Show()
        {
            base.Show();
            
            UpdateText(_researchPointsTextObject, string.Format(_researchPointsTextFormat, _game.Company.ResearchPoints.Value));
            UpdateText(_researchPointsPriceTextObject, string.Format(_researchPointsPriceTextFormat, _game.Company.GetResearchPointsPrice()));
            //TODO add update selection for selected technology
        }

        #region Callbacks
        
        private void OnSelected(Technology technology)
        {
            _game.Company.ResearchTechnology(technology);
            SelectTechnology(technology);
        }
        private void OnSliderValueChanged(float value)
        {
            var resultValue = Mathf.CeilToInt(value);
            
            _game.Company.SetResearchPoints(resultValue);
            _researchPointsSlider.value = resultValue;
            
            UpdateText(_researchPointsTextObject, string.Format(_researchPointsTextFormat, resultValue));
            UpdateText(_researchPointsPriceTextObject, string.Format(_researchPointsPriceTextFormat, _game.Company.GetResearchPointsPrice()));
        }
        private void OnExitButtonClicked()
        {
            _uiService.ShowWindow<CoreWindow>();
        }
        
        #endregion

        private void UpdateText(TextMeshProUGUI textObject, string value)
        {
            textObject.text = value;
        }

        private void SelectTechnology(Technology technology)
        {
            foreach (var techView in _technologyViews)
            {
                techView.SetSelected(false);
            }
            _technologyViews.Find(techView => techView.Technology == technology).SetSelected(true);
        }
        private List<TechnologyView> SpawnTechnologyViews(List<Technology> technologies)
        {
            var technologyViews = new List<TechnologyView>();
            foreach (var technology in technologies)
            {
                var technologyView = _uiFactory.CreateTechnologyView(_technologiesContainer);
                technologyView.SetTechnology(technology);
                technologyView.Selected += OnSelected;
                technologyViews.Add(technologyView);
            }
            return technologyViews;
        }
    }
}