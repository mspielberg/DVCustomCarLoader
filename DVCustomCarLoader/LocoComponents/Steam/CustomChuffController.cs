﻿using DVCustomCarLoader.Effects;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DVCustomCarLoader.LocoComponents.Steam
{
    public class CustomChuffController : MonoBehaviour
    {
        public event ChuffController.ChuffAction OnChuff;

        protected CustomLocoControllerSteam loco;
        protected DrivingAnimation driverAnimation;

        protected float wheelCircumference;
        public int chuffsPerRevolution = 2;

        public int currentChuff;
        private int lastChuff;
        public float chuffKmh;
        public float chuffPower;
        private float revolutionPos = 0;

        private const float MPS_KPH_FACTOR = 3.6f;

        protected void Awake()
        {
            loco = GetComponent<CustomLocoControllerSteam>();
            if (!loco)
            {
                Main.Error("Chuff Controller: No custom steam controller for chuff controller");
                enabled = false;
            }

            driverAnimation = GetComponents<DrivingAnimation>().Where(d => d.IsDrivingWheels).SingleOrDefault();
            if (!driverAnimation)
            {
                Main.Error("Chuff Controller: No driver animation found, or more than one driver animation found");
                enabled = false;
            }

            wheelCircumference = driverAnimation.DefaultWheelRadius * Mathf.PI * 2;
            Main.LogVerbose("CustomChuffController awakened");
        }

        protected void Update()
        {
            chuffPower = loco.GetTotalPowerForcePercentage();
            float speed = (loco.drivingForce.wheelslip > 0f) ? (driverAnimation.DefaultWheelRadius * wheelCircumference) : loco.GetForwardSpeed();

            revolutionPos = (revolutionPos + speed * Time.deltaTime) % wheelCircumference;
            currentChuff = (int)(revolutionPos / wheelCircumference * chuffsPerRevolution) % chuffsPerRevolution;
            chuffKmh = speed * MPS_KPH_FACTOR;

            if (currentChuff != lastChuff)
            {
                lastChuff = currentChuff;
                OnChuff?.Invoke(chuffPower);
            }
        }
    }
}