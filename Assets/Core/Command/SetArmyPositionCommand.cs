using Core.Model;
using Core.Model.Commands;
using Core.Model.Config;
using Core.Model.Config.Formation;
using UnityEngine;
using Zenject;

namespace Core.Command
{
    public class SetArmyPositionCommand : ICommand<SetArmyPositionCommandData>
    {
        [Inject]
        private FormationConfigData[] _formationConfigDatas;
        
        private readonly ArmiesData _armiesData;

        public SetArmyPositionCommand(ArmiesData armiesData)
        {
            _armiesData = armiesData;
        }

        public void Execute(SetArmyPositionCommandData commandData)
        {
            var formationPositions = commandData.FormationConfigData.Positions;
            var unitList=_armiesData.GetUnitsById(commandData.ArmyId);
            for (var i = 0; i < unitList.Count; i++)
            {
                var unitData = unitList[i];
                var increaseVector = new Vector3(Constants.UnitSpacing * formationPositions[i].x, 0, Constants.UnitSpacing * -formationPositions[i].y);

                increaseVector = Quaternion.AngleAxis(commandData.StartRotation.y, Vector3.right) * increaseVector;
                unitData.InitialPosition.Value = commandData.StartPosition + increaseVector;
                unitData.InitialRotation.Value = commandData.StartRotation;
            }
        }
    }
}