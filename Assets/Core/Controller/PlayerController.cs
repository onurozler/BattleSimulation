using System;
using System.Collections.Generic;
using Core.Command;
using Core.Model;
using Core.Model.Commands;
using Core.Model.Config.Formation;
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
        private readonly ArmiesData _armiesData;
        private readonly CreateArmyCommand _createArmyCommand;
        private readonly SetArmyPositionCommand _setArmyPositionCommand;
        private readonly ShuffleArmyCommand _shuffleArmyCommand;

        [Inject] 
        private FormationConfigData[] _formationConfigDatas;
        
        public PlayerController(GameConfigData gameConfigData, IPlayerView playerView, PlayerData playerData,
            CreateArmyCommand createArmyCommand, ShuffleArmyCommand shuffleArmyCommand, SetArmyPositionCommand setArmyPositionCommand,
            ArmiesData armiesData)
        {
            _gameConfigData = gameConfigData;
            _playerView = playerView;
            _playerData = playerData;
            _armiesData = armiesData;
            _createArmyCommand = createArmyCommand;
            _shuffleArmyCommand = shuffleArmyCommand;
            _setArmyPositionCommand = setArmyPositionCommand;

            _armiesData.OnArmyDestroyed += OnArmyDestroyed;
            _playerView.OnPlayPressed += OnPlayPressed;
            _playerView.OnShufflePressed += OnShufflePressed;
            _playerView.OnFormationPressed += OnFormationPressed;
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
                    StartPosition = gameArmyConfigData.StartPosition,
                    StartRotation = gameArmyConfigData.StartRotation,
                    FormationConfigData = _formationConfigDatas[0]
                });
                
                shuffleButtonIds[index] = index;

                var formationNames = new List<string>();
                foreach (var formationConfigData in _formationConfigDatas)
                {
                    formationNames.Add(formationConfigData.FormationName);
                }
                
                _playerView.SetDropDownButtons(index,formationNames);
            }
            
            _playerView.SetShuffleButtonsId(shuffleButtonIds);
        }

        private void OnFormationPressed(int armyId, int formationId)
        {
            var gameArmyConfigData = _gameConfigData.GameArmyConfigData[armyId];
            _setArmyPositionCommand.Execute(new SetArmyPositionCommandData
            {
                ArmyId = armyId,
                StartPosition = gameArmyConfigData.StartPosition,
                StartRotation = gameArmyConfigData.StartRotation,
                FormationConfigData = _formationConfigDatas[formationId]
            });
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

        private void OnArmyDestroyed()
        {
            if(_playerData.State.Value == SimulationState.GameSettings)
                return;
            
            _playerData.State.Value = SimulationState.GameSettings;
            _playerView.Show();
            Initialize();
        }

        public void Dispose()
        {
            _armiesData.OnArmyDestroyed -= OnArmyDestroyed;
            _playerView.OnPlayPressed -= OnPlayPressed;
            _playerView.OnShufflePressed -= OnShufflePressed;
        }
    }
}