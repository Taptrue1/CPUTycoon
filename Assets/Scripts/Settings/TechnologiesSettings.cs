using System.Collections.Generic;
using Core.Datas;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "TechnologiesSettings", menuName = "Settings/TechnologiesSettings")]
    public class TechnologiesSettings : ScriptableObject
    {
        [field: SerializeField] public List<TechnologyData> Technologies { get; private set; }
    }
}