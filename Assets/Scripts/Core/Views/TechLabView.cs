using System.Collections.Generic;
using Core.Technologies;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views
{
    public class TechLabView : MonoBehaviour
    {
        //Tests
        [SerializeField] private Button _openButton;

        [Header("Settings")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private Transform _contentTransfrom;
        [SerializeField] private TechnologyView _technologyViewPrefab;

        private List<Technology> _technologies;
        private List<TechnologyView> _technologyViews = new();
        
        private void Awake()
        {
            var technologyDatas = Resources.LoadAll<TechnologyConfig>($"Data/Technologies");
            _technologies = new List<Technology>();
            foreach (var technologyData in technologyDatas)
            {
                _technologies.Add(new Technology(technologyData));
            }

            _openButton.onClick.AddListener(OnOpenButtonClicked);
        }
        
        public void Init(List<Technology> technologies)
        {
            _technologies = technologies;
        }
        
        private void OnOpenButtonClicked()
        {
            if(_panel.gameObject.activeSelf)
                Hide();
            else
                Show();
        }
        
        private void Show()
        {
            _panel.gameObject.SetActive(true);
            SpawnTechViews();
        }
        private void Hide()
        {
            _panel.gameObject.SetActive(false);
            foreach (var technologyView in _technologyViews)
            {
                Destroy(technologyView.gameObject);
            }
            _technologyViews.Clear();
        }
        private void SpawnTechViews()
        {
            foreach (var technology in _technologies)
            {
                var techView = Instantiate(_technologyViewPrefab, _contentTransfrom);
                techView.Init(technology);
                _technologyViews.Add(techView);
            }
        }
    }
}