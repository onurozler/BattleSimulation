using System;
using UnityEngine;

namespace Core.Model.Config.Unit
{
    [Serializable]
    public class UnitColorConfigData
    {
        public Color Color;
        public int HealthEffect;
        public int AttackEffect;
        public int SpeedEffect;
        public float AttackSpeedEffect;
    }
}