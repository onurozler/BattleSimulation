using System;
using UnityEngine;

namespace Core.Model.Config.Unit
{
    [Serializable]
    public class UnitSizeConfigData
    {
        public UnitSizeType UnitSizeType;
        [Range(0, 2)] 
        public float ScaleEffect;
        public int HealthEffect;
    }

    public enum UnitSizeType
    {
        Big,
        Small
    }
}