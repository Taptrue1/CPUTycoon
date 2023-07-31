using System;
using Core.Team;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "TeamSettings", menuName = "Settings/TeamSettings")]
    public class TeamSettings : ScriptableObject
    {
        [field: SerializeField] public int FreeWorkersCount { get; private set; }
        [field: SerializeField] public int FreeWorkersGenerateDelay { get; private set; }
        [field: SerializeField] public int WorkersMinAge { get; private set; }
        [field: SerializeField] public int WorkersMaxAge { get; private set; }
        [field: SerializeField] public int WorkersMinSalary { get; private set; }
        [field: SerializeField] public int WorkersMaxSalary { get; private set; }
        [field: SerializeField] public int WorkersMinPointsGeneration { get; private set; }
        [field: SerializeField] public int WorkersMaxPointsGeneration { get; private set; }
        [field: SerializeField] public string[] Names { get; private set; }
        [field: SerializeField] public string[] Surnames { get; private set; }
        [field: SerializeField] public Office[] Offices { get; private set; }
        [field: SerializeField] public WorkerPair[] WorkerPairs { get; private set; }
    }
    [Serializable]
    public class WorkerPair
    {
        [field:SerializeField] public Sprite Icon { get; private set; }
        [field:SerializeField] public GameObject WorkerView { get; private set; }
    }
}