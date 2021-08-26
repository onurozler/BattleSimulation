using System;
using System.Collections.Generic;
using System.Linq;
using Core.Util;
using UnityEngine;

namespace Core.Model
{
    public class ArmiesData
    {
        public event Action OnArmyDestroyed;
        
        private readonly Dictionary<int,IList<UnitData>> _unitDataDictionary;

        public ArmiesData()
        {
            _unitDataDictionary = new Dictionary<int, IList<UnitData>>();
        }

        public void AddUnit(UnitData unitData)
        {
            if (_unitDataDictionary.TryGetValue(unitData.ArmyId.Value, out var unitDataList))
            {
                unitDataList.Add(unitData);
            }
            else
            {
                var unitDatas = new List<UnitData> {unitData};
                _unitDataDictionary[unitData.ArmyId.Value] = unitDatas;
            }
        }

        public void RemoveUnit(UnitData unitData)
        {
            if (_unitDataDictionary.TryGetValue(unitData.ArmyId.Value,out var unitDataList))
            {
                unitDataList.Remove(unitData);
                if(unitDataList.Count == 0)
                    OnArmyDestroyed?.Invoke();
            }
        }

        public IList<UnitData> GetUnitsById(int armyId)
        {
            return _unitDataDictionary.TryGetValue(armyId, out var unitDataList) ? unitDataList : null;
        }

        public IList<UnitData> GetAllUnits()
        {
            return _unitDataDictionary.Values.SelectMany(x => x).ToList();
        }

        public UnitData GetClosestEnemyUnit(int armyId, Vector3 unitPosition)
        {
            var enemyArmy = _unitDataDictionary.GetOtherListRandomly(armyId);
            return enemyArmy.OrderBy(unit => Vector3.Distance(unit.CurrentPosition, unitPosition)).FirstOrDefault();
        }

        public void Clear()
        {
            _unitDataDictionary.Clear();
        }
    }
}