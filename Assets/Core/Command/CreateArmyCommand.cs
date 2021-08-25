using Core.Controller;
using Core.Model;
using Core.Model.Commands;
using Core.Model.Config.Unit;
using Core.Util;

namespace Core.Command
{
    public class CreateArmyCommand : ICommand<CreateArmyCommandData>
    {
        private readonly ArmiesData _armiesData;
        private readonly UnitController.Pool _unitControllerPool;
        private readonly UnitConfigData _unitConfigData;

        public CreateArmyCommand(ArmiesData armiesData, UnitController.Pool unitControllerPool, UnitConfigData unitConfigData)
        {
            _armiesData = armiesData;
            _unitControllerPool = unitControllerPool;
            _unitConfigData = unitConfigData;
        }

        public void Execute(CreateArmyCommandData createArmyCommandData)
        {
            for (int i = 0; i < createArmyCommandData.ArmySize; i++)
            {
                var unitBase = _unitConfigData.UnitBaseConfigData;
                var unitColor = _unitConfigData.UnitColorConfigData.GetRandom();
                var unitShape = _unitConfigData.UnitShapeConfigData.GetRandom();
                var unitSize = _unitConfigData.UnitSizeConfigData.GetRandom();
                var unitArmy = createArmyCommandData.ArmyId;

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