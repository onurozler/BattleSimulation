using Core.Model;
using Core.Model.Config.Unit;
using Core.Pool;
using Core.Util;

namespace Core.Command
{
    public class CreateArmyCommand : ICommand<CreateArmySignal>
    {
        private readonly ArmiesData _armiesData;
        private readonly UnitControllerPool _unitControllerPool;
        private readonly UnitConfigData _unitConfigData;

        public CreateArmyCommand(ArmiesData armiesData, UnitControllerPool unitControllerPool, UnitConfigData unitConfigData)
        {
            _armiesData = armiesData;
            _unitControllerPool = unitControllerPool;
            _unitConfigData = unitConfigData;
        }

        public void Execute(CreateArmySignal createArmySignal)
        {
            for (int i = 0; i < createArmySignal.ArmySize; i++)
            {
                var unitBase = _unitConfigData.UnitBaseConfigData;
                var unitColor = _unitConfigData.UnitColorConfigData.GetRandom();
                var unitShape = _unitConfigData.UnitShapeConfigData.GetRandom();
                var unitSize = _unitConfigData.UnitSizeConfigData.GetRandom();
                var unitArmy = createArmySignal.ArmyId;

                var unitData = new UnitData.Builder()
                    .SetArmy(unitArmy)
                    .SetBase(unitBase)
                    .SetColor(unitColor)
                    .SetShape(unitShape)
                    .SetSize(unitSize)
                    .Build();
                
                _armiesData.AddUnit(unitData);
                _unitControllerPool.Spawn(unitData);
            }
        }
    }
}