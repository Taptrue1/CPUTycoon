using System;
using System.Collections.Generic;
using System.Linq;
using Core.Games;
using Core.Services;
using Core.Technologies;
using Core.UI.Views;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Core.UI.Windows
{
    public class ResearchWindow : WindowPresenter
    {
        [Header("Technologies")]
        [SerializeField] private int _yOffset;
        [SerializeField] private int _xOffset;
        [SerializeField] private RectTransform _technologiesContainer;

        [Header("Other")]
        [SerializeField] private UiLineRenderer _lineRenderer;
        [SerializeField] private Button _exitButton;

        private Game _game;
        private UIService _uiService;
        private UIFactory _uiFactory;
        private Technology _techTreeRootNode;
        private TechnologiesSettings _technologiesSettings;
        
        private Technology _selectedTechnology;
        private List<TechnologyView> _technologyViews;

        private void Awake()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        [Inject]
        public void InjectDependencies(Game game, UIService uiService, UIFactory uiFactory, CoreSettings coreSettings,
            Technology techTreeRootNode)
        {
            _game = game;
            _uiService = uiService;
            _uiFactory = uiFactory;
            _techTreeRootNode = techTreeRootNode;
            _technologyViews = new List<TechnologyView>();
            _technologiesSettings = coreSettings.TechnologiesSettings;
            
            SetupTechTreeView();
            
            _lineRenderer.gameObject.transform.localPosition -= new Vector3(_technologiesContainer.sizeDelta.x, 0, 0);
        }
        public override void Show()
        {
            base.Show();
            UpdateViews();
        }
        
        #region TechTreeSetup

        private void SetupTechTreeView()
        {
            var views = CreateViews();
            SetupTechnologiesContainer(views);
            SetupViews(views);
            SetupLines();
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
        private void SetupLines()
        {
            var lines = (from view in _technologyViews
                let viewTechnology = view.Technology
                from tech in viewTechnology.Children
                let techView = _technologyViews.First(v => v.Technology.Name == tech.Name)
                select new Line(view.OutputPoint.position, techView.InputPoint.position)).ToList();
            _lineRenderer.SetLines(lines);
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
                    view.Init(node, GetTechnologyIcon(node));
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
        private Sprite GetTechnologyIcon(Technology technology)
        {
            //change enum to type
            return technology.Type switch
            {
                TechnologyType.TechProcess => _technologiesSettings.TechProcessIcon,
                TechnologyType.Frequency => _technologiesSettings.FrequencyIcon,
                TechnologyType.FormFactor => _technologiesSettings.FormFactorIcon,
                TechnologyType.Cache => null,
                TechnologyType.Ram => _technologiesSettings.RamIcon,
                TechnologyType.Bitness => _technologiesSettings.BitsIcon,
                TechnologyType.Architecture => null,
                TechnologyType.InstructionSet => null,
                TechnologyType.Separate => null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion
        
        #region Callbacks
        
        private void OnTechnologySelected(Technology technology)
        {
            var referenceNodes = FindReferencingNodes(technology);
            var canResearch = referenceNodes.All(obj => obj.IsResearched());
            var technologyToResearch = canResearch ? technology : FindTechnologyToResearch(technology);
            
            _game.SetResearchingTarget(technologyToResearch);
            
            UpdateViews();
        }
        private void OnExitButtonClicked()
        {
            _uiService.ShowWindow<CoreWindow>();
        }
        
        #endregion

        #region Other

        private void UpdateViews()
        {
            foreach(var view in _technologyViews)
                view.UpdateView(_game.ResearchingTarget);
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