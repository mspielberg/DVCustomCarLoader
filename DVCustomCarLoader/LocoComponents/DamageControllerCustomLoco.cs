﻿using System.Collections.Generic;
using System.Reflection;
using CCL_GameScripts;
using DV.ServicePenalty;
using HarmonyLib;

namespace DVCustomCarLoader.LocoComponents
{
    public abstract class DamageControllerCustomLoco : DamageController, IServicePenaltyProvider
    {
        public abstract IEnumerable<DebtTrackingInfo> GetDebtComponents();
        public abstract void ResetDebt( DebtComponent debt );
        public abstract void UpdateDebtValue( DebtComponent debt );
        public abstract IEnumerable<PitStopRefillable> GetPitStopParameters();
        public abstract float GetPitStopLevel( ResourceType type );
        public abstract void ChangePitStopLevel( ResourceType type, float changeAmount );

        private static readonly FieldInfo carDamagePropsField =
            AccessTools.Field(typeof(CarDamageModel), "carDamageProperties");

        protected void SetBodyDamageProps( CarDamageProperties value )
        {
            carDamagePropsField.SetValue(bodyDamage, value);
        }
    }

    public abstract class DamageControllerCustomLoco<TCtrl,TEvents,TConfig> : DamageControllerCustomLoco
        where TCtrl : CustomLocoController
        where TEvents : CustomLocoSimEvents
        where TConfig : DamageConfigBasic
    {
        protected TCtrl locoController;
        protected TEvents eventController;
        protected TConfig config;

        protected override void Awake()
        {
            controller = locoController = GetComponent<TCtrl>();
            if( !locoController ) Main.Error($"Missing {typeof(TCtrl).Name} on {gameObject.name}");

            eventController = GetComponent<TEvents>();
            if( !eventController ) Main.Error($"Missing {typeof(TEvents).Name} on {gameObject.name}");

            config = GetComponent<TConfig>();
            if( !config ) Main.Error($"Missing {typeof(TConfig).Name} on {gameObject.name}");

            InitializeTrainDamages();
        }

        protected override void Start()
        {
            base.Start();
            ApplyBodyParameters();
        }

        protected override void InitializeTrainDamages()
        {
            if( wheels == null )
            {
                wheels = new TrainDamage(config.WheelHitpoints);
            }
            else
            {
                wheels.fullHitPoints = config.WheelHitpoints;
                if( wheels.fullHitPoints == 0f )
                {
                    Main.Error("TrainDamage[wheels].fullHitPoints is set to invalid value 0! Overriding to 1000");
                    wheels.fullHitPoints = 1000f;
                }
                wheels.SetCurrentHitPoints(wheels.fullHitPoints);
            }
        }

        protected void ApplyBodyParameters()
        {
            if( bodyDamage )
            {
                var props = new CarDamageProperties(
                    config.BodyHitpoints,
                    config.BodyCollisionResistance,
                    config.BodyCollisionMultiplier,
                    config.BodyFireResistance,
                    config.BodyFireMultiplier,
                    config.DamageTolerance
                );

                SetBodyDamageProps(props);
                Main.LogVerbose("Applied body damage properties");
            }
            else
            {
                Main.Warning("Body damage properties not found!");
            }
        }
    }
}