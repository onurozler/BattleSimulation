using Core.Controller;
using Core.Model.Config.Formation;
using UnityEngine;

namespace Core.Model
{
    public class CreateArmySignal
    {
        public int ArmyId;
        public int ArmySize;
    }

    public class ShuffleArmySignal
    {
        public int ArmyId;
    }

    public class SetArmyPositionSignal
    {
        public int ArmyId;
        public Vector3 StartPosition;
        public Vector3 StartRotation;
        public FormationConfigData FormationConfigData;
    }

    public class KillUnitSignal
    {
        public UnitController UnitController;
        public UnitData UnitData;
    }
}