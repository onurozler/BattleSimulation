using System;
using Core.Model;
using Core.View.Camera;
using Zenject;

namespace Core.Controller
{
    public class CameraController :  ITickable, IDisposable
    {
        private ICameraView _cameraView;
        private CameraData _cameraData;
        private ArmiesData _armiesData;
        private PlayerData _playerData;

        [Inject]
        private void Initialize(CameraData cameraData, ICameraView cameraView,PlayerData playerData, ArmiesData armiesData)
        {
            _cameraView = cameraView;
            _cameraData = cameraData;
            _playerData = playerData;
            _armiesData = armiesData;
            
            _playerData.State.Subscribe(OnSimulationStateChanged);
        }

        private void OnSimulationStateChanged(SimulationState simulationState)
        {
            if (simulationState == SimulationState.Playing)
            {
                UpdateCameraPosition();
                _cameraView.SetOffset(_cameraData.Position);
            }
            else
            {
                _cameraView.ResetPosition();
            }
        }

        public void Tick()
        {
            if (_playerData.State.Value == SimulationState.GameSettings)
                return;
            
            UpdateCameraPosition();
            _cameraView.UpdatePosition(_cameraData.Position, _cameraData.Speed);
        }

        private void UpdateCameraPosition()
        {
            _cameraData.ResetPosition();
            foreach (var unitData in _armiesData.GetAllUnits())
            {
                _cameraData.Position += unitData.CurrentPosition;
            }

            _cameraData.Position /= _armiesData.GetAllUnits().Count;
        }

        public void Dispose()
        {
            _playerData.State.Unsubscribe(OnSimulationStateChanged);
        }
    }
}