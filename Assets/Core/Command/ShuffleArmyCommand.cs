using Core.Model;
using Core.Model.Config.Unit;
using Core.Util;

namespace Core.Command
{
    public class ShuffleArmyCommand : ICommand<ShuffleArmySignal>
    {
        private readonly ArmiesData _armiesData;
        private readonly UnitConfigData _unitConfigData;

        public ShuffleArmyCommand(ArmiesData armiesData, UnitConfigData unitConfigData)
        {
            _armiesData = armiesData;
            _unitConfigData = unitConfigData;
        }

        public void Execute(ShuffleArmySignal signal)
        {
            var armyUnits = _armiesData.GetUnitsById(signal.ArmyId);
            for (int i = 0; i < armyUnits.Count; i++)
            {
                var unitBase = _unitConfigData.UnitBaseConfigData;
                var unitColor = _unitConfigData.UnitColorConfigData.GetRandom();
                var unitShape = _unitConfigData.UnitShapeConfigData.GetRandom();
                var unitSize = _unitConfigData.UnitSizeConfigData.GetRandom();
                
                armyUnits[i] = new UnitData.Builder(armyUnits[i])
                                    .SetBase(unitBase)
                                    .SetColor(unitColor)
                                    .SetShape(unitShape)
                                    .SetSize(unitSize)
                                    .Build();
            }
        }
    }
}