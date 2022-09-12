using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace AR
{
    public class ARTranslationInteractableNonRotator : ARBaseGestureInteractable
    {
        private Action onEndTranslation;

        public delegate void OnStartTranslation(bool val);

        public OnStartTranslation onStartTranslation;

        public Action OnEndTranslation
        {
            get => onEndTranslation;
            set => onEndTranslation += value;
        }

        [SerializeField]
        GestureTransformationUtility.GestureTranslationMode m_ObjectGestureTranslationMode;

        /// <summary>
        /// Controls whether the object will be constrained vertically, horizontally, or free to move in all axis.
        /// </summary>
        public GestureTransformationUtility.GestureTranslationMode objectGestureTranslationMode
        {
            get => m_ObjectGestureTranslationMode;
            set => m_ObjectGestureTranslationMode = value;
        }

        [SerializeField]
        [Tooltip("The maximum translation distance of this object.")]
        float m_MaxTranslationDistance = 10f;

        /// <summary>
        /// The maximum translation distance of this object.
        /// </summary>
        public float maxTranslationDistance
        {
            get => m_MaxTranslationDistance;
            set => m_MaxTranslationDistance = value;
        }

        [SerializeField]
        [Tooltip("The LayerMask that Unity uses during an additional ray cast when a user touch does not hit any AR trackable planes.")]
        LayerMask m_FallbackLayerMask;

        /// <summary>
        /// The <see cref="LayerMask"/> that Unity uses during an additional ray cast
        /// when a user touch does not hit any AR trackable planes.
        /// </summary>
        public LayerMask fallbackLayerMask
        {
            get => m_FallbackLayerMask;
            set => m_FallbackLayerMask = value;
        }

        const float k_PositionSpeed = 12f;
        const float k_DiffThreshold = 0.0001f;

        bool m_IsActive;

        UnityEngine.Vector3 m_DesiredLocalPosition;
        float m_GroundingPlaneHeight;
        UnityEngine.Vector3 m_DesiredAnchorPosition;
        UnityEngine.Quaternion m_DesiredRotation;
        GestureTransformationUtility.Placement m_LastPlacement;
        
        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
                UpdatePosition();
        }

        protected override bool CanStartManipulationForGesture(DragGesture gesture)
        {
            return gesture.targetObject != null && gesture.targetObject == gameObject && transform.parent != null;
        }

        protected override void OnStartManipulation(DragGesture gesture)
        {
            m_GroundingPlaneHeight = transform.parent.position.y;
            onStartTranslation?.Invoke(true);
        }

        protected override void OnContinueManipulation(DragGesture gesture)
        {
            if (transform.parent == null)
            {
                Debug.LogError("Translation Interactable needs a parent object.", this);
                return;
            }

            m_IsActive = true;

            var desiredPlacement = xrOrigin != null
                ? GestureTransformationUtility.GetBestPlacementPosition(
                    transform.parent.position, gesture.position, m_GroundingPlaneHeight, 0.03f,
                    maxTranslationDistance, objectGestureTranslationMode, xrOrigin, fallbackLayerMask: m_FallbackLayerMask)
#pragma warning disable 618 // Calling deprecated method to help with backwards compatibility.
                : GestureTransformationUtility.GetBestPlacementPosition(
                    transform.parent.position, gesture.position, m_GroundingPlaneHeight, 0.03f,
                    maxTranslationDistance, objectGestureTranslationMode, arSessionOrigin, fallbackLayerMask: m_FallbackLayerMask);
#pragma warning restore 618

            if (desiredPlacement.hasHoveringPosition && desiredPlacement.hasPlacementPosition)
            {
                // If desired position is lower than current position, don't drop it until it's finished.
                m_DesiredLocalPosition = transform.parent.InverseTransformPoint(desiredPlacement.hoveringPosition);
                m_DesiredAnchorPosition = desiredPlacement.placementPosition;

                m_GroundingPlaneHeight = desiredPlacement.updatedGroundingPlaneHeight;

                // Rotate if the plane direction has changed.
                if (((desiredPlacement.placementRotation * Vector3.up) - transform.up).magnitude > k_DiffThreshold)
                    m_DesiredRotation = desiredPlacement.placementRotation;
                else
                    m_DesiredRotation = transform.rotation;

                if (desiredPlacement.hasPlane)
                    m_LastPlacement = desiredPlacement;
            }
        }

        protected override void OnEndManipulation(DragGesture gesture)
        {
            if (!m_LastPlacement.hasPlacementPosition)
                return;

            var oldAnchor = transform.parent.gameObject;
            var desiredPose = new Pose(m_DesiredAnchorPosition, m_LastPlacement.placementRotation);

            var desiredLocalPosition = transform.parent.InverseTransformPoint(desiredPose.position);

            if (desiredLocalPosition.magnitude > maxTranslationDistance)
                desiredLocalPosition = desiredLocalPosition.normalized * maxTranslationDistance;
            desiredPose.position = transform.parent.TransformPoint(desiredLocalPosition);

            var anchor = new GameObject("PlacementAnchor").transform;
            anchor.position = m_LastPlacement.placementPosition;
            anchor.rotation = m_LastPlacement.placementRotation;
            transform.parent = anchor;

            Destroy(oldAnchor);

            m_DesiredLocalPosition = Vector3.zero;

            // Make sure position is updated one last time.
            m_IsActive = true;
            
            print("Ended Translation");
            onEndTranslation?.Invoke();
        }


        private void UpdatePosition()
        {
            if (!m_IsActive)
                return;

            // Lerp position.
            Vector3 oldLocalPosition = transform.localPosition;
            var newLocalPosition = Vector3.Lerp(
                oldLocalPosition, m_DesiredLocalPosition, Time.deltaTime * k_PositionSpeed);

            var diffLength = (m_DesiredLocalPosition - newLocalPosition).magnitude;
            if (diffLength < k_DiffThreshold)
            {
                newLocalPosition = m_DesiredLocalPosition;
                m_IsActive = false;
            }

            transform.localPosition = newLocalPosition;

            /*// Lerp rotation.
            var oldRotation = transform.rotation;
            var newRotation =
                Quaternion.Lerp(oldRotation, m_DesiredRotation, Time.deltaTime * k_PositionSpeed);
            transform.rotation = newRotation;*/
        }
    }
}
