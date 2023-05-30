using Core.Team;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "TeamSettings", menuName = "Settings/TeamSettings")]
    public class TeamSettings : ScriptableObject
    {
        [field: SerializeField] public int FreeWorkersCount { get; private set; }
        [field: SerializeField] public int FreeWorkersGenerateDelay { get; private set; }
        [field: SerializeField] public string[] Names { get; private set; }
        [field: SerializeField] public string[] Surnames { get; private set; }
        [field: SerializeField] public Office[] Offices { get; private set; }
        [field: SerializeField] public GameObject[] WorkerViews { get; private set; }
    }
}