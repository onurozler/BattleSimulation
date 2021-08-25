using Core.Model.Config.Unit;
using Core.Util;
using UnityEngine;

namespace Core.Model
{
    public class UnitData
    {
        public ObservableProperty<Vector3> InitialPosition { get; }
        public ObservableProperty<Vector3> InitialRotation { get; }
        public ObservableProperty<Color> Color { get; }
        public ObservableProperty<float> Size { get;}
        public ObservableProperty<Mesh> Mesh { get;}
        public ObservableProperty<int> ArmyId { get;}
        public ObservableProperty<int> Health { get;}
        public Vector3 CurrentPosition { get; set; }
        public int Attack { get; private set; }
        public float AttackSpeed { get; private set; }
        public int Speed { get; private set; }

        private UnitData()
        {
            ArmyId = new ObservableProperty<int>();
            InitialRotation = new ObservableProperty<Vector3>();
            InitialPosition = new ObservableProperty<Vector3>();
            Color = new ObservableProperty<Color>();
            Size = new ObservableProperty<float>();
            Mesh = new ObservableProperty<Mesh>();
            Health = new ObservableProperty<int>();
        }
        
        public class Builder
        {
            private readonly UnitData _unitData;
            private UnitBaseConfigData _unitBaseConfigData;
            private UnitColorConfigData _unitColorConfigData;
            private UnitShapeConfigData _unitShapeConfigData;
            private UnitSizeConfigData _unitSizeConfigData;
            
            public Builder(UnitData unitData = null)
            {
                _unitData = unitData ?? new UnitData();
            }
            
            public Builder SetArmy(int armyId)
            {
                _unitData.ArmyId.Value = armyId;
                return this;
            }

            public Builder SetBase(UnitBaseConfigData unitBaseConfigData)
            {
                _unitBaseConfigData = unitBaseConfigData;
                return this;
            }
            
            public Builder SetColor(UnitColorConfigData unitColorConfigData)
            {
                _unitColorConfigData = unitColorConfigData;
                return this;
            }
            
            public Builder SetShape(UnitShapeConfigData unitShapeConfigData)
            {
                _unitShapeConfigData = unitShapeConfigData;
                return this;
            }
            
            public Builder SetSize(UnitSizeConfigData unitSizeConfigData)
            {
                _unitSizeConfigData = unitSizeConfigData;
                return this;
            }

            public UnitData Build()
            {
                _unitData.Mesh.Value = _unitShapeConfigData.Mesh;
                _unitData.Color.Value = _unitColorConfigData.Color;
                _unitData.Size.Value = _unitSizeConfigData.ScaleEffect;
                _unitData.Health.Value = _unitBaseConfigData.Health + _unitColorConfigData.HealthEffect + _unitShapeConfigData.HealthEffect + _unitSizeConfigData.HealthEffect;
                _unitData.Attack = _unitBaseConfigData.Attack + _unitColorConfigData.AttackEffect + _unitShapeConfigData.AttackEffect;
                _unitData.AttackSpeed = _unitBaseConfigData.AttackSpeed + _unitColorConfigData.AttackSpeedEffect;
                _unitData.Speed = _unitBaseConfigData.Speed + _unitColorConfigData.SpeedEffect;
                return _unitData;
            }
        }
    }
}