using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem.ScalingWalls
{
    public class VisualScalable : MonoBehaviour
{
    [SerializeField] private ScalableTextureTile tile;
    
    private void OnTriggerExit(Collider other) {
        if(other.tag == "Mask") 
        {
            {
                tile.DeleteVisual(gameObject);
            }
        }
    }

    public void SetTile(ScalableTextureTile tile)
    {
        this.tile =tile;
    }
}
}
