using UnityEngine;

namespace GridSystem
{
    public class Snappable : MonoBehaviour
    {
        [SerializeField] private float xAxisValue;

        public float GetAxisPos()
        {
            return xAxisValue;
        }
    }
}
