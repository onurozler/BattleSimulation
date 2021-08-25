using Core.Controller;
using Core.Model;
using Core.Model.Commands;

namespace Core.Command
{
    public class KillUnitCommand : ICommand<KillUnitCommandData>
    {
        private readonly ArmiesData _armiesData;
        private readonly UnitController.Pool _unitControllerPool;
        
        public KillUnitCommand(ArmiesData armiesData, UnitController.Pool unitControllerPool)
        {
            _armiesData = armiesData;
            _unitControllerPool = unitControllerPool;
        }
        
        public void Execute(KillUnitCommandData commandData)
        {
            _armiesData.RemoveUnit(commandData.UnitData);
            _unitControllerPool.Despawn(commandData.UnitController);
        }
    }
}