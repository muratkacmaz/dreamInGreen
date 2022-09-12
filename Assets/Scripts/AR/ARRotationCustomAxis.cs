using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace AR
{
    public class ARRotationCustomAxis : ARBaseGestureInteractable
    {
        [SerializeField] private bool yRot = true; //If false then it rotates at z axis
        
        [SerializeField, Tooltip("The rate at which Unity rotates the attached object with a drag gesture.")]
        float m_RotationRateDegreesDrag = 100f;

        /// <summary>
        /// The rate at which Unity rotates the attached object with a drag gesture.
        /// </summary>
        public float rotationRateDegreesDrag
        {
            get => m_RotationRateDegreesDrag;
            set => m_RotationRateDegreesDrag = value;
        }

        [SerializeField, Tooltip("The rate at which Unity rotates the attached object with a twist gesture.")]
        float m_RotationRateDegreesTwist = 2.5f;

        /// <summary>
        /// The rate at which Unity rotates the attached object with a twist gesture.
        /// </summary>
        public float rotationRateDegreesTwist
        {
            get => m_RotationRateDegreesTwist;
            set => m_RotationRateDegreesTwist = value;
        }

        protected override bool CanStartManipulationForGesture(DragGesture gesture)
        {
            return IsGameObjectSelected() && gesture.targetObject == null;
        }

        protected override bool CanStartManipulationForGesture(TwistGesture gesture)
        {
            return IsGameObjectSelected() && gesture.targetObject == null;
        }

        protected override void OnContinueManipulation(DragGesture gesture)
        {
            // ReSharper disable once LocalVariableHidesMember -- hide deprecated camera property
            var camera = xrOrigin != null
                ? xrOrigin.Camera
#pragma warning disable 618 // Calling deprecated property to help with backwards compatibility.
                : (arSessionOrigin != null ? arSessionOrigin.camera : Camera.main);
#pragma warning restore 618
            if (camera == null)
                return;

            var forward = camera.transform.forward;
            var worldToVerticalOrientedDevice = Quaternion.Inverse(Quaternion.LookRotation(forward, Vector3.up));
            var deviceToWorld = camera.transform.rotation;
            var rotatedDelta = worldToVerticalOrientedDevice * deviceToWorld * gesture.delta;

            var rotationAmount = -1f * (rotatedDelta.x / Screen.dpi) * m_RotationRateDegreesDrag;
            
            if(yRot)
                transform.Rotate(0f, rotationAmount, 0f);
            else
            {
                transform.Rotate(rotationAmount, 0f, 0f);
            }
        }

        protected override void OnContinueManipulation(TwistGesture gesture)
        {
            var rotationAmount = -gesture.deltaRotation * m_RotationRateDegreesTwist;
            
            if(yRot)
                transform.Rotate(0f, rotationAmount, 0f);
            else
            {
                transform.Rotate(0f, 0f, rotationAmount);
            }
        }

        public void SetAxis(bool val)
        {
            yRot = val;
            print("YROT : " + yRot);
        }
    }
}
