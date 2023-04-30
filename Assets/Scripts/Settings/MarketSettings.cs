using Core.Datas;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "MarketSettings", menuName = "Settings/MarketSettings")]
    public class MarketSettings : ScriptableObject
    {
        //TODO change to progression
        [field: SerializeField] public int ClientsCount { get; private set; }
        [field: SerializeField] public CompetitorData[] Competitors { get; private set; }
    }
}