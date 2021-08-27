using Core.Controller;
using Core.Model;
using Core.View.Unit;
using Zenject;

namespace Core.Pool
{
    public class UnitControllerPool : MemoryPool<UnitData,UnitController>
    {
        [Inject]
        private UnitViewPool _unitViewPool;
            
        protected override void OnCreated(UnitController item)
        {
            base.OnCreated(item);
            Container.Resolve<TickableManager>().AddFixed(item);
            Container.Inject(item);
        }

        protected override void Reinitialize(UnitData unitData, UnitController item)
        {
            base.Reinitialize(unitData, item);
            var unitView = _unitViewPool.Spawn();
            item.Initialize(unitView,unitData);
        }

        protected override void OnDespawned(UnitController item)
        {
            base.OnDespawned(item);
            item.Dispose();
            _unitViewPool.Despawn(item.UnitView as UnitView);
        }

        protected override void OnDestroyed(UnitController item)
        {
            base.OnDestroyed(item);
            Container.Resolve<TickableManager>().RemoveFixed(item);
        }
    }
}