using UnityEngine;

namespace Misc
{
    public class BackgroundPanelRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction = Vector3.up;
        [SerializeField] private float   _speed     = 20;
    
    
        private void Update()
        {
            transform.Rotate(_direction, _speed * Time.deltaTime, Space.Self);
        }
    }
}
