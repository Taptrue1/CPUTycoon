using Core.Datas;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "CurrencySettings", menuName = "Settings/CurrencySettings")]
    public class CurrencySettings : ScriptableObject
    {
        [field: SerializeField] public CurrencyData[] CurrenciesDatas { get; private set; }
    }
}