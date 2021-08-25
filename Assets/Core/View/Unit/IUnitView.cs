using UnityEngine;

namespace Core.View.Unit
{
    public interface IUnitView : ICollidableView
    {
        Vector3 Position { get; }
        void SetMesh(Mesh mesh);
        void SetColor(Color color);
        void SetSize(float size);
        void SetInitialPosition(Vector3 position);
        void SetInitialRotation(Vector3 rotation);
        void UpdatePosition(float speed, Vector3 rotation);
    }
}