using System.Collections.Generic;

namespace Core.Model
{
    public class ArmiesData
    {
        private readonly Dictionary<int,IList<UnitData>> _unitDataDictionary;

        public ArmiesData()
        {
            _unitDataDictionary = new Dictionary<int, IList<UnitData>>();
        }

        public void AddUnitById(int armyId, UnitData unitData)
        {
            if (_unitDataDictionary.TryGetValue(armyId, out var unitDataList))
            {
                unitDataList.Add(unitData);
            }
            else
            {
                var unitDatas = new List<UnitData> {unitData};
                _unitDataDictionary[armyId] = unitDatas;
            }
        }

        public IList<UnitData> GetUnitsById(int armyId)
        {
            return _unitDataDictionary.TryGetValue(armyId, out var unitDataList) ? unitDataList : null;
        }
    }
}