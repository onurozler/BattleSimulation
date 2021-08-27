using Core.Model.Config;
using UnityEngine;

namespace Core.Model
{
    public class CameraData
    {
        public Vector3 Position { get; set; }
        public float Speed { get; }

        public CameraData()
        {
            Speed = Constants.CameraSpeed;
            ResetPosition();
        }
        
        public void ResetPosition()
        {
            Position = Vector3.zero;
        }
    }
}