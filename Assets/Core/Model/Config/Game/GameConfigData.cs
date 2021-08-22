using UnityEngine;

namespace Core.Model.Config.Game
{
    [CreateAssetMenu(fileName = "GameConfigData", menuName = "Game/Config/Create Game Config", order = 2)]
    public class GameConfigData : ScriptableObject
    {
        public GameArmyConfigData[] GameArmyConfigData;
    }
}