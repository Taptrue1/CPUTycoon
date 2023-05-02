using UnityEngine;
using Utils;

namespace Core.Datas
{
    [CreateAssetMenu(fileName = "TechnologyData", menuName = "Data/TechnologyData")]
    public class TechnologyData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public int Power { get; private set; }
        [field: SerializeField] public int ImplementPrice { get; private set; }
        [field: SerializeField] public int ResearchPointsPrice { get; private set; }
        [field: SerializeField] public int DevelopmentPointsPrice { get; private set; }
    }
}