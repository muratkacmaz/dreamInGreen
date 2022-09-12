using System;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// UnityEvent that responds to changes of hover and selection by this interactor.
/// </summary>
[Serializable]
    public class ARObjectPlacementEvent : UnityEvent<ARPlacementInteractableSingle, GameObject> { }
