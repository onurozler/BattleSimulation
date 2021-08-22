using System;
using UnityEngine;

namespace Core.Model.Config.Unit
{
    [Serializable]
    public class UnitShapeConfigData
    {
        public UnitShapeType UnitShapeType;
        public int HealthEffect;
        public int AttackEffect;
        public Mesh Mesh;
    }

    public enum UnitShapeType
    {
        Cube,
        Sphere
    }
}