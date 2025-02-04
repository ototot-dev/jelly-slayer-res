using UnityEngine;

namespace PampelGames.BloodFactory.Demo
{
    public class DemoCameraController : MonoBehaviour
    {
        public Transform target;

        public float maxZoom = 10;


        public void ZoomCamera(float value)
        {
            value = 1f - value;
            var zoomLevel = Mathf.Lerp(0.01f, maxZoom, value);
            var direction = (transform.position - target.transform.position).normalized;
            transform.position = target.transform.position + direction * zoomLevel;
        }
        
        public void RotateAroundY(float value)
        {
            var angle = value * 360f;

            var distanceToTarget2D = Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                new Vector2(target.transform.position.x, target.transform.position.z));  
        
            var direction = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            var newPosition = target.transform.position + direction * distanceToTarget2D;
            newPosition.y = transform.position.y;
        
            transform.position = newPosition;
            transform.LookAt(target.transform.position);
        }
    }
}