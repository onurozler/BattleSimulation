using System;
using Core.Command;
using Core.Model;
using Core.Model.Commands;
using Core.Model.Config.Game;
using Core.View.Player;
using Zenject;

namespace Core.Controller
{
    public class PlayerController : IInitializable, IDisposable
    {
        private readonly GameConfigData _gameConfigData;
        private readonly IPlayerView _playerView;
        private readonly PlayerData _playerData;
        private readonly CreateArmyCommand _createArmyCommand;
        private readonly SetArmyPositionCommand _setArmyPositionCommand;
        private readonly ShuffleArmyCommand _shuffleArmyCommand;

        public PlayerController(GameConfigData gameConfigData, IPlayerView playerView, PlayerData playerData,
            CreateArmyCommand createArmyCommand, ShuffleArmyCommand shuffleArmyCommand, SetArmyPositionCommand setArmyPositionCommand)
        {
            _gameConfigData = gameConfigData;
            _playerView = playerView;
            _playerData = playerData;
            _createArmyCommand = createArmyCommand;
            _shuffleArmyCommand = shuffleArmyCommand;
            _setArmyPositionCommand = setArmyPositionCommand;

            _playerView.OnPlayPressed += OnPlayPressed;
            _playerView.OnShufflePressed += OnShufflePressed;
        }
        
        public void Initialize()
        {
            var shuffleButtonIds = new int[_gameConfigData.GameArmyConfigData.Length];
            
            for (var index = 0; index < _gameConfigData.GameArmyConfigData.Length; index++)
            {
                var gameArmyConfigData = _gameConfigData.GameArmyConfigData[index];
                _createArmyCommand.Execute(new CreateArmyCommandData
                {
                    ArmyId = index,
                    ArmySize = gameArmyConfigData.ArmySize,
                });
                _setArmyPositionCommand.Execute(new SetArmyPositionCommandData
                {
                    ArmyId = index,
                    StartPosition = gameArmyConfigData.StartPosition
                });
                
                shuffleButtonIds[index] = index;
            }
            
            _playerView.SetShuffleButtonsId(shuffleButtonIds);
        }
        
        private void OnShufflePressed(int armyId)
        {
            _shuffleArmyCommand.Execute(new ShuffleArmyCommandData {ArmyId = armyId});
        }
        
        private void OnPlayPressed()
        {
            _playerData.State.Value = SimulationState.Playing;
            _playerView.Hide();
        }

        public void Dispose()
        {
            _playerView.OnPlayPressed -= OnPlayPressed;
            _playerView.OnShufflePressed -= OnShufflePressed;
        }
    }
}