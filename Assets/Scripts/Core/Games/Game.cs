using System;
using System.Collections.Generic;
using System.Linq;
using Core.Datas;
using Core.Markets;
using Core.Services;
using Core.Technologies;
using Settings;

namespace Core.Games
{
    public class Game
    {
        public event Action<DateTime> OnDateChanged;

        public Company Company { get; private set; }
        public DateTime Date { get; private set; }
        public List<Technology> Technologies { get; private set; }

        private readonly Market _market;
        
        public Game(TickService tickService, CoreSettings coreSettings)
        {
            Date = new DateTime(2020, 1, 1);
            Company = new Company("Test", 1000);
            Technologies = GetTechnologies(coreSettings.TechnologiesSettings.Technologies);
            
            _market = new Market(Date, Company, coreSettings.MarketSettings);

            tickService.Tick += OnTick;
        }

        #region TickMethods
        
        private void OnTick()
        {
            UpdateDate();

            Company.Tick();
            _market.Tick(Date);
        }
        private void UpdateDate()
        {
            Date = Date.AddDays(1);
            OnDateChanged?.Invoke(Date);
        }
        
        #endregion
        
        #region Other
        
        private List<Technology> GetTechnologies(IEnumerable<TechnologyData> technologiesData)
        {
            return technologiesData.Select(technologyData => new Technology(technologyData)).ToList();
        }
        
        #endregion
    }
}