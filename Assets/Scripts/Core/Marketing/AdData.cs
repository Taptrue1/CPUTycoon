using System;
using UnityEngine;

namespace Core.Marketing
{
    [Serializable]
    public class AdData
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public double Price { get; private set; }
        [field: SerializeField] public double SalesMultiplier { get; private set; }
    }
}