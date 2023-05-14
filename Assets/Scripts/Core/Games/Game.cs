using Core.CPU;
using Core.Services;
using Core.Technologies;
using UnityEngine;
using Zenject;

namespace Core.Games
{
    public class Game
    {
        public Processor ProcessorToDevelop => _processorToDevelop;
        public Technology TechnologyToResearch => _technologyToResearch;
        
        private Processor _processorToDevelop;
        private Technology _technologyToResearch;
        
        private readonly CurrencyService _currencyService;
        //TODO delete const and set values by TeamService
        private const int RPPerDay = 1;
        private const int DPPerDay = 1;
        private const int RPPrice = 100;
        private const int DPPrice = 100;
        
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

        #region TickMethods
        
        private void OnTick()
        {
            //TODO change RP, DP and Money to CurrencyData
            var money = _currencyService.GetCurrency("Money");
            var researchPoints = _currencyService.GetCurrency("RP");
            var developmentPoints = _currencyService.GetCurrency("DP");
            var researchPointsPrice = RPPerDay * RPPrice;
            var developmentPointsPrice = DPPerDay * DPPrice;
            
            if(_technologyToResearch != null && money.Value >= researchPointsPrice)
            {
                researchPoints.Value += RPPerDay;
                money.Value -= researchPointsPrice;
                Debug.Log("Researching...");
                if (_technologyToResearch.ResearchPrice > researchPoints.Value) return;
                researchPoints.Value = 0;
                _technologyToResearch.Research();
                _technologyToResearch = null;
                Debug.Log("Researched!");
            }
            if(_processorToDevelop != null && money.Value >= developmentPointsPrice)
            {
                developmentPoints.Value += DPPerDay;
                money.Value -= developmentPointsPrice;
                Debug.Log("Developing...");
                if (_processorToDevelop.DevelopmentPoints > developmentPoints.Value) return;
                developmentPoints.Value = 0;
                _processorToDevelop = null;
                Debug.Log("Developed!");
            }
        }

        #endregion
    }
}