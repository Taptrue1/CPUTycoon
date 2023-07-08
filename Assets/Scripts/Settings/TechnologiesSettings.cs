using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "TechnologiesData", menuName = "Data/TechnologiesData")]
    public class TechnologiesSettings : ScriptableObject
    {
        [field: Header("Icons")]
        [field: SerializeField] public Sprite TechProcessIcon { get; private set; }
        [field: SerializeField] public Sprite FrequencyIcon { get; private set; }
        [field: SerializeField] public Sprite FormFactorIcon { get; private set; }
        [field: SerializeField] public Sprite RamIcon { get; private set; }
        [field: SerializeField] public Sprite BitsIcon { get; private set; }
    }
}