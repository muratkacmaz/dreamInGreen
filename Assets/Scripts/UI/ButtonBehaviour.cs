using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ButtonBehaviour : MonoBehaviour, IPointerDownHandler
    {
        private void Enablee()
        {
            ARPlacementInteractableSingle.SetEnable();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Invoke(nameof(Enablee),.2f );
        }
    }
}
