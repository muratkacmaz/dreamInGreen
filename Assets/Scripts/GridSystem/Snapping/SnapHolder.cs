using System;
using System.Collections.Generic;
using AR;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GridSystem
{
    public class SnapHolder : MonoBehaviour
    {
        [SerializeField] private float offset;
        [SerializeField] private GameObject leftSnapVisual;
        [SerializeField] private GameObject rightSnapVisual;
        [SerializeField] private TextMeshProUGUI debug;

        private static List<SnapHolder> _snappables = new List<SnapHolder>();
        private static int snapIndex;

        private Transform arSpawnable;
        private SnapHolder snapHolder;
        private BoxCollider snapCollider;
        
        private Vector3 defColSize;
        private bool isDragging;

        private bool leftPlaced;
        private bool rightPlaced;

        private void Awake()
        {
            arSpawnable = transform.parent.parent;
            snapCollider = GetComponent<BoxCollider>();
            defColSize = snapCollider.bounds.size;

            arSpawnable.GetComponent<ARTranslationInteractableNonRotator>().OnEndTranslation += ManipulateEnded;
            arSpawnable.GetComponent<ARTranslationInteractableNonRotator>().onStartTranslation += ManipulateStarted;
            
            _snappables.Add(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!isDragging)
                return;
            
            var holder = other.GetComponent<SnapHolder>();

            if (holder != null)
            {
                snapHolder = holder;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(!isDragging)
                return;
            
            var holder = other.GetComponent<SnapHolder>();

            if (holder != null)
            {
                if (holder == snapHolder) //is it the same snappable we quit out of zone
                {
                    snapHolder = null;
                }
                else // we might have entered another before quitting a snapppable
                {
                    
                }
            }
        }

        private void Snap()
        {
            if (snapHolder != null)
            {
                var fl = Vector3.SignedAngle(snapHolder.transform.parent.forward, arSpawnable.position, snapHolder.transform.parent.up);

                debug.text = fl.ToString();
                arSpawnable.rotation = snapHolder.arSpawnable.rotation;
                arSpawnable.localScale = snapHolder.arSpawnable.localScale;

                var newPos = Vector3.zero;
                var oldPar = arSpawnable.parent;
                arSpawnable.parent = snapHolder.arSpawnable;
                
                if (fl >= 0) //right side
                {
                    newPos.x += snapHolder.offset + offset;
                    snapHolder.rightPlaced = true;
                }
                else
                {
                    newPos.x -= snapHolder.offset + offset;
                    snapHolder.leftPlaced = true;
                }

                arSpawnable.DOLocalMove(newPos, 2).OnComplete(() =>
                {
                    arSpawnable.parent = oldPar;
                });
            }
        }

        private void OnDestroy()
        {
            _snappables.Remove(this);
        }

        private void ManipulateEnded()
        {
            EnableSnappables(false);
            isDragging = false;

            Invoke("Snap", 1);
        }

        private void ManipulateStarted(bool val)
        {
            isDragging = true;
            
            EnableSnappables(true);
        }

        private void EnableSnappables(bool val)
        {
            foreach (var snaps in _snappables)
            {
                if(snaps == this)
                    continue;
                
                snaps.SetSnapCollider(val);
            }
        }

        private void SetSnapCollider(bool val)
        {
            print("TRUE HAS CAME : " + val);
            var newSize = snapCollider.bounds.size;

            if (val)
            {
                newSize.x *= 2.5f;
                snapCollider.size = newSize;   
            }
            else
            {
                snapCollider.size = defColSize;
            }

            leftSnapVisual.SetActive(val);
            rightSnapVisual.SetActive(val);
        }
    }
}
