using UnityEngine;

namespace Core.Datas
{
    [CreateAssetMenu(fileName = "CurrencyData", menuName = "Data/CurrencyData")]
    public class CurrencyData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int StartValue { get; private set; }
    }
}