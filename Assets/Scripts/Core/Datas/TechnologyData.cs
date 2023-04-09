using UnityEngine;
using Utils;

namespace Core.Datas
{
    [CreateAssetMenu(fileName = "TechnologyData", menuName = "Data/TechnologyData")]
    public class TechnologyData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; }
        [field: SerializeField] public string Description { get; }
        [field: SerializeField] public Progression ResearchCost { get; }
        [field: SerializeField] public Progression DevelopmentCost { get; }
        [field: SerializeField] public Progression Power { get; }
    }
}