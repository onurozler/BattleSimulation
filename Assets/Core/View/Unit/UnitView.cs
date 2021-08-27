using UnityEngine;

namespace Core.View.Unit
{
    public class UnitView : MonoBehaviour, IUnitView
    {
        [SerializeField] 
        private MeshFilter meshFilter;
        
        [SerializeField] 
        private MeshRenderer meshRenderer;

        [SerializeField] 
        private Rigidbody rigidBody;

        public Rigidbody Rigidbody => rigidBody;
        public int CollisionId { get; private set; }
        public Vector3 Position => transform.position;

        public void SetColor(Color color)
        {
            var materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetColor("_Color",color);
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }
        
        public void SetCollisionId(int id) => CollisionId = id;
        public void SetMesh(Mesh mesh) => meshFilter.mesh = mesh;
        public void SetSize(float size) => transform.localScale = Vector3.one * size;
        public void SetInitialPosition(Vector3 position) => transform.position = position;
        public void SetInitialRotation(Vector3 rotation) => transform.eulerAngles = rotation;
        public void UpdatePosition(float speed,Vector3 rotation)
        {
            rigidBody.velocity = Vector3.zero;
            if (rotation != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation), Time.deltaTime * speed);
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime,Space.Self);
        }

        public bool TryRaycast(int layer, out ICollidableView collidableView)
        {
            if (Physics.Raycast(transform.position,transform.forward, out var hitInfo, transform.localScale.z , layer))
            {
                if (hitInfo.collider.TryGetComponent(out ICollidableView collidable))
                {
                    collidableView = collidable;
                    return true;
                }
            }

            collidableView = null;
            return false;
        }
    }
}