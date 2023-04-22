using UnityEngine;
using Utils;

namespace Core.Datas
{
    [CreateAssetMenu(fileName = "TechnologyData", menuName = "Data/TechnologyData")]
    public class TechnologyData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Progression ResearchCost { get; private set; }
        [field: SerializeField] public Progression DevelopmentCost { get; private set; }
        [field: SerializeField] public Progression Power { get; private set; }
    }
}