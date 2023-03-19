using System.Collections.Generic;
using Core.Views;

namespace Core.Technologies
{
    public class TechnologyLaboratory : ITickable
    {
        private double _pointsPerDay;
        private double _pointPrice;
        private Technology _currentResearchingTechnology;
        private List<Technology> _allTechnologies;
        private TechLabView _view;

        public TechnologyLaboratory(double pointsPerDay, double pointPrice, List<Technology> allTechnologies)
        {
            _pointsPerDay = pointsPerDay;
            _pointPrice = pointPrice;
            _allTechnologies = allTechnologies;
        }
        
        public void Tick()
        {
            ResearchTechnology();
        }
        public void LinkView(TechLabView view)
        {
            _view = view;
        }

        private void ResearchTechnology()
        {
            if (_currentResearchingTechnology == null) return;

            _currentResearchingTechnology.AddExperience(_pointsPerDay);
        }
    }
}