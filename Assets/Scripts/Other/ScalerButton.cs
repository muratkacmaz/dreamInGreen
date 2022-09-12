using System;
using System.Collections.Generic;
using GridSystem;
using GridSystem.ScalingWalls;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Other
{
    public class ScalerButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        [SerializeField] private ModelViewer modelViewer;
        [SerializeField] private ScalableTextureTile.TilerAction actionType;

        private Action buttonAction;
        private ScalableTextureTile tile;
        bool ispressed = false;
        
        private void Awake()
        {
            if (tile != null) return;
            
            
            tile = modelViewer.GetTiler();
            buttonAction = actionType switch
            {
                ScalableTextureTile.TilerAction.Up => tile.OnUp,
                ScalableTextureTile.TilerAction.Right => tile.OnRight,
                ScalableTextureTile.TilerAction.Left => tile.OnLeft,
                ScalableTextureTile.TilerAction.Down => tile.OnDown,
                _ => buttonAction
            };
        }

        private void Update()
        {
            if (!ispressed)
                return;

            buttonAction.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ispressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ispressed = false;
        }
    }
}
