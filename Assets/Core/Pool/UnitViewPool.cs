using Core.View.Unit;
using Zenject;

namespace Core.Pool
{
    public class UnitViewPool : MonoMemoryPool<UnitView>
    {
        protected override void OnSpawned(UnitView item)
        {
            base.OnSpawned(item);
            item.Rigidbody.isKinematic = false;
        }

        protected override void OnDespawned(UnitView item)
        {
            base.OnDespawned(item);
            item.Rigidbody.isKinematic = true;
        }
    }
}