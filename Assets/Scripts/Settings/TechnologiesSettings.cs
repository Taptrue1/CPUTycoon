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
        [field: SerializeField] public RamPair[] Ram { get; private set; }
        [field: SerializeField] public BitsPair[] Bits { get; private set; }
    }
    [Serializable]
    public abstract class BasePair
    {
        [field: SerializeField] public int Power { get; private set; }
        [field: SerializeField] public int DevelopmentPrice { get; private set; }
    }
    [Serializable]
    public class TechProcessPair : BasePair
    {
        [field: HorizontalGroup("TechProcess"), LabelWidth(100), Range(1, 999)]
        [field: SerializeField] public int TechProcess { get; private set; }
        [field: HideLabel, HorizontalGroup("TechProcess", Width = 100)]
        [field: SerializeField] public TechProcessMesureUnit MesureUnit { get; private set; }
    }
    [Serializable]
    public class FrequencyPair : BasePair
    {
        [field: HorizontalGroup("Frequency"), LabelWidth(100), Range(1, 999)]
        [field: SerializeField] public int Frequency { get; private set; }
        [field: HideLabel, HorizontalGroup("Frequency", Width = 100)]
        [field: SerializeField] public FrequencyMesureUnit MesureUnit { get; private set; }
    }
    [Serializable]
    public class RamPair : BasePair
    {
        [field: HorizontalGroup("Ram"), Range(1, 999)]
        [field: SerializeField] public int Ram { get; private set; }
        [field: HideLabel, HorizontalGroup("Ram", Width = 100)]
        [field: SerializeField] public MemoryMesureUnit MesureUnit { get; private set; }
    }
    [Serializable]
    public class FormFactorPair : BasePair
    {
        [PropertyOrder(-1)]
        [field: SerializeField] public string FormFactorName { get; private set; }
        [field: SerializeField] public int DevelopmentPointsPrice { get; private set; }
    }
    [Serializable]
    public class BitsPair : BasePair
    {
        [field: SerializeField] public int Bits { get; private set; }
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