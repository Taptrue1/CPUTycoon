using System;
using UnityEngine;

namespace Core.Datas
{
    [CreateAssetMenu(fileName = "CompetitorData", menuName = "Data/CompetitorData")]
    public class CompetitorData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ProductData[] ProductReleases { get; private set; }
    }
    
    [Serializable]
    public class ProductData
    {
        //TODO change power to Option like in ClientGroupData
        public string Name;
        public double Price;
        public double Power;
        public int AppearDay;
        public int Duration;
    }
}