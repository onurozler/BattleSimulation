using UnityEngine;
using UnityCamera = UnityEngine.Camera;
namespace Core.View.Camera
{
    public class CameraView : MonoBehaviour, ICameraView
    {
        private UnityCamera MainCamera => _cachedCamera ? _cachedCamera : _cachedCamera =UnityCamera.main;
        private UnityCamera _cachedCamera;
        private Vector3 _offset;
        private Vector3 _firstPosition;
        private Quaternion _firstRotation;

        private void Awake()
        {
            _firstPosition = MainCamera.transform.position;
            _firstRotation = MainCamera.transform.rotation;
        }

        public void SetOffset(Vector3 middlePosition)
        {
            _offset = MainCamera.transform.position - middlePosition;
        }
        
        public void UpdatePosition(Vector3 middlePosition, float speed)
        {
            var lookRotation = middlePosition - MainCamera.transform.position;
            MainCamera.transform.position = Vector3.MoveTowards(MainCamera.transform.position, _offset - middlePosition, Time.deltaTime * speed);
            MainCamera.transform.rotation = Quaternion.RotateTowards(MainCamera.transform.rotation, Quaternion.LookRotation(lookRotation),
                    Time.deltaTime * speed);
        }

        public void ResetPosition()
        {
            MainCamera.transform.position = _firstPosition;
            MainCamera.transform.rotation = _firstRotation;
        }
    }
}
