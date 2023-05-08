using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "TechnologiesData", menuName = "Data/TechnologiesData")]
    public class TechnologiesSettings : ScriptableObject
    {
        [field: SerializeField] public TechProcessPair[] TechProcesses { get; private set; }
        [field: SerializeField] public FrequencyPair[] Frequencies { get; private set; }
        [field: SerializeField] public FormFactorPair[] FormFactors { get; private set; }
        //now unusable but in futurue will be used
        [field: Space()]
        [field: SerializeField] public RamPair[] Ram { get; private set; }
        [field: SerializeField] public int[] Bits { get; private set; }
    }
    
    [Serializable]
    public class TechProcessPair
    {
        [field: HorizontalGroup("TechProcess"), LabelWidth(100), Range(1, 1000)]
        [field: SerializeField] public int TechProcess { get; private set; }
        [field: HideLabel, HorizontalGroup("TechProcess", Width = 100)]
        [field: SerializeField]public TechProcessMesureUnit MesureUnit { get; private set; }
    }
    [Serializable]
    public class FrequencyPair
    {
        [field: HorizontalGroup("Frequency"), LabelWidth(100), Range(1, 1000)]
        [field: SerializeField] public int Frequency { get; private set; }
        [field: HideLabel, HorizontalGroup("Frequency", Width = 100)]
        [field: SerializeField] public FrequencyMesureUnit MesureUnit { get; private set; }
    }
    [Serializable]
    public class RamPair
    {
        [field: HorizontalGroup("Ram"), Range(1, 1000)]
        [field: SerializeField] public int Ram { get; private set; }
        [field: HideLabel, HorizontalGroup("Ram", Width = 100)]
        [field: SerializeField] public MemoryMesureUnit MesureUnit { get; private set; }
    }
    [Serializable]
    public class FormFactorPair
    {
        [field: SerializeField] public string FormFactorName { get; private set; }
        [field: SerializeField] public int DevelopmentPointsPrice { get; private set; }
        [field: SerializeField] public int ImplementPrice { get; private set; }
    }
    
    
    public enum FrequencyMesureUnit
    {
        Hz,
        KHz,
        MHz,
        GHz,
        THz
    }
    public enum TechProcessMesureUnit
    {
        Mkm,
        Nm
    }
    public enum MemoryMesureUnit
    {
        Byte,
        KByte,
        MByte,
        GByte,
        TByte,
        PByte
    }
}