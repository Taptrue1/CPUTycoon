using System;
using Core.Datas;
using Core.Technologies;
using UnityEngine;
using Utils;

namespace Settings
{
    [CreateAssetMenu(fileName = "MarketSettings", menuName = "Settings/MarketSettings")]
    public class MarketSettings : ScriptableObject
    {
        [field: SerializeField] public Progression ClientsCount { get; private set; }
        [field: SerializeField] public ClientGroup[] ClientGroups { get; private set; }
        [field: SerializeField] public CompetitorData[] Competitors { get; private set; }
        [field: SerializeField] public Trend[] Trends { get; private set; }
    }

    [Serializable]
    public class ClientGroup
    {
        [field: SerializeField] public PriorityType Priority { get; set; }
        [field: SerializeField] public int ClientsPercentage { get; set; } 
    }

    [Serializable]
    public class Trend
    {
        [field: SerializeField] public int StartDayOffset { get; private set; }
        [field: SerializeField] public int Duration { get; private set; }
        [field: SerializeField] public float SellPercentBonus { get; private set; }
        [field: SerializeField] public TechnologyType TechnologyType { get; private set; }
        [field: SerializeField] public int TechnologyIndex { get; private set; }
    }

    public enum PriorityType
    {
        Price,
        Balance,
        Power
    }
}