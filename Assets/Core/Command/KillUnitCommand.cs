using Core.Model;
using Core.Pool;

namespace Core.Command
{
    public class KillUnitCommand : ICommand<KillUnitSignal>
    {
        private readonly ArmiesData _armiesData;
        private readonly UnitControllerPool _unitControllerPool;
        
        public KillUnitCommand(ArmiesData armiesData, UnitControllerPool unitControllerPool)
        {
            _armiesData = armiesData;
            _unitControllerPool = unitControllerPool;
        }
        
        public void Execute(KillUnitSignal signal)
        {
            _unitControllerPool.Despawn(signal.UnitController);
            _armiesData.RemoveUnit(signal.UnitData);
        }
    }
}