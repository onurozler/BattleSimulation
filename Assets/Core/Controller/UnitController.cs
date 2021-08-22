using System;
using Core.Model;
using Core.View.Unit;
using Zenject;

namespace Core.Controller
{
    public class UnitController : ITickable,IDisposable
    {
        private IUnitView _unitView;
        private UnitData _unitData;

        private void Initialize()
        {
            _unitData.Size.Subscribe(_unitView.SetSize,true);
            _unitData.Mesh.Subscribe(_unitView.SetMesh,true);
            _unitData.Color.Subscribe(_unitView.SetColor,true);
            _unitData.Position.Subscribe(_unitView.SetPosition);
        }

        public void Tick()
        {
        }
        
        public void Dispose()
        {
            _unitData.Size.Unsubscribe(_unitView.SetSize);
            _unitData.Mesh.Unsubscribe(_unitView.SetMesh);
            _unitData.Color.Unsubscribe(_unitView.SetColor);
            _unitData.Position.Unsubscribe(_unitView.SetPosition);
        }

        public class Pool : MemoryPool<UnitData,UnitController>
        {
            [Inject]
            private UnitView.Pool _unitViewPool;

            protected override void OnCreated(UnitController item)
            {
                base.OnCreated(item);
                
                Container.Resolve<DisposableManager>().Add(item);
                Container.Resolve<TickableManager>().Add(item);

                item._unitView = _unitViewPool.Spawn();
            }

            protected override void Reinitialize(UnitData unitData, UnitController item)
            {
                base.Reinitialize(unitData, item);
                item._unitData = unitData;
                item.Initialize();
            }

            protected override void OnDespawned(UnitController item)
            {
                base.OnDespawned(item);
                item.Dispose();
            }
        }

    }
}