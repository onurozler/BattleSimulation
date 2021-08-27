using Core.Util.Timing;
using UnityEngine;
using Zenject;

namespace Core.Pool
{
    public class UnitAttackParticlePool : MonoMemoryPool<Vector3,ParticleSystem>
    {
        [Inject] 
        private ITimingManager _timingManager;
        
        protected override void Reinitialize(Vector3 position, ParticleSystem item)
        {
            base.Reinitialize(position, item);
            item.gameObject.transform.position = position;
        }

        protected override void OnSpawned(ParticleSystem item)
        {
            base.OnSpawned(item);
            _timingManager.SetInterval(0.5f, () =>
            {
                Despawn(item);
            });
        }
    }
}