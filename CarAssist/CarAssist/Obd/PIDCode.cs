namespace CarAssist.Obd
{
    public enum PIDCode
    {
        // Mode 1
        PIDsSupported1_01_1F = 0x0100,
        MILStatus = 0x0101,
        FreezeDTC = 0x102,
        FuelSystemStatus = 0x0103,
        CalculatedEngineLoadValue = 0x0104,
        CoolantTemperature = 0x0105,
        ShortTermFuelTrimBank1 = 0x0106,
        LongTermFuelTrimBank1 = 0x0107,
        ShortTermFuelTrimBank2 = 0x0108,
        LongTermFuelTrimBank2 = 0x0109,
        FuelPressure = 0x010A,
        IntakeManifoldAbsolutePressure = 0x010B,
        RPM = 0x010C,
        Speed = 0x010D,
        TimingAdvance = 0x010E,
        IntakeAirTemperature = 0x010F,
        MAFAirFlowRate = 0x0110,
        ThrottlePosition = 0x0111,
        SecondaryAirStatus = 0x0112,
        O2SensorsPresent = 0x0113,
        O2Sensor1_1 = 0x0114,
        O2Sensor1_2 = 0x0115,
        O2Sensor1_3 = 0x0116,
        O2Sensor1_4 = 0x0117,
        O2Sensor2_1 = 0x0118,
        O2Sensor2_2 = 0x0119,
        O2Sensor2_3 = 0x011A,
        O2Sensor2_4 = 0x011B,
        OBDCompliance = 0x011C,
        O2SensorsPresent_Alternate = 0x011D,
        AuxiliaryInputStatus = 0x011E,
        RuntimeSinceEngineStart = 0x011F,

        PIDsSupported1_21_3F = 0x0120,
        DistanceTraveledWithMILOn = 0x0121,
        FuelRailPressure = 0x0122,
        FuelRailGaugePressure = 0x0123,
        O2SensorLambda1 = 0x0124,
        O2SensorLambda2 = 0x0125,
        O2SensorLambda3 = 0x0126,
        O2SensorLambda4 = 0x0127,
        O2SensorLambda5 = 0x0128,
        O2SensorLambda6 = 0x0129,
        O2SensorLambda7 = 0x012A,
        O2SensorLambda8 = 0x012B,
        ExhaustGasRecirculation = 0x012C,
        ExhaustGasRecirculationError = 0x012D,
        EvaporationPurge = 0x012E,
        FuelTankLevel = 0x012F,
        WarmUpsSinceCodesCleared = 0x0130,
        DistanceTraveledSinceCodesCleared = 0x0131,
        EvaporativeSystemVaporPressure = 0x0132,
        AbsoluteBarometricPressure = 0x0133,
        O2Sensor3_1 = 0x0134,
        O2Sensor3_2 = 0x0135,
        O2Sensor3_3 = 0x0136,
        O2Sensor3_4 = 0x0137,
        O2Sensor3_5 = 0x0138,
        O2Sensor3_6 = 0x0139,
        O2Sensor3_7 = 0x013A,
        O2Sensor3_8 = 0x013B,
        CatalystTemperatureBank1Sensor1 = 0x013C,
        CatalystTemperatureBank2Sensor1 = 0x013D,
        CatalystTemperatureBank1Sensor2 = 0x013E,
        CatalystTemperatureBank2Sensor2 = 0x013F,

        PIDsSupported1_41_5F = 0x0140,
        //MonitorStatusDriveCycle = 0x0141,
        //ControlModuleVoltage = 0x0142,
        //AbsoluteLoadValue = 0x0143,
        //FuelAirCommandedEquivalenceRatio = 0x0144,
        //RelativeThrottlePosition = 0x0145,
        //AmbientAirTemperature = 0x0146,
        //AbsoluteThrottlePositionB = 0x0147,
        //AbsoluteThrottlePositionC = 0x0148,
        //AcceleratorPedalPositionD = 0x0149,
        //AcceleratorPedalPositionE = 0x014A,
        //AcceleratorPedalPositionF = 0x014B,
        //CommandedThrottleActuator = 0x014C,
        //TimeRunWithMILon = 0x014D,
        //TimeSinceDTCCleared = 0x014E,
        //MaxValueForFuelAir_O2SensorVoltage_O2SensorCurrent_IntakeManifoldAbsolutePressure = 0x014F,
        //MaxValueForAirFlowRateMAFSensor = 0x0150,
        FuelType = 0x0151,
        //EthanolFuelPercent = 0x0152,
        //AbsoluteEvapSystemVaporPressure = 0x0153,
        //EvapSystemVaporPressure = 0x0154,
        //ShortTermSecondaryO2SensorTrimBank1Bank3 = 0x0155,
        //LongTermSecondaryO2SensorTrimBank1Bank3 = 0x0156,
        //ShortTermSecondaryO2SensorTrimBank2Bank4 = 0x0157,
        //LongTermSecondaryO2SensorTrimBank2Bank4 = 0x0158,
        //FuelRailAbsolutePressure = 0x0159,
        //RelativeAcceleratorPedalPosition = 0x015A,
        //HybridBatteryPackRemainingLife = 0x015B,
        EngineOilTemperature = 0x015C,
        //FuelInjectionTiming = 0x015D,
        EngineFuelRate = 0x015E,
        //EmissionRequirements = 0x015F,

        PIDsSupported1_61_7F = 0x0160,
        //DriverDemandEnginePercentTorque = 0x0161,
        //ActualEnginePercentTorque = 0x0162,
        //EngineReferenceTorque = 0x0163,
        //EnginePercentTorqueData = 0x0164,
        //AuxiliaryInput = 0x0165,
        //MAFSensor = 0x0166,                             // MAF: Mass Air Flow
        //EngineCoolantTemperature = 0x0167,
        //IntakeAirTemperatureSensor = 0x0168,
        //EGRAndEGRError = 0x0169,
        //DieselIntakeAirFlowControlAndRelativeIntakeAirFlowPosition = 0x016A,
        //ExhaustGasRecirculationTemperature = 0x016B,
        //ThrottleActuatorControlAndRelativeThrottlePosition = 0x016C,
        //FuelPressureControlSystem = 0x016D,
        //InjectionPressureControlSystem = 0x016E,
        //TurbochargerCompressorInletPressure = 0x016F,
        //BoostPressureControl = 0x0170,
        //VGTControl = 0x0171,                            // VGT: Variable Geometry turbo
        //WastegateControl = 0x0172,
        //ExhaustPressure = 0x0173,
        //TurbochargerRPM = 0x0174,
        //TurbochargerTemperatureA = 0x0175,
        //TurbochargerTemperatureB = 0x0176,
        //ChargeAirCoolerTemperature = 0x0177,
        //ExhaustGasTemperatureBank1 = 0x0178,
        //ExhaustGasTemperatureBank2 = 0x0179,
        //DieselParticulateFilterBank1 = 0x017A,
        //DieselParticulateFilterBank2 = 0x017B,
        //DieselParticulateFilterTemperature = 0x017C,
        //NOxNTEControlAreaStatus = 0x017D,
        //PM_NTEControlAreaStatus = 0x017E,
        //EngineRuntime = 0x017F,

        // Mode 2
        // identical with mode 1, the returned values refer to last DTC (freeze frame) 

        // Mode 3
        RequestDTCs = 0x03,             // DTC: Diagnostic Trouble Code

        // Mode 4
        //ClearDTCsAndFreezeData = 0x04,

        // Mode 5
        // Mode 6
        // Mode 7
        RequestCurrentDTC = 0x07,
        // Mode 8

        // Mode 9
        PidsSupported9_01_1F = 0x0900,
        VINMessageCountInPID02 = 0x0901,
        VIN = 0x0902,                                       // Vehicle Identification Number
        //CalibrationIDMessageCountForPID04 = 0x0903,
        CalibrationID = 0x0904,
        //CVNMessageCountForPID06 = 0x0905,                   // CVN: Calibration verification numbers 
        CVN = 0x0906,
        //InUsePerformanceTrackingMessageCountForPID08_0B = 0x0907,
        //InUsePerformanceTrackingForSparkIgnitionVehicles = 0x0908,
        //ECUNameMessageCountForPID0A = 0x0909,
        ECUName = 0x090A,                                   // ECU: Engine Control Unit
        //InUsePerformanceTrackingForCompressionIgnitionVehicles = 0x090B
        // 0x090C - 0x09FF: ISO/SAE reserved
    };

}
