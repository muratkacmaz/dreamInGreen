using Other;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoverButton : MonoBehaviour, IPointerDownHandler
{
    public VerticalMovement _mover;
    public bool Rot;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _mover.Move(Rot);
    }
}
