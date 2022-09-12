using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace AR
{
    public class ARSelectionInteractableElected : ARSelectionInteractable
    { 
        bool m_GestureSelected;

        /// <inheritdoc />
        public override bool IsSelectableBy(IXRSelectInteractor interactor) => interactor is ARGestureInteractor && m_GestureSelected;

        /// <inheritdoc />
        protected override bool CanStartManipulationForGesture(TapGesture gesture) => true;

        /// <inheritdoc />
        protected override void OnEndManipulation(TapGesture gesture)
        {
            base.OnEndManipulation(gesture);

            if (gesture.isCanceled)
                return;
            if (gestureInteractor == null)
                return;

            if (gesture.targetObject == gameObject)
            {
                // Toggle selection
                m_GestureSelected = !m_GestureSelected;
            }
            else
            {
                m_GestureSelected = false;
            }
            /*var ob = gesture.targetObject.GetComponent<ARSelectionInteractableElected>();
            
            if(ob == null)
                return;*/
        }

        public void SetSelectedOn()
        {
            m_GestureSelected = true;
        }
    }
}
