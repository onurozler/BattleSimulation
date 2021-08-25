using System;
using System.Collections.Generic;
using Core.Command;
using Core.Model;
using Core.Model.Commands;
using Core.Model.Config;
using Core.Util.Timing;
using Core.View.Unit;
using Zenject;

namespace Core.Controller
{
    public class UnitController : IFixedTickable, IDisposable
    {
        [Inject]
        private PlayerData _playerData;

        [Inject]
        private ArmiesData _armiesData;

        [Inject] 
        private KillUnitCommand _killUnitCommand;

        [Inject] 
        private ITimingManager _timingManager;

        private IUnitView _unitView;
        private UnitData _unitData;
        private bool _canAttack = true;

        private void Initialize()
        {
            _unitData.Size.Subscribe(_unitView.SetSize,true);
            _unitData.Mesh.Subscribe(_unitView.SetMesh,true);
            _unitData.Color.Subscribe(_unitView.SetColor,true);
            _unitData.ArmyId.Subscribe(_unitView.SetCollisionId,true);
            _unitData.InitialPosition.Subscribe(_unitView.SetInitialPosition);
            _unitData.InitialRotation.Subscribe(_unitView.SetInitialRotation);
            _unitData.Health.Subscribe(OnUnitHealthChanged);
        }
        public void FixedTick()
        {
            if (_playerData.State.Value == SimulationState.Playing)
            {
                _unitData.CurrentPosition = _unitView.Position;
                var targetUnit = _armiesData.GetClosestEnemyUnit(_unitData.ArmyId.Value, _unitData.CurrentPosition);
                var direction = (targetUnit.CurrentPosition - _unitData.CurrentPosition).normalized;
                
                if (_unitView.TryRaycast(Constants.UnitLayer,out var collidableView))
                {
                    if (collidableView.CollisionId != _unitView.CollisionId)
                    {
                        if (_canAttack)
                        {
                            _canAttack = false;
                            _timingManager.SetInterval(_unitData.AttackSpeed, () => _canAttack = true);
                            targetUnit.Health.Value -= _unitData.Attack;
                        }
                    }
                }
                else
                {
                    _unitView.UpdatePosition(_unitData.Speed,direction);
                }
            }
        }

        private void OnUnitHealthChanged(int currentHealth)
        {
            if (currentHealth <= 0 && _playerData.State.Value == SimulationState.Playing)
            {
                _killUnitCommand.Execute(new KillUnitCommandData
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
        }

        #region Pool

        public class Pool : MemoryPool<UnitData,UnitController>
        {
            [Inject]
            private UnitView.Pool _unitViewPool;
            
            protected override void OnCreated(UnitController item)
            {
                base.OnCreated(item);
                Container.Inject(item);
            }

            protected override void Reinitialize(UnitData unitData, UnitController item)
            {
                base.Reinitialize(unitData, item);
                Container.Resolve<DisposableManager>().Add(item);
                Container.Resolve<TickableManager>().AddFixed(item);
                item._unitView = _unitViewPool.Spawn();
                item._unitData = unitData;
                item.Initialize();
            }

            protected override void OnDespawned(UnitController item)
            {
                base.OnDespawned(item);
                item.Dispose();
                item._unitData = null;
                _unitViewPool.Despawn(item._unitView as UnitView);
                
                Container.Resolve<DisposableManager>().Remove(item);
                Container.Resolve<TickableManager>().RemoveFixed(item);
            }
        }
        
        #endregion
    }
}