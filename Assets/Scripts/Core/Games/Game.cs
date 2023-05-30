using System;
using Core.CPU;
using Core.Services;
using Core.Technologies;
using UnityEngine;
using Zenject;

namespace Core.Games
{
    public class Game
    {
        public event Action<Processor> ProcessorDeveloped;

        public Processor ProcessorToDevelop => _processorToDevelop;
        public Technology TechnologyToResearch => _technologyToResearch;
        
        private Processor _processorToDevelop;
        private Technology _technologyToResearch;
        private readonly CurrencyService _currencyService;

        [Inject]
        public Game(CurrencyService currencyService, TimeService timeService)
        {
            _currencyService = currencyService;

            timeService.Tick += OnTick;
        }
        public void SetTechnologyToResearch(Technology technology)
        {
            _technologyToResearch = technology;
        }
        public void SetProcessorToDevelop(Processor processor)
        {
            _processorToDevelop = processor;
        }

        #region Callbacks
        
        private void OnTick()
        {
            //TODO change RP and DP to CurrencyData
            var researchPoints = _currencyService.GetCurrency("RP");
            var developmentPoints = _currencyService.GetCurrency("DP");

            if(_technologyToResearch != null)
            {
                Debug.Log("Researching...");
                if (_technologyToResearch.ResearchPrice > researchPoints.Value) return;
                researchPoints.Value = 0;
                _technologyToResearch.Research();
                _technologyToResearch = null;
                Debug.Log("Researched!");
            }
            if(_processorToDevelop != null)
            {
                Debug.Log("Developing...");
                if (_processorToDevelop.DevelopmentPointsPrice > developmentPoints.Value) return;
                developmentPoints.Value = 0;
                ProcessorDeveloped?.Invoke(_processorToDevelop);
                _processorToDevelop = null;
                Debug.Log("Developed!");
            }
        }

        #endregion
    }
}