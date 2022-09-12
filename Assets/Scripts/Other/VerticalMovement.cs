using UnityEngine;

namespace Other
{
    public class VerticalMovement : MonoBehaviour
    {
        public bool vert;
        private Vector3 to;
    
        public void Move(bool val)
        {
            if (vert)
            {
                to = val ? Vector3.forward : Vector3.back;   
            }
            else
            {
                to = val ? Vector3.up : Vector3.down;   
            }
        
            transform.Translate(to/3f);
        }
    }
}
