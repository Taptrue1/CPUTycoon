using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class Progression
    {
        [field: SerializeField] public double DefaultValue { get; private set; }
        [field: SerializeField] public double GrowthValue { get; private set; }
        [field: SerializeField] public ProgressionType ProgressionType { get; private set; }
        
        public double GetProgressionValue(double level)
        {
            return ProgressionType switch
            {
                ProgressionType.Linear => GetLinearProgression(level),
                ProgressionType.Geometric => GetGeometricProgression(level),
                ProgressionType.Exponential => GetExponentialProgression(level),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private double GetLinearProgression(double level)
        {
            return DefaultValue + GrowthValue * level;
        }
        private double GetGeometricProgression(double level)
        {
            return DefaultValue * GrowthValue * level;
        }
        private double GetExponentialProgression(double level)
        {
            return Math.Pow(DefaultValue, GrowthValue * level);
        }
    }
    
    public enum ProgressionType
    {
        Linear,
        Geometric,
        Exponential
    }
}