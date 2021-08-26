using Core.Model;
using UnityEngine;
using Zenject;

namespace Core.Controller
{
    public class CameraController :  ITickable
    {
        [Inject] 
        private Camera _mainCamera;

        [Inject] 
        private ArmiesData _armiesData;

        private Vector3 _offset;
        
        private PlayerData _playerData;

        [Inject]
        private void Initialize(PlayerData playerData)
        {
            _playerData = playerData;
            _playerData.State.Subscribe(OnSimulationStateChanged);
        }

        private void OnSimulationStateChanged(SimulationState simulationState)
        {
            if (simulationState == SimulationState.Playing)
            {
                _offset = _mainCamera.transform.position - GetMiddlePosition();
            }
        }

        public void Tick()
        {
            if (_playerData.State.Value != SimulationState.Playing)
                return;
            
            //_mainCamera.transform.position = _offset + GetMiddlePosition();
        }

        private Vector3 GetMiddlePosition()
        {
            var averagePosition = Vector3.zero;
            foreach (var unitData in _armiesData.GetAllUnits())
            {
                averagePosition += unitData.CurrentPosition;
            }

            averagePosition /= _armiesData.GetAllUnits().Count;
            return averagePosition;
        }
    }
}