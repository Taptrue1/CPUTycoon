using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "CoreSettings", menuName = "Settings/CoreSettings")]
    public class CoreSettings : ScriptableObject
    {
        [field: SerializeField] public GameSpeedSettings GameSpeedSettings { get; private set; }
        [field: SerializeField] public TechnologiesSettings TechnologiesSettings { get; private set; }
        [field: SerializeField] public UISettings UISettings { get; private set; }
    }
}