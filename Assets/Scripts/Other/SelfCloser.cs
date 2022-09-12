using UnityEngine;

namespace Other
{
    public class SelfCloser : MonoBehaviour
    {
        [SerializeField] private float closeTime = 2;
    
        private void OnEnable()
        {
            Invoke(nameof(Close), closeTime);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
