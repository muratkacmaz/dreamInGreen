using System;
using UnityEngine;

namespace GridSystem
{
    public class Multiplier : MonoBehaviour
    {
        [SerializeField] private MultiplierController placement;
        [SerializeField] private bool align;
        [SerializeField] private bool plfzen = false;

        private void OnMouseUpAsButton() {
            FindObjectOfType<ARPlacementInteractableSingle>().SetSnappable(placement.transform, placement.GetOffSet(), align, plfzen);
        }
    }
}
