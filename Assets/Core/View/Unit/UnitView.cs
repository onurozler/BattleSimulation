using UnityEngine;
using Zenject;

namespace Core.View.Unit
{
    public class UnitView : MonoBehaviour, IUnitView
    {
        [SerializeField] 
        private MeshFilter meshFilter;
        
        [SerializeField] 
        private MeshRenderer meshRenderer;
        
        public void SetMesh(Mesh mesh)
        {
            meshFilter.mesh = mesh;
        }

        public void SetColor(Color color)
        {
            var materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetColor("_Color",color);
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        public void SetSize(float size)
        {
            transform.localScale = Vector3.one * size;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public class Pool : MonoMemoryPool<UnitView>{}
    }
}