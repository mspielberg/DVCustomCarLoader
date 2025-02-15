﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL_GameScripts
{
    /// <summary>
    /// The underlying type of cars.
    /// </summary>
    public enum BaseTrainCarType
    {
        NotSet = 0,
        LocoShunter = 10,
        LocoSteamHeavy = 20,
        Tender = 21,
        LocoSteamHeavyBlue = 22,
        TenderBlue = 23,
        LocoRailbus = 30,
        LocoDiesel = 40,
        FlatbedEmpty = 200,
        FlatbedStakes = 201,
        FlatbedMilitary = 202,
        AutorackRed = 250,
        AutorackBlue = 251,
        AutorackGreen = 252,
        AutorackYellow = 253,
        TankOrange = 300,
        TankWhite = 301,
        TankYellow = 302,
        TankBlue = 303,
        TankChrome = 304,
        TankBlack = 305,
        BoxcarBrown = 400,
        BoxcarGreen = 401,
        BoxcarPink = 402,
        BoxcarRed = 403,
        BoxcarMilitary = 404,
        RefrigeratorWhite = 450,
        HopperBrown = 500,
        HopperTeal = 501,
        HopperYellow = 502,
        PassengerRed = 600,
        PassengerGreen = 601,
        PassengerBlue = 602,
        HandCar = 700,
        NuclearFlask = 800
    }

    public enum BaseCargoContainerType
    {
        None,
        Hopper,
        TankerOil,
        TankerGas,
        TankerChem,
        Flatcar,
        FlatcarStakes,
        Boxcar,
        Gondola,
        Refrigerator,
        Cars,
        Passengers,
        MilitaryBoxcar,
        NuclearFlask,
        MilitaryFlatcar
    }
}
