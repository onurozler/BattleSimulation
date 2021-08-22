using UnityEngine;

namespace Core.Model.Commands
{
    public struct CreateArmyCommandData
    {
        public int ArmyId;
        public int ArmySize;
    }

    public struct ShuffleArmyCommandData
    {
        public int ArmyId;
    }

    public struct SetArmyPositionCommandData
    {
        public int ArmyId;
        public Vector3 StartPosition;
    }
}