using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "GameSpeedSettings", menuName = "Settings/GameSpeedSettings")]
    public class GameSpeedSettings : ScriptableObject
    {
        [field: SerializeField] public float PauseTimeScale { get; private set; }
        [field: SerializeField] public float NormalTimeScale { get; private set; }
        [field: SerializeField] public float FastTimeScale { get; private set; }
        [field: SerializeField] public float FastestTimeScale { get; private set; }
    }
}