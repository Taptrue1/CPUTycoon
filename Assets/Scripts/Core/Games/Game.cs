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
        public List<Technology> Technologies { get; private set; }
        public DateTime Date { get; private set; }
        
        private readonly Market _market;
        
        public Game(TickService tickService, CoreSettings coreSettings)
        {
            Date = new DateTime(2020, 1, 1);
            Company = new Company("Test", 1000, tickService);
            Technologies = GetTechnologies(coreSettings.TechnologiesSettings.Technologies);
            
            _market = new Market(Company);

            tickService.Tick += OnTick;
        }

        private void OnTick()
        {
            UpdateDate();
        }

        private void UpdateDate()
        {
            Date = Date.AddDays(1);
            OnDateChanged?.Invoke(Date);
        }
        
        private List<Technology> GetTechnologies(IEnumerable<TechnologyData> technologiesData)
        {
            return technologiesData.Select(technologyData => new Technology(technologyData)).ToList();
        }
    }
}