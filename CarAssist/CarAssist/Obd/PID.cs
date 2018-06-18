using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CarAssist.Obd
{
    public class PID
    {
        private PID() { }

        static public PID GetPID(PIDCode pidCode)
        {
            foreach (var pid in _listPIDs)
            {
                if (pid.PIDCode == pidCode)
                    return pid;
            }
            return null;
        }

        static private List<PID> _listPIDs = new List<PID>()
        {
            new PID
            {
                PIDCode = PIDCode.PIDsSupported1_01_1F, ByteCount = 4, Description = "PIDs supported 1-31",
                Decoder = (rawValue) => PidList(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.MILStatus, ByteCount = 4, Description = "Status since DTCs cleared",
                Decoder = (rawValue) => GenericRaw(rawValue)                                                // TODO: status
            },
            new PID
            {
                PIDCode = PIDCode.FreezeDTC, ByteCount = 2, Description = "Freeze DTC",
                Decoder = (rawValue) => GenericRaw(rawValue)                                                // TODO: single_dtc
            },
            new PID
            {
                PIDCode = PIDCode.FuelSystemStatus, ByteCount = 2, Description = "Fuel System Status",
                Decoder = (rawValue) => FuelSystemStatus(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.CalculatedEngineLoadValue, ByteCount = 1, Description = "Calculated engine load value %",
                Decoder = (rawValue) => Percent(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.CoolantTemperature, ByteCount = 1, Description = "Engine coolant temperature C°",
                Decoder = (rawValue) => Celsius(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.ShortTermFuelTrimBank1, ByteCount = 1, Description = "Short term fuel trim bank 1 %",
                Decoder = (rawValue) => PercentCentered(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.LongTermFuelTrimBank1, ByteCount = 1, Description = "Long term fuel trim bank 1 %",
                Decoder = (rawValue) => PercentCentered(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.ShortTermFuelTrimBank2, ByteCount = 1, Description = "Short term fuel trim bank 2 %",
                Decoder = (rawValue) => PercentCentered(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.LongTermFuelTrimBank2, ByteCount = 1, Description = "Long term fuel trim bank 2 %",
                Decoder = (rawValue) => PercentCentered(rawValue)
            },

            new PID
            {
                PIDCode = PIDCode.FuelPressure, ByteCount = 1, Description = "Fuel pressure kPa",
                Decoder = (rawValue) => FuelPressure(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.IntakeManifoldAbsolutePressure, ByteCount = 1, Description = "Intake manifold absolute pressure kPa",
                Decoder = (rawValue) => GenericInt(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.RPM, ByteCount = 2, Description = "RPM U/min",
                Decoder = (rawValue) => RPM(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.Speed ,ByteCount = 1, Description = "Speed km/h",
                Decoder = (rawValue) => GenericInt(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.TimingAdvance, ByteCount = 1, Description = "Timing advance",
                Decoder = (rawValue) =>
                {
                    var number = GenericInt(rawValue);

                    if (number != null)
                    {
                        return ((int)number - 128) / 2.0;
                    }
                    return null;
                }
            },
            new PID()
            {
                PIDCode = PIDCode.IntakeAirTemperature, ByteCount = 1, Description = "Intake air temperature C°",
                Decoder = (rawValue) => Celsius(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.MAFAirFlowRate, ByteCount = 2, Description = "MAF air flow rate in grams/sec",
                Decoder = (rawValue) => GramsPerSecond(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.ThrottlePosition, ByteCount = 1, Description = "Throttle position in %",
                Decoder = (rawValue) => Percent(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.SecondaryAirStatus, ByteCount = 1, Description = "Secondary air status",
                Decoder = (rawValue) => AirStatus(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.O2SensorsPresent, ByteCount = 1, Description = "Oxygen Sensors Present",
                Decoder = (rawValue) => O2Sensors(rawValue)                                                    // TODO: o2_sensors
            },
            new PID()
            {
                PIDCode = PIDCode.O2Sensor1_1, ByteCount = 2, Description = "Oxygen: Bank 1 - Sensor 1 Voltage",
                Decoder = (rawValue) => SensorVoltage(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.O2Sensor1_2, ByteCount = 2, Description = "Oxygen: Bank 1 - Sensor 2 Voltage",
                Decoder = (rawValue) => SensorVoltage(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.O2Sensor1_3, ByteCount = 2, Description = "Oxygen: Bank 1 - Sensor 3 Voltage",
                Decoder = (rawValue) => SensorVoltage(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.O2Sensor1_4, ByteCount = 2, Description = "Oxygen: Bank 1 - Sensor 4 Voltage",
                Decoder = (rawValue) => SensorVoltage(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.O2Sensor2_1, ByteCount = 2, Description = "Oxygen: Bank 2 - Sensor 1 Voltage",
                Decoder = (rawValue) => SensorVoltage(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.O2Sensor2_2, ByteCount = 2, Description = "Oxygen: Bank 2 - Sensor 2 Voltage",
                Decoder = (rawValue) => SensorVoltage(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.O2Sensor2_3, ByteCount = 2, Description = "Oxygen: Bank 2 - Sensor 3 Voltage",
                Decoder = (rawValue) => SensorVoltage(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.O2Sensor2_4, ByteCount = 2, Description = "Oxygen: Bank 2 - Sensor 4 Voltage",
                Decoder = (rawValue) => SensorVoltage(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.OBDCompliance, ByteCount = 1, Description = "OBD Standards Compliance",
                Decoder = (rawValue) => OBDCompliance(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.O2SensorsPresent_Alternate, ByteCount = 1, Description = "Oxygen Sensors present (alternate)",
                Decoder = (rawValue) => GenericRaw(rawValue)                                                    // TODO: o2_sensors_alt
            },
            new PID()
            {
                PIDCode = PIDCode.AuxiliaryInputStatus, ByteCount = 1, Description = "Auxiliary input status (power take off)",
                Decoder = (rawValue) => GenericRaw(rawValue)                                                    // TODO: aux_input_status
            },
            new PID()
            {
                PIDCode = PIDCode.RuntimeSinceEngineStart, ByteCount = 2, Description = "Engine Run Time since starting engine in seconds",
                Decoder = (rawValue) => GenericInt(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.PIDsSupported1_21_3F, ByteCount = 4, Description = "PIDs supported 32-63",
                Decoder = (rawValue) => PidList(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.DistanceTraveledWithMILOn, ByteCount = 2, Description = "Distance traveled with MIL on in km",
                Decoder = (rawValue) => GenericInt(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.FuelRailPressure, ByteCount = 2, Description = "Fuel Rail Pressure (relative to vacuum) in kPa",
                Decoder = (rawValue) => 0.079 * ((int?)GenericInt(rawValue) ?? 0)
            },
            new PID
            {
                PIDCode = PIDCode.FuelRailGaugePressure, ByteCount = 2, Description = "Fuel Rail Pressure (direct inject) in kPa",
                Decoder = (rawValue) => { var value = (int?)GenericInt(rawValue);  return value == null ? null : 10.0 * value; }
            },
            new PID
            {
                PIDCode = PIDCode.O2SensorLambda1, ByteCount = 4, Description = "Oxygen Sensor 1 WR Lambda Voltage",
                Decoder = (rawValue) => O2SensorLambdaVoltage(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2SensorLambda2, ByteCount = 4, Description = "Oxygen Sensor 2 WR Lambda Voltage",
                Decoder = (rawValue) => O2SensorLambdaVoltage(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2SensorLambda3, ByteCount = 4, Description = "Oxygen Sensor 3 WR Lambda Voltage",
                Decoder = (rawValue) => O2SensorLambdaVoltage(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2SensorLambda4, ByteCount = 4, Description = "Oxygen Sensor 4 WR Lambda Voltage",
                Decoder = (rawValue) => O2SensorLambdaVoltage(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2SensorLambda5, ByteCount = 4, Description = "Oxygen Sensor 5 WR Lambda Voltage",
                Decoder = (rawValue) => O2SensorLambdaVoltage(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2SensorLambda6, ByteCount = 4, Description = "Oxygen Sensor 6 WR Lambda Voltage",
                Decoder = (rawValue) => O2SensorLambdaVoltage(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2SensorLambda7, ByteCount = 4, Description = "Oxygen Sensor 7 WR Lambda Voltage",
                Decoder = (rawValue) => O2SensorLambdaVoltage(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2SensorLambda8, ByteCount = 4, Description = "Oxygen Sensor 8 WR Lambda Voltage",
                Decoder = (rawValue) => O2SensorLambdaVoltage(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.ExhaustGasRecirculation, ByteCount = 1, Description = "Exhaust Gas Recirculation in %",
                Decoder = (rawValue) => Percent(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.ExhaustGasRecirculationError, ByteCount = 1, Description = "Exhaust Gas Recirculation Error",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.EvaporationPurge, ByteCount = 1, Description = "Evaporative purge",
                Decoder = (rawValue) => Percent(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.FuelTankLevel, ByteCount = 1, Description = "Fuel tank level %",
                Decoder = (rawValue) => Percent(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.WarmUpsSinceCodesCleared, ByteCount = 1, Description = "Warm-ups since codes cleared",
                Decoder = (rawValue) => GenericInt(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.DistanceTraveledSinceCodesCleared, ByteCount = 2, Description = "Distance traveled since codes cleared in km",
                Decoder = (rawValue) => GenericInt(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.EvaporativeSystemVaporPressure, ByteCount = 2, Description = "Evap. System Vapor Pressure in Pa",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.AbsoluteBarometricPressure, ByteCount = 1, Description = "Absolute Barometric Pressure in kPa",
                Decoder = (rawValue) => GenericInt(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2Sensor3_1, ByteCount = 4, Description = "Oxygen Sensor 1 in mA",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2Sensor3_2, ByteCount = 4, Description = "Oxygen Sensor 2 in mA",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2Sensor3_3, ByteCount = 4, Description = "Oxygen Sensor 3 in mA",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2Sensor3_4, ByteCount = 4, Description = "Oxygen Sensor 4 in mA",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2Sensor3_5, ByteCount = 4, Description = "Oxygen Sensor 5 in mA",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2Sensor3_6, ByteCount = 4, Description = "Oxygen Sensor 6 in mA",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2Sensor3_7, ByteCount = 4, Description = "Oxygen Sensor 7 in mA",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.O2Sensor3_8, ByteCount = 4, Description = "Oxygen Sensor 8 in mA",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.CatalystTemperatureBank1Sensor1, ByteCount = 2, Description = "Catalyst Temperature: Bank 1, Sensor 1 in C°",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.CatalystTemperatureBank1Sensor2, ByteCount = 2, Description = "Catalyst Temperature: Bank 1, Sensor 2 in C°",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.CatalystTemperatureBank2Sensor1, ByteCount = 2, Description = "Catalyst Temperature: Bank 2, Sensor 1 in C°",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },
            new PID
            {
                PIDCode = PIDCode.CatalystTemperatureBank2Sensor2, ByteCount = 2, Description = "Catalyst Temperature: Bank 2, Sensor 2 in C°",
                Decoder = (rawValue) => GenericRaw(rawValue)
            },

            new PID
            {
                PIDCode = PIDCode.PIDsSupported1_41_5F, ByteCount = 4, Description = "PIDs supported 64-95",
                Decoder = (rawValue) => PidList(rawValue)
            },
            //...
            new PID
            {
                PIDCode = PIDCode.FuelType, ByteCount = 1, Description = "Engine fuel type",
                Decoder = (rawValue) => FuelType(rawValue)
            },
            //...
            new PID
            {
                PIDCode = PIDCode.EngineOilTemperature, ByteCount = 1, Description = "Engine oil temperature",
                Decoder = (rawValue) => Celsius(rawValue)
            },
            //...
            new PID
            {
                PIDCode = PIDCode.EngineFuelRate, ByteCount = 2, Description = "Engine fuel rate L/hour",
                Decoder = (rawValue) => FuelRate(rawValue)
            },
            //...
            new PID
            {
                PIDCode = PIDCode.PIDsSupported1_61_7F, ByteCount = 4, Description = "PIDs supported 96-127",
                Decoder = (rawValue) => PidList(rawValue)
            },
            //...

            // PID 2
            //...

            // PID 3
            new PID
            {
                PIDCode = PIDCode.RequestDTCs, ByteCount = 0, Description = "Request Diagnostic Trouble Code (DTC)",
                Decoder = (rawValue) => GenericRaw(rawValue)                                        // TODO: dtc
            },

            // PID 4
            //new PID
            //{
            //    PIDCode = PIDCode.ClearDTCsAndFreezeData, ByteCount = 0, Description = "Clear DTCs and freeze data",
            //    Decoder = (rawValue) => GenericRaw(rawValue)
            //},
            // PID 5
            // PID 6
            // PID 7
            new PID
            {
                PIDCode = PIDCode.RequestCurrentDTC, ByteCount = 0, Description = "Get DTCs from the current/last driving cycle",
                Decoder = (rawValue) => GenericRaw(rawValue)                                        // TODO: dtc
            },

            // PID 8
            //...

            // PID 9
            new PID()
            {
                PIDCode = PIDCode.PidsSupported9_01_1F, ByteCount = 4, Description = "PIDs supported 1-31",
                Decoder = (rawValue) => PidList(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.VINMessageCountInPID02, ByteCount = 0, Description = "Vehicle Identification Number",
                Decoder = (rawValue) => CharacterString(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.VIN, ByteCount = 0, Description = "Vehicle Identification Number",
                Decoder = (rawValue) => CharacterString(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.CalibrationID, ByteCount = 0, Description = "Calibration Identification (CALID)",
                Decoder = (rawValue) => CharacterString(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.CVN, ByteCount = 0, Description = "Calibration Verification Numbers (CVN)",
                Decoder = (rawValue) => CharacterString(rawValue)
            },
            new PID()
            {
                PIDCode = PIDCode.ECUName, ByteCount = 0, Description = "Engine Control Unit (ECU) name",
                Decoder = (rawValue) => CharacterString(rawValue)
            },

        };

        public string Description { get; private set; }
        public PIDCode PIDCode { get; private set; }
        public int ByteCount { get; private set; }
        public Func<string, object> Decoder { get; private set; }
        public object Value { get; private set; }

        public static PID Parse(PIDCode pidCode, ObdCommand command, string response, string ecu)
        {
            var pid = _listPIDs.FirstOrDefault(p => p.PIDCode == pidCode);

            if (pid == null)
                throw new ArgumentException($"PIDCode {pidCode} not defined in PID.CS (_listPids)");

            var rawValue = GetRawValue(pid, response, ecu);

            if (rawValue != null)
                pid.Value = pid.Decoder(rawValue);

            return pid;
        }

        static private string GetRawValue(PID pid, string response, string ecu)
        {
            string value = null;

            var pidReturn = PID.ToPidReturn(pid.PIDCode);

            response = Regex.Replace(response, @"[\n>]", "");
            var lines = response.Split('\r');

            int index = -1;
            foreach (var line in lines)
            {
                if (!IsCurrentEcu(line, ecu))
                    continue;

                var cleaned = Regex.Replace(line, @"[\n> ]", "");
                if (index == -1)
                {
                    index = cleaned.IndexOf(pidReturn);
                    if (index == -1)
                        continue;
                }
                if (cleaned.Length >= index + pidReturn.Length + pid.ByteCount * 2)
                {
                    if (pid.ByteCount == 0) // mehrzeilig
                    {
                        if (value == null)
                            value = "";
                        if (ecu.Length == 2)    // ECU mit 2 Zeichen (3.tes Byte=12): 83 F1 12 41 0D 00 D4
                        {
                            value += cleaned.Substring(index + 2 + pidReturn.Length);   // +2 = sequence number if multiline
                            value = value.Remove(value.Length - 2, 2);                  // remove checksum
                        }
                        else                    // ECU with 3 chars (7E8): 7E8 03 41 0D 31 EB
                        {
                            if (value.Length == 0)
                                value += cleaned.Substring(index + 2 + pidReturn.Length);
                            else
                                value += cleaned.Substring(3 + 2);
                        }
                    }
                    else
                    {
                        value = cleaned.Substring(index + pidReturn.Length, pid.ByteCount * 2);
                    }
                }
            }
            return value;
        }

        private static bool IsCurrentEcu(string response, string ecu)
        {
            var parts = response.Split(' ');

            if (parts[0].Length == 3 && parts[0] == ecu)
                return true;
            else if (parts.Length > 2 && parts[2] == ecu)
                return true;

            return false;
        }

        static private object GenericRaw(string rawValue)
        {
            return rawValue;
        }

        static private object GenericInt(string rawValue)
        {
            if (!int.TryParse(rawValue, NumberStyles.HexNumber, null, out int value))
                return null;

            return value;
        }

        static private object RPM(string rawValue)
        {
            if (!int.TryParse(rawValue, NumberStyles.HexNumber, null, out int value))
                return null;

            return value * 0.25;        // value / 4.0
        }

        static private object Celsius(string rawValue)
        {
            if (!int.TryParse(rawValue, NumberStyles.HexNumber, null, out int value))
                return null;

            return value - 40;
        }

        static private object GramsPerSecond(string rawValue)
        {
            if (!int.TryParse(rawValue, NumberStyles.HexNumber, null, out int value))
                return null;

            return value * 0.01;           // value / 100.0
        }

        static private object Percent(string rawValue)
        {
            var number = GenericInt(rawValue);

            if (number != null)
            {
                return (int)number * 100.0 / 255.0;
            }

            return null;
        }

        static private object PercentCentered(string rawValue)
        {
            var number = GenericInt(rawValue);

            if (number != null)
            {
                return ((int)number - 128.0) * 100.0 / 128.0;
            }

            return null;
        }

        static private object FuelPressure(string rawValue)
        {
            var number = GenericInt(rawValue);

            if (number != null)
            {
                // 0 to 765 kPa (KiloPascal)
                return (int)number * 3;
            }

            return null;
        }

        static private object SensorVoltage(string rawValue)
        {
            var number = GenericInt(rawValue);

            if (number != null)
            {
                // 0 bis 1.275 Volt
                return (int)number / 200.0;
            }

            return null;
        }
        static private object FuelRate(string rawValue)
        {
            var number = GenericInt(rawValue);

            if (number != null)
            {
                // 0 to 3276,75 L/h
                return (int)number / 20.0;
            }

            return null;
        }

        static private object FuelType(string rawValue)
        {
            var number = GenericInt(rawValue);

            if (number != null && (int)number < FUEL_TYPES.Length)
            {
                return FUEL_TYPES[(int)number];
            }

            return null;
        }

        static string[] FUEL_TYPES = {
            "Not available",
            "Gasoline",
            "Methanol",
            "Ethanol",
            "Diesel",
            "LPG",
            "CNG",
            "Propane",
            "Electric",
            "Bifuel running Gasoline",
            "Bifuel running Methanol",
            "Bifuel running Ethanol",
            "Bifuel running LPG",
            "Bifuel running CNG",
            "Bifuel running Propane",
            "Bifuel running Electricity",
            "Bifuel running electric and combustion engine",
            "Hybrid gasoline",
            "Hybrid Ethanol",
            "Hybrid Diesel",
            "Hybrid Electric",
            "Hybrid running electric and combustion engine",
            "Hybrid Regenerative",
            "Bifuel running diesel"
        };

        static private object O2SensorLambdaVoltage(string rawValue)
        {
            var value = (int?)GenericInt(rawValue.Substring(4, 4));

            if (value != null)
                return (value * 8.0) / 65536.0;

            return null;
        }

        static private object PidList(string rawValue)
        {
            return int.Parse(rawValue, NumberStyles.HexNumber);
        }

        public static string CharacterString(string rawValue)
        {
            rawValue = rawValue.TrimStart('0');
            if (rawValue.Length % 2 != 0)
                rawValue = '0' + rawValue;
            rawValue = rawValue.TrimEnd('0');
            if (rawValue.Length % 2 != 0)
                rawValue = rawValue + '0';

            var bytes = new byte[rawValue.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = System.Convert.ToByte(rawValue.Substring(i * 2, 2), 16);
            }
            return Encoding.ASCII.GetString(bytes);
        }

        static private object FuelSystemStatus(string rawValue)
        {
            var value = (int?)GenericInt(rawValue.Substring(0, 2));

            if (value != null)
            {
                if (value != 0)
                {
                    int index = (int)Math.Log((double)value, 2);

                    if (index < FUEL_STATUS.Length)
                        return FUEL_STATUS[index];
                }
            }
            return null;
        }

        static string[] FUEL_STATUS = {
            "Open loop due to insufficient engine temperature",
            "Closed loop, using oxygen sensor feedback to determine fuel mix",
            "Open loop due to engine load OR fuel cut due to deceleration",
            "Open loop due to system failure",
            "Closed loop, using at least one oxygen sensor but there is a fault in the feedback system"
        };

        static private object AirStatus(string rawValue)
        {
            var value = (int?)GenericInt(rawValue.Substring(0, 2));

            if (value != null && value != 0)
            {
                int index = (int)Math.Log((double)value, 2);

                if (index < AIR_STATUS.Length)
                    return AIR_STATUS[index];
            }
            return null;
        }

        static string[] AIR_STATUS = {
            "Upstream",
            "Downstream of catalytic converter",
            "From the outside atmosphere or off",
            "Pump commanded on for diagnostics"
        };

        static private object O2Sensors(string rawValue)
        {
            string result = "";
            var value = (int?)GenericInt(rawValue.Substring(0, 2));

            if (value != null && value != 0)
            {
                int pos = 0b00000001;

                for (int i = 1; i <= 8; i++)
                {
                    if ((value & pos) != 0)
                    {
                        int bank = i / 4 + 1;
                        int sensor = i <= 4 ? i : i - 4;
                        result += $"Bank{bank}_Sensor{sensor} ";
                    }
                    pos <<= 1;
                }
            }
            return result == "" ? null : result;
        }

        static private object OBDCompliance(string rawValue)
        {
            var pos = (int?)GenericInt(rawValue);

            if (pos != null && pos <= OBD_COMPLIANCE.Length - 1)
            {
                return OBD_COMPLIANCE[(int)pos];
            }

            return null;
        }

        private static readonly string[] OBD_COMPLIANCE =       // see open source project: python-OBD
        {
            "Undefined",
            "OBD-II as defined by the CARB",
            "OBD as defined by the EPA",
            "OBD and OBD-II",
            "OBD-I",
            "Not OBD compliant",
            "EOBD (Europe)",
            "EOBD and OBD-II",
            "EOBD and OBD",
            "EOBD, OBD and OBD II",
            "JOBD (Japan)",
            "JOBD and OBD II",
            "JOBD and EOBD",
            "JOBD, EOBD, and OBD II",
            "Reserved",
            "Reserved",
            "Reserved",
            "Engine Manufacturer Diagnostics (EMD)",
            "Engine Manufacturer Diagnostics Enhanced (EMD+)",
            "Heavy Duty On-Board Diagnostics (Child/Partial) (HD OBD-C)",
            "Heavy Duty On-Board Diagnostics (HD OBD)",
            "World Wide Harmonized OBD (WWH OBD)",
            "Reserved",
            "Heavy Duty Euro OBD Stage I without NOx control (HD EOBD-I)",
            "Heavy Duty Euro OBD Stage I with NOx control (HD EOBD-I N)",
            "Heavy Duty Euro OBD Stage II without NOx control (HD EOBD-II)",
            "Heavy Duty Euro OBD Stage II with NOx control (HD EOBD-II N)",
            "Reserved",
            "Brazil OBD Phase 1 (OBDBr-1)",
            "Brazil OBD Phase 2 (OBDBr-2)",
            "Korean OBD (KOBD)",
            "India OBD I (IOBD I)",
            "India OBD II (IOBD II)",
            "Heavy Duty Euro OBD Stage VI (HD EOBD-IV)"
        };

        public static string ToPidString(PIDCode code)
        {
            return ((int)code).ToString("X4");
        }

        public static string ToPidReturn(PIDCode code)
        {
            var pidCode = ((int)code).ToString("X4");
            return '4' + pidCode.Substring(1);
        }
    }
}
