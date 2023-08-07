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
        public event Action<Technology> ResearchingTargetChanged;
        public event Action<Processor> DevelopingTargetChanged;

        public Processor DevelopingTarget => _developingTarget;
        public Technology ResearchingTarget => _researchingTarget;
        
        private Processor _developingTarget;
        private Technology _researchingTarget;
        private readonly CurrencyService _currencyService;

        [Inject]
        public Game(CurrencyService currencyService, TimeService timeService)
        {
            _currencyService = currencyService;

            timeService.Tick += OnTick;
        }
        public void SetResearchingTarget(Technology technology)
        {
            _researchingTarget = technology;
            ResearchingTargetChanged?.Invoke(technology);
        }
        public void SetDevelopingTarget(Processor processor)
        {
            _developingTarget = processor;
            DevelopingTargetChanged?.Invoke(processor);
        }

        #region Callbacks
        
        private void OnTick()
        {
            //TODO change RP and DP to CurrencyData
            var researchPoints = _currencyService.GetCurrency("RP");
            var developmentPoints = _currencyService.GetCurrency("DP");

            if(_researchingTarget != null)
            {
                Debug.Log("Researching...");
                if (_researchingTarget.ResearchPoints > researchPoints.Value) return;
                researchPoints.Value = 0;
                _researchingTarget.Research();
                _researchingTarget = null;
                ResearchingTargetChanged?.Invoke(null);
                Debug.Log("Researched!");
            }
            if(_developingTarget != null)
            {
                Debug.Log("Developing...");
                if (_developingTarget.DevelopmentPoints > developmentPoints.Value) return;
                developmentPoints.Value = 0;
                ProcessorDeveloped?.Invoke(_developingTarget);
                _developingTarget = null;
                DevelopingTargetChanged?.Invoke(null);
                Debug.Log("Developed!");
            }
        }

        #endregion
    }
}