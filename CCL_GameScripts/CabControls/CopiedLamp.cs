﻿using System.Collections;
using UnityEngine;

namespace CCL_GameScripts.CabControls
{
    public class CopiedLamp : CopiedCabDevice
    {
        public enum CopiedLampType
        {
            SmallRed,
            SmallBlue,
            SmallYellow,
            LargeRed,
            LargeGreen,

            SmallFlatRed,
            SmallFlatBlue,
            SmallFlatYellow,
            LargeFlatRed,
            TinyBlue,
        }

        protected static readonly (BaseTrainCarType, string)[] TargetObjects =
            new[]
            {
                (BaseTrainCarType.LocoShunter, "I brake_aux_res_lamp"),
                (BaseTrainCarType.LocoShunter, "C dashboard buttons controller/I fan_lamp"),
                (BaseTrainCarType.LocoShunter, "C dashboard buttons controller/I deploy_sand_lamp"),
                (BaseTrainCarType.LocoShunter, "C dashboard indicators controller/I service_engine_lamp"),
                (BaseTrainCarType.LocoShunter, "C dashboard buttons controller/I power_fuse_lamp"),

                (BaseTrainCarType.LocoDiesel, "offset/I Indicator lamps/I brake_aux_lamp"),
                (BaseTrainCarType.LocoDiesel, "offset/I Indicator lamps/I headlights_lamp"),
                (BaseTrainCarType.LocoDiesel, "offset/I Indicator lamps/I button_sand_lamp"),
                (BaseTrainCarType.LocoDiesel, "offset/I Indicator lamps/I service_engine_lamp"),
                (BaseTrainCarType.LocoDiesel, "offset/I Indicator lamps/I fan_lamp"),
            };

        protected static readonly LampGizmoInfo[] GizmoData =
            new[]
            {
                // DE2
                new LampGizmoInfo(0.011f, 0.01f, new Color(0.6f, 0, 0)),    // s red
                new LampGizmoInfo(0.01f, 0.01f, new Color(0, 0.5f, 0.6f)),  // s blue
                new LampGizmoInfo(0.009f, 0.01f, new Color(0.6f, 0.6f, 0)), // s yellow
                new LampGizmoInfo(0.02f, 0.02f, new Color(0.6f, 0, 0)),     // L red
                new LampGizmoInfo(0.02f, 0.02f, new Color(0, 0.6f, 0)),     // L green

                // DE6
                new LampGizmoInfo(0.0095f, 0.002f, new Color(0.6f, 0, 0), true),       // s red
                new LampGizmoInfo(0.0095f, 0.002f, new Color(0, 0.5f, 0.6f), true),    // s blue
                new LampGizmoInfo(0.0095f, 0.002f, new Color(0.6f, 0.6f, 0), true),    // s yellow
                new LampGizmoInfo(0.0175f, 0.012f, new Color(0.6f, 0, 0), true),       // L red
                new LampGizmoInfo(0.0073f, 0.009f, new Color(0, 0.5f, 0.6f), true),    // t blue
            };


        public SimEventType SimBinding;
        public CopiedLampType LampType;

        [Header("Simulation Binding")]
        public bool UseGaugeValue = false;
        public SimThresholdDirection ThresholdDirection;
        public SimAmount SolidThreshold;
        public bool UseBlinkMode;
        public SimAmount BlinkThreshold;

        public override (BaseTrainCarType, string) GetSourceObject()
        {
            return TargetObjects[(int)LampType];
        }

        public CopiedLamp()
        {
            ReplaceThisObject = true;
        }

        protected const int GIZMO_SEGMENTS = 40;
        protected const int GIZMO_RADIAL_DIVISOR = 2;

        private void OnDrawGizmos()
        {
            LampGizmoInfo gizmo = GizmoData[(int)LampType];

            Vector3 axis = gizmo.Flipped ? Vector3.back : Vector3.forward;
            if (LampType == CopiedLampType.LargeRed)
            {
                // rotate gizmo 90 around X axis for this lamp
                axis = Vector3.up;
            }

            Vector3 peak = transform.TransformPoint(axis * gizmo.Depth);

            Vector3 lastVector = transform.position;
            for( int i = 0; i <= GIZMO_SEGMENTS; i++ )
            {
                Gizmos.color = gizmo.Color;

                Vector3 radialVector = (Quaternion.AngleAxis(
                    Mathf.Lerp(0, 360, (float)i / GIZMO_SEGMENTS), axis)
                    * Vector3.right).normalized;
                Vector3 nextVector = transform.TransformPoint(radialVector * gizmo.Radius);

                if( (i % GIZMO_RADIAL_DIVISOR) == 0 )
                {
                    Vector3 midPt1 = transform.TransformPoint(
                        radialVector * Mathf.Cos(Mathf.PI / 6f) * gizmo.Radius +
                        axis * Mathf.Sin(Mathf.PI / 6f) * gizmo.Depth);

                    Vector3 midPt2 = transform.TransformPoint(
                        radialVector * Mathf.Cos(Mathf.PI / 3f) * gizmo.Radius +
                        axis * Mathf.Sin(Mathf.PI / 3f) * gizmo.Depth);

                    Gizmos.DrawLine(peak, midPt2);
                    Gizmos.DrawLine(midPt2, midPt1);
                    Gizmos.DrawLine(midPt1, nextVector);
                }

                if( i != 0 )
                {
                    Gizmos.DrawLine(lastVector, nextVector);
                }

                lastVector = nextVector;
            }
        }

        protected class LampGizmoInfo
        {
            public float Radius;
            public float Depth;
            public Color Color;
            public bool Flipped;

            public LampGizmoInfo( float radius, float depth, Color color, bool flipped = false )
            {
                Radius = radius;
                Depth = depth;
                Color = color;
                Flipped = flipped;
            }
        }
    }
}