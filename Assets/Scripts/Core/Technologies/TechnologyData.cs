using UnityEngine;

namespace Core.Technologies
{
    [CreateAssetMenu(fileName = "TechnologyData", menuName = "ScriptableObjects/TechnologyData", order = 0)]
    public class TechnologyData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Progression PowerProgression { get; private set; }
        [field: SerializeField] public Progression LevelProgression { get; private set; }
    }
}