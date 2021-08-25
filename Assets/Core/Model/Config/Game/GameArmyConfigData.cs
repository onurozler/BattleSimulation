using System;
using UnityEngine;

namespace Core.Model.Config.Game
{
    [Serializable]
    public class GameArmyConfigData
    {
        public int ArmySize;
        public Vector3 StartPosition;
        public Vector3 StartRotation;
    }
}