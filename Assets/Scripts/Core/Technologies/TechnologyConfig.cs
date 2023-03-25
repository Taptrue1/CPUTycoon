using UnityEngine;

namespace Core.Technologies
{
    [CreateAssetMenu(menuName = "ScriptableObjects/TechnologyConfig")]
    public class TechnologyConfig : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Progression PowerProgression { get; private set; }
        [field: SerializeField] public Progression ExperienceProgression { get; private set; }
    }
}