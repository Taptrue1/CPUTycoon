using System;
using System.Collections.Generic;

namespace Core.Technologies
{
    [Serializable]
    public class Technology
    {
        public string Name;
        public int ResearchPrice;
        public int TypeValue;
        public TechnologyType Type;
        public List<Technology> Children;
        
        private bool _isResearched;

        public Technology(string name)
        {
            Name = name;
            Children = new List<Technology>();
        }
        public void Research()
        {
            _isResearched = true;
        }
        public bool IsResearched()
        {
            return Name == "ROOT" || _isResearched;
        }
    }
}