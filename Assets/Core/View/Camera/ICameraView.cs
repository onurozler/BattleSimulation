using UnityEngine;

namespace Core.View.Camera
{
    public interface ICameraView
    {
        void SetOffset(Vector3 middlePosition);
        void UpdatePosition(Vector3 middlePosition, float speed);
        void ResetPosition();
    }
}