using Core.Model;
using Core.Model.Config;
using Core.Util.Timing;
using Core.View.Unit;
using Zenject;

namespace Core.Controller
{
    public class UnitController : IFixedTickable
    {
        [Inject]
        private PlayerData _playerData;

        [Inject]
        private ArmiesData _armiesData;

        [Inject] 
        private SignalBus _signalBus;

        [Inject] 
        private ITimingManager _timingManager;

        public IUnitView UnitView => _unitView;

        private IUnitView _unitView;
        private UnitData _unitData;
        private bool _isAttackCoolDownFinished;

        public void Initialize(UnitView unitView, UnitData unitData)
        {
            _unitView = unitView;
            _unitData = unitData;
            
            _isAttackCoolDownFinished = true;
            _unitData.Size.Subscribe(_unitView.SetSize,true);
            _unitData.Mesh.Subscribe(_unitView.SetMesh,true);
            _unitData.Color.Subscribe(_unitView.SetColor,true);
            _unitData.ArmyId.Subscribe(_unitView.SetCollisionId,true);
            _unitData.InitialPosition.Subscribe(_unitView.SetInitialPosition);
            _unitData.InitialRotation.Subscribe(_unitView.SetInitialRotation);
            _unitData.Health.Subscribe(OnUnitHealthChanged);
            _playerData.State.Subscribe(OnStateChanged);
        }
        public void FixedTick()
        {
            var canUpdateUnit = _playerData.State.Value == SimulationState.Playing && _unitData != null;
            if (canUpdateUnit)
            {
                _unitData.CurrentPosition = _unitView.Position;
                var targetUnit = _armiesData.GetClosestEnemyUnit(_unitData.ArmyId.Value, _unitData.CurrentPosition);
                var direction = (targetUnit.CurrentPosition - _unitData.CurrentPosition).normalized;
                
                if (_unitView.TryRaycast(Constants.UnitLayer,out var collidableView))
                {
                    var isAttackableUnit = collidableView.CollisionId != _unitView.CollisionId && _isAttackCoolDownFinished;
                    if (isAttackableUnit)
                    {
                        HandleAttack(targetUnit);
                    }
                }

                _unitView.UpdatePosition(_unitData.Speed,direction);
            }
        }

        private void HandleAttack(UnitData targetUnit)
        {
            _isAttackCoolDownFinished = false;
            _timingManager.SetInterval(_unitData.AttackSpeed, () => _isAttackCoolDownFinished = true);
            targetUnit.Health.Value -= _unitData.Attack;
        }

        private void OnUnitHealthChanged(int currentHealth)
        {
            if (currentHealth <= 0 && _playerData.State.Value == SimulationState.Playing)
            {
                _signalBus.Fire(new KillUnitSignal
                {
                    UnitController = this,
                    UnitData = _unitData
                });
            }
        }

        private void OnStateChanged(SimulationState state)
        {
            if (state == SimulationState.GameSettings)
            {
                _signalBus.Fire(new KillUnitSignal
                {
                    UnitController = this,
                    UnitData = _unitData
                });
            }
        }
        
        public void Dispose()
        {
            _unitData.Size.Unsubscribe(_unitView.SetSize);
            _unitData.Mesh.Unsubscribe(_unitView.SetMesh);
            _unitData.Color.Unsubscribe(_unitView.SetColor);
            _unitData.InitialPosition.Unsubscribe(_unitView.SetInitialPosition);
            _unitData.InitialRotation.Unsubscribe(_unitView.SetInitialRotation);
            _unitData.Health.Unsubscribe(OnUnitHealthChanged);
            _playerData.State.Unsubscribe(OnStateChanged);
            _unitData = null;
        }
    }
}