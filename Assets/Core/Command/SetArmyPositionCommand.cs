using System;
using Core.Model;
using Core.Model.Config;
using UnityEngine;

namespace Core.Command
{
    public class SetArmyPositionCommand : ICommand<SetArmyPositionSignal>
    {
        private readonly ArmiesData _armiesData;

        public SetArmyPositionCommand(ArmiesData armiesData)
        {
            _armiesData = armiesData;
        }

        public void Execute(SetArmyPositionSignal signal)
        {
            var unitList=_armiesData.GetUnitsById(signal.ArmyId);
            if (unitList.Count != signal.FormationConfigData.UnitSize)
            {
                throw new Exception("Unit size and formation size must match!");
            }
            
            var formationPositions = signal.FormationConfigData.Positions;
            for (var i = 0; i < unitList.Count; i++)
            {
                var unitData = unitList[i];
                var increaseVector = new Vector3(Constants.UnitSpacing * formationPositions[i].x, 0, Constants.UnitSpacing * -formationPositions[i].y);

                increaseVector = Quaternion.AngleAxis(signal.StartRotation.y, Vector3.right) * increaseVector;
                unitData.InitialPosition.Value = signal.StartPosition + increaseVector;
                unitData.InitialRotation.Value = signal.StartRotation;
            }
        }
    }
}