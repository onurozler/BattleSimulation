using System;
using System.Collections.Generic;
using Core.Model;
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
        private readonly FormationConfigData[] _formationConfigDatas;
        private readonly SignalBus _signalBus;
        
        public PlayerController(GameConfigData gameConfigData, IPlayerView playerView, 
            PlayerData playerData, ArmiesData armiesData,FormationConfigData[] formationConfigDatas,
            SignalBus signalBus)
        {
            _gameConfigData = gameConfigData;
            _playerView = playerView;
            _playerData = playerData;
            _armiesData = armiesData;
            _formationConfigDatas = formationConfigDatas;
            _signalBus = signalBus;

            _armiesData.OnArmyDestroyed += OnArmyDestroyed;
            _playerView.OnPlayPressed += OnPlayPressed;
            _playerView.OnShufflePressed += OnShufflePressed;
            _playerView.OnFormationPressed += OnFormationPressed;
        }

        public void Initialize()
        {
            for (var index = 0; index < _gameConfigData.GameArmyConfigData.Length; index++)
            {
                var gameArmyConfigData = _gameConfigData.GameArmyConfigData[index];
                _signalBus.Fire(new CreateArmySignal
                {
                    ArmyId = index,
                    ArmySize = gameArmyConfigData.ArmySize,
                });
                _signalBus.Fire(new SetArmyPositionSignal
                {
                    ArmyId = index,
                    StartPosition = gameArmyConfigData.StartPosition,
                    StartRotation = gameArmyConfigData.StartRotation,
                    FormationConfigData = _formationConfigDatas[0]
                });
                
                var formationNames = new List<string>();
                foreach (var formationConfigData in _formationConfigDatas)
                {
                    formationNames.Add(formationConfigData.FormationName);
                }
                
                _playerView.SetDropDownButton(index,formationNames);
                _playerView.SetShuffleButton(index, index);
            }
        }

        private void OnFormationPressed(int armyId, int formationId)
        {
            var gameArmyConfigData = _gameConfigData.GameArmyConfigData[armyId];
            _signalBus.Fire(new SetArmyPositionSignal
            {
                ArmyId = armyId,
                StartPosition = gameArmyConfigData.StartPosition,
                StartRotation = gameArmyConfigData.StartRotation,
                FormationConfigData = _formationConfigDatas[formationId]
            });
        }

        private void OnShufflePressed(int armyId)
        {
            _signalBus.Fire(new ShuffleArmySignal {ArmyId = armyId});
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