using System;
using UnityEngine;

namespace Core.Datas
{
    [CreateAssetMenu(fileName = "CompetitorData", menuName = "Data/CompetitorData")]
    public class CompetitorData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ProductReleaseData[] ProductReleases { get; private set; }
    }
    
    [Serializable]
    public class ProductReleaseData
    {
        public string Name;
        public int Price;
        public int Power;
        public int StartSellDay;
        public int EndSellDay;
    }
}