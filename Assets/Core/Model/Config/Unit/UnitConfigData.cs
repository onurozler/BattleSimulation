using UnityEngine;

namespace Core.Model.Config.Unit
{
    [CreateAssetMenu(fileName = "UnitConfigData", menuName = "Game/Config/Create Unit Config", order = 1)]
    public class UnitConfigData : ScriptableObject
    {
        public UnitBaseConfigData UnitBaseConfigData;
        public UnitShapeConfigData[] UnitShapeConfigData;
        public UnitSizeConfigData[] UnitSizeConfigData;
        public UnitColorConfigData[] UnitColorConfigData;
    }
}
