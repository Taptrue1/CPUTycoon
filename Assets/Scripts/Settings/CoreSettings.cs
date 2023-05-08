using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "CoreSettings", menuName = "Settings/CoreSettings")]
    public class CoreSettings : ScriptableObject
    {
        [field: SerializeField] public GameSpeedSettings GameSpeedSettings { get; private set; }
        [field: SerializeField] public UISettings UISettings { get; private set; }
        [field: SerializeField] public MarketSettings MarketSettings { get; private set; }
        [field: SerializeField] public CurrencySettings CurrencySettings { get; private set; }
        [field: SerializeField] public string TechTreePath { get; private set; }
    }
}