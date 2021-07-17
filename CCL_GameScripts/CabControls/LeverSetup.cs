﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CCL_GameScripts.CabControls
{
    public class LeverSetup : ControlSetupBase
    {
        protected override string TargetTypeName => "DV.CabControls.Spec.Lever";
        protected override bool DestroyAfterCreation => true;
        public override CabControlType ControlType => CabControlType.Lever;

		[Header("Lever")]
		[ProxyField("useSteppedJoint")]
		public bool UseNotches = true; // useSteppedJoint
		[ProxyField("notches")]
		public int Notches = 20;
		[ProxyField("invertDirection")]
		public bool InvertDirection = false;

		[Header("Hinge Joint")]
		[ProxyField("jointAxis")]
		public Vector3 JointAxis = Vector3.up;
		//public bool UseLimits = true;
		[ProxyField("jointLimitMin")]
		public float JointLimitMin = 0;
		[ProxyField("jointLimitMax")]
		public float JointLimitMax = 90;
		//public bool UseSpring = true;
		[ProxyField("jointSpring")]
		public float JointSpring = 100f;
		[ProxyField("jointDamper")]
		public float JointDamper = 10;

		[Header("Rigidbody")]
		[ProxyField("rigidbodyMass")]
		public float RigidbodyMass = 30;
		[ProxyField("rigidbodyDrag")]
		public float RigidbodyDrag = 8;
		//public float RigidbodyAngularDrag = 0;

		[Header("NonVR")]
		[ProxyField("scrollWheelHoverScroll")]
		public float HoverScrollMagnitude = 4;
		[ProxyField("scrollWheelSpring")]
		public float ScrollWheelSpring = 50;
		[ProxyComponent("nonVrStaticInteractionArea", "StaticInteractionArea")]
		public GameObject StaticInteractionArea = null;

		[Header("VR")]
		[ProxyField("maxForceAppliedMagnitude")]
		public float MaxAppliedForce = float.PositiveInfinity;
		[ProxyField("pullingForceMultiplier")]
		public float PullingForceMultiplier = 1;

		[InspectorName("Interaction Point (optional)")]
		[ProxyField("interactionPoint")]
		public Transform InteractionPoint = null;
		[ProxyField("limitVibration")]
		public bool VibrateAtLimit = false;

		// TODO: Audio

		protected const float GIZMO_RADIUS = 0.1f;
		protected const int GIZMO_SEGMENTS = 40;
		protected static readonly Color START_COLOR = new Color(0, 0, 0.65f);
		protected static readonly Color END_COLOR = new Color(0, 0.65f, 0);

		private void OnDrawGizmosSelected()
        {
			Color startColor = InvertDirection ? END_COLOR : START_COLOR;
			Color endColor = InvertDirection ? START_COLOR : END_COLOR;

			if( UseNotches )
			{
				// draw ray segments
				for( int i = 0; i <= Notches; i++ )
				{
					Color segmentColor = Color.Lerp(startColor, endColor, (float)i / Notches);
					Vector3 rayVector = Quaternion.AngleAxis(
						Mathf.Lerp(JointLimitMin, JointLimitMax, (float)i / Notches), JointAxis)
						* Vector3.forward * GIZMO_RADIUS;
					rayVector = transform.TransformPoint(rayVector);

					Debug.DrawLine(transform.position, rayVector, segmentColor, 0, false);
				}
			}
			else
            {
				// draw semi-circle
				Vector3 lastVector = transform.parent.position;
				for( int i = 0; i <= GIZMO_SEGMENTS; i++ )
				{
					Color segmentColor = Color.Lerp(startColor, endColor, (float)i / Notches);
					Vector3 nextVector = Quaternion.AngleAxis(
						Mathf.Lerp(JointLimitMin, JointLimitMax, (float)i / GIZMO_SEGMENTS), JointAxis)
						* Vector3.forward * GIZMO_RADIUS;
					nextVector = transform.TransformPoint(nextVector);

					if( i == 0 || i == GIZMO_SEGMENTS )
                    {
						Debug.DrawLine(transform.position, nextVector, segmentColor, 0, false);
					}
					else if( i != 0 )
                    {
						Debug.DrawLine(lastVector, nextVector, segmentColor, 0f, false);
					}

					lastVector = nextVector;
				}
			}
        }

		[MethodButton(
			nameof(IndependentBrakeDefaults),
			nameof(TrainBrakeDefaults),
			nameof(DieselThrottleDefaults),
			nameof(DieselReverserDefaults))]
		[SerializeField]
		private bool editorFoldout;

		public void IndependentBrakeDefaults()
		{
			UseNotches = true;
			Notches = 20;
			InvertDirection = false;

			JointAxis = Vector3.up;
			JointLimitMin = -61;
			JointLimitMax = 0;
			JointSpring = 100f;
			JointDamper = 10;

			RigidbodyMass = 30;
			RigidbodyDrag = 8;

			HoverScrollMagnitude = 4;
			ScrollWheelSpring = 50;

			MaxAppliedForce = float.PositiveInfinity;
			PullingForceMultiplier = 1;
			VibrateAtLimit = false;
		}

		public void TrainBrakeDefaults()
        {
			UseNotches = true;
			Notches = 20;
			InvertDirection = false;

			JointAxis = Vector3.up;
			JointLimitMin = 0;
			JointLimitMax = 72;
			JointSpring = 100f;
			JointDamper = 10;

			RigidbodyMass = 30;
			RigidbodyDrag = 8;

			HoverScrollMagnitude = 4;
			ScrollWheelSpring = 50;

			MaxAppliedForce = float.PositiveInfinity;
			PullingForceMultiplier = 1;
			VibrateAtLimit = false;
		}

		public void DieselThrottleDefaults()
        {
			UseNotches = true;
			Notches = 8;
			InvertDirection = false;

			JointAxis = Vector3.up;
			JointLimitMin = -52;
			JointLimitMax = 0;
			JointSpring = 100f;
			JointDamper = 0;

			RigidbodyMass = 30;
			RigidbodyDrag = 16;

			HoverScrollMagnitude = 1;
			ScrollWheelSpring = 100;

			MaxAppliedForce = float.PositiveInfinity;
			PullingForceMultiplier = 1;
			VibrateAtLimit = false;
		}

		public void DieselReverserDefaults()
        {
			UseNotches = true;
			Notches = 3;
			InvertDirection = false;

			JointAxis = Vector3.up;
			JointLimitMin = -25;
			JointLimitMax = 65;
			JointSpring = 25;
			JointDamper = 2;

			RigidbodyMass = 30;
			RigidbodyDrag = 0;

			HoverScrollMagnitude = 1;
			ScrollWheelSpring = 8;

			MaxAppliedForce = float.PositiveInfinity;
			PullingForceMultiplier = 1;
			VibrateAtLimit = false;
		}
	}
}
