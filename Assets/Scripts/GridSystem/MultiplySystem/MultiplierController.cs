using UnityEngine;

namespace GridSystem
{
    public class MultiplierController : MonoBehaviour
    {
        [SerializeField] private float offset;
        [SerializeField] private GameObject multipers;

        public void SetActive(bool val)
        {
            multipers.SetActive(val);
        }

        public float GetOffSet()
        {
            return offset;
        }
    }
}
