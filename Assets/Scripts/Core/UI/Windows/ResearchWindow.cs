using System;
using System.Collections.Generic;
using System.Linq;
using Core.Datas;
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
        [SerializeField] private int _yOffset;
        [SerializeField] private int _xOffset;
        [SerializeField] private RectTransform _technologiesContainer;
        [SerializeField] private CurrencyData _researchPointsCurrencyData;

        [Header("Texts")]
        [SerializeField] private string _totalRPTextFormat = "{0}";
        [SerializeField] private string _rpPerDayTextFormat = "{0}/day";
        [SerializeField] private string _rpPerDayPriceTextFormat = "{0}/day";
        [SerializeField] private TextMeshProUGUI _totalRPTextObject;
        [SerializeField] private TextMeshProUGUI _rpPerDayTextObject;
        [SerializeField] private TextMeshProUGUI _rpPerDayPriceTextObject;

        [Header("Other")]
        [SerializeField] private Slider _researchPointsSlider;
        [SerializeField] private Button _exitButton;

        private Game _game;
        private UIService _uiService;
        private UIFactory _uiFactory;
        private CurrencyService _currencyService;
        private Technology _techTreeRootNode;
        
        private Technology _selectedTechnology;
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
        public void InjectDependencies(Game game, UIService uiService, UIFactory uiFactory,
            CurrencyService currencyService, Technology techTreeRootNode)
        {
            _game = game;
            _uiService = uiService;
            _uiFactory = uiFactory;
            _currencyService = currencyService;
            _techTreeRootNode = techTreeRootNode;
            _technologyViews = new List<TechnologyView>();

            var rpCurrency = _currencyService.GetCurrency(_researchPointsCurrencyData.Name);
            rpCurrency.Changed += OnResearchPointsChanged;
            
            OnSliderValueChanged(1);
            OnResearchPointsChanged(rpCurrency.Value);
            SetupTechTreeView();
        }
        
        #region TechTreeSetup

        private void SetupTechTreeView()
        {
            var views = CreateViews();
            SetupTechnologiesContainer(views);
            SetupViews(views);
        }
        private void SetupTechnologiesContainer(List<List<TechnologyView>> views)
        {
            var containerWidth = views.Count * _xOffset;
            var containerHeight = views.OrderByDescending(list => list.Count).First().Count * _yOffset;

            _technologiesContainer.sizeDelta = new Vector2(containerWidth, containerHeight);
            _technologiesContainer.anchoredPosition = new Vector2(containerWidth / 2, containerHeight / 2);
        }
        private void SetupViews(List<List<TechnologyView>> views)
        {
            var nodeX = 0 - views.Count / 2;
            foreach (var viewList in views)
            {
                var nodeY = 0 - viewList.Count / 2;
                var yOffset = viewList.Count % 2 == 0 ? _yOffset / 2 : 0;
                foreach (var view in viewList)
                {
                    view.GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(nodeX * _xOffset, nodeY * _yOffset + yOffset);
                    nodeY++;
                }
                nodeX++;
            }
        }
        private List<List<TechnologyView>> CreateViews()
        {
            var currentNodes = _techTreeRootNode.Children.ToList();
            var views = new List<List<TechnologyView>>();
            while (currentNodes.Count > 0)
            {
                var newNodes = new List<Technology>();
                var newViews = new List<TechnologyView>();
                foreach (var node in currentNodes)
                {
                    var view = _uiFactory.CreateTechnologyView(_technologiesContainer);

                    view.Init(node);
                    view.Selected += OnTechnologySelected;
                    newViews.Add(view);
                    newNodes.AddRange(node.Children.Where(child =>
                        child != null && !newNodes.Any(node => node.Name == child.Name)));
                    _technologyViews.Add(view);
                }
                views.Add(newViews);
                currentNodes = newNodes;
            }

            return views;
        }
        
        #endregion
        
        #region Callbacks
        
        private void OnSliderValueChanged(float value)
        {
            var resultValue = Mathf.RoundToInt(value);
            
            _game.RPPerDay = resultValue;
            
            UpdateText(_rpPerDayTextObject, string.Format(_rpPerDayTextFormat, _game.RPPerDay));
            UpdateText(_rpPerDayPriceTextObject, string.Format(_rpPerDayPriceTextFormat, _game.RPPrice));
        }
        private void OnTechnologySelected(Technology technology)
        {
            var referenceNodes = FindReferencingNodes(technology);
            var canResearch = referenceNodes.All(obj => obj.IsResearched());
            
            _selectedTechnology = canResearch ? technology : FindTechnologyToResearch(technology);

            UpdateViewsSelection();
            OnResearchPointsChanged(_currencyService.GetCurrency(_researchPointsCurrencyData.Name).Value);
        }
        private void OnResearchPointsChanged(int value)
        {
            var canResearch = _selectedTechnology != null && value >= _selectedTechnology.ResearchPrice;
            if (canResearch)
                ResearchTechnology(_selectedTechnology);

            UpdateText(_totalRPTextObject, string.Format(_totalRPTextFormat, value));
        }
        private void OnExitButtonClicked()
        {
            _uiService.ShowWindow<CoreWindow>();
        }
        
        #endregion

        #region Other

        private void UpdateViewsSelection()
        {
            foreach(var view in _technologyViews)
                view.SetSelected(false);
            _technologyViews.Find(view => view.Technology == _selectedTechnology).SetSelected(true);
        }
        private void ResearchTechnology(Technology technology)
        {
            var view = _technologyViews.Find(view => view.Technology == technology);
            technology.Research();
            view.SetResearched();
            view.SetSelected(false);
            _currencyService.GetCurrency(_researchPointsCurrencyData.Name).Value -= technology.ResearchPrice;
        }
        private void UpdateText(TextMeshProUGUI textObject, string value)
        {
            textObject.text = value;
        }
        private Technology FindTechnologyToResearch(Technology selectedNode)
        {
            if(selectedNode.Name == "ROOT")
                throw new Exception("Try to research ROOT node");
            
            var referencingNodes = FindReferencingNodes(selectedNode);
            if (referencingNodes.All(obj => obj.IsResearched()))
                return selectedNode;
            
            var firstNotResearchedNode = referencingNodes.First(obj => !obj.IsResearched());
            return FindTechnologyToResearch(firstNotResearchedNode);
        }
        private List<Technology> FindReferencingNodes(Technology selectedNode)
        {
            var referencingObjects = new List<Technology>();
            var currentNodes = new List<Technology> {_techTreeRootNode};
            while (currentNodes.Count > 0)
            {
                var newNodes = new List<Technology>();
                foreach (var node in currentNodes)
                {
                    if(node.Children.Any(child => child.Name == selectedNode.Name))
                        referencingObjects.Add(node);
                    newNodes.AddRange(node.Children.Where(child =>
                        child != null && !newNodes.Any(node => node.Name == child.Name)));
                }
                currentNodes = newNodes;
            }
            return referencingObjects;
        }

        #endregion
    }
}