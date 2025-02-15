﻿using System.Collections;
using CCL_GameScripts.Attributes;
using UnityEngine;

namespace CCL_GameScripts.CabControls
{
    public class PullerSetup : ControlSetupBase, IApertureTrackable
    {
        public override string TargetTypeName => "DV.CabControls.Spec.Puller";
        public override bool DestroyAfterCreation => false;
        public override CabControlType ControlType => CabControlType.Puller;

        [Header("Basic Settings")]
        [ProxyField("useSteppedPuller")]
        public bool useNotches = false;
        [ProxyField]
        public int notches = 20;
        [ProxyField]
        public bool invertDirection = false;
        [ProxyField("scrollWheelHoverScroll")]
        public float scrollWheelDelta = 0.025f;
        [ProxyField]
        public float linearLimit = 0.3f;

        public bool TrackAsWindow = false;
        public bool TrackAsAperture => TrackAsWindow;

        [Header("Physics")]
        [ProxyField]
        public float rigidBodyMass = 5f;
        [ProxyField]
        public float rigidBodyDrag = 15f;

        // TODO: Custom connection anchor?
        [ProxyField]
        public Transform pivot;

        [ProxyComponent("nonVrStaticInteractionArea", "StaticInteractionArea")]
        public GameObject StaticInteractionArea = null;

        private void OnDrawGizmos()
        {
            Vector3 movedOffset = transform.TransformPoint(Vector3.up * linearLimit);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, movedOffset);
        }
    }
}