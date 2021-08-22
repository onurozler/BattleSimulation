using UnityEngine;

namespace Core.View.Unit
{
    public interface IUnitView
    {
        void SetMesh(Mesh mesh);
        void SetColor(Color color);
        void SetSize(float size);
        void SetPosition(Vector3 position);
    }
}