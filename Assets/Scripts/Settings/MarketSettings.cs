using System;
using Core.Datas;
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
    }

    [Serializable]
    public class ClientGroup
    {
        [field: SerializeField] public PriorityType Priority { get; set; }
        [field: SerializeField] public int ClientsPercentage { get; set; } 
    }

    public enum PriorityType
    {
        Price,
        Balance,
        Power
    }
}