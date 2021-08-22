using Core.Model;
using Core.Model.Commands;
using UnityEngine;

namespace Core.Command
{
    public class SetArmyPositionCommand : ICommand<SetArmyPositionCommandData>
    {
        private readonly ArmiesData _armiesData;

        public SetArmyPositionCommand(ArmiesData armiesData)
        {
            _armiesData = armiesData;
        }

        public void Execute(SetArmyPositionCommandData commandData)
        {
            var unitList=_armiesData.GetUnitsById(commandData.ArmyId);
            for (var index = 0; index < unitList.Count; index++)
            {
                var unitData = unitList[index];
                var increaseVector = new Vector3(3 * (index % 5), 0, 3 * Mathf.FloorToInt(index / 5f));
                
                unitData.Position.Value = commandData.StartPosition + increaseVector;
            }
        }
    }
}