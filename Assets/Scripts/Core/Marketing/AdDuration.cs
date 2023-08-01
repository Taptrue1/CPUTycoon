using System;
using UnityEngine;

namespace Core.Marketing
{
    [Serializable]
    public class AdDuration
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public double PriceMultiplier { get; private set; }
    }
}