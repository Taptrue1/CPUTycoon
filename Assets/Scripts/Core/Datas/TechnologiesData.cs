using System;
using UnityEngine;

namespace Core.Datas
{
    [CreateAssetMenu(fileName = "TechnologiesData", menuName = "Data/TechnologiesData")]
    public class TechnologiesData : ScriptableObject
    {
        [field: SerializeField] public TechProcessPair[] TechProcesses { get; private set; }
        [field: SerializeField] public FrequencyPair[] Frequencies { get; private set; }
        [field: SerializeField] public FormFactorPair[] FormFactors { get; private set; }
        [field: SerializeField] public RamPair[] Ram { get; private set; }
        [field: SerializeField] public int[] Bits { get; private set; }
    }
    
    [Serializable]
    public class TechProcessPair
    {
        public int TechProcess { get; private set; }
        public TechProcessMesureUnit MesureUnit { get; private set; }
    }
    [Serializable]
    public class FrequencyPair
    {
        public int Frequency { get; private set; }
        public FrequencyMesureUnit MesureUnit { get; private set; }
    }
    [Serializable]
    public class RamPair
    {
        public int Ram { get; private set; }
        public MemoryMesureUnit MesureUnit { get; private set; }
    }
    [Serializable]
    public class FormFactorPair
    {
        public string FormFactorName { get; private set; }
        public int DevelopmentPointsPrice { get; private set; }
        public int ImplementPrice { get; private set; }
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