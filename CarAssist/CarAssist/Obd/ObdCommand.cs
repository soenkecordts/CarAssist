using CarAssist.Connection;
using CarAssist.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;


namespace CarAssist.Obd
{
    public class ObdCommand
    {
        public ILogger Logger { get; set; }

        public bool IsPidSupported(PIDCode pidCode)
        {
            int[] list = PidsSupported;

            if (list != null)
                return list.FirstOrDefault(n => n == (int)pidCode) != 0;

            return false;
        }

        public PID this[PIDCode pidCode]
        {
            get
            {
                var pid = PID.GetPID(pidCode);

                if (pid == null)     // not registered in PID.cs 
                    Logger?.WriteLine($"{pidCode} not found in static PID list (PID.cs)");

                return pid;
            }
        }

        public IConnection Connection { get; private set; }
        public string LastResponse { get; private set; }
        public string CurrentEcu { get; set; }

        public ObdCommand(IConnection connection)
        {
            Connection = connection;
        }

        public bool Open()
        {
            if (OBDSetDefaults() == null)
                return false;
            if (OBDReset() == null)
                return false;

            OBDEcho = false; // no \n following \r, only important for terminal program
            OBDPrintingSpaces = true;
            OBDLinefeed = false;
            OBDProtocol = ObdCommand.Protocol.Auto;
            OBDHeaders = true;

            // Caching ECUs
            object value = ECUs;

            value = PidsSupported;
            value = OBDDeviceDescription;

            return ECUs != null;
        }

        /// OBD Commands 
        ///////////////////////////////////
        public IList<string> ECUs
        {
            get
            {
                return GetECUs();
            }
        }

        private int[] _pidsSupported = null;
        public int[] PidsSupported
        {
            get
            {
                if (_pidsSupported == null)
                {
                    var list = new List<int>();

                    int? value = (int?)GetValue(PIDCode.PIDsSupported1_01_1F);
                    if (value != null)
                        Converter.DecodeSupportedPids((int)value, 0x0100, list);
                    value = (int?)GetValue(PIDCode.PIDsSupported1_21_3F);
                    if (value != null)
                        Converter.DecodeSupportedPids((int)value, 0x0120, list);
                    value = (int?)GetValue(PIDCode.PIDsSupported1_41_5F);
                    if (value != null)
                        Converter.DecodeSupportedPids((int)value, 0x0140, list);
                    value = (int?)GetValue(PIDCode.PIDsSupported1_61_7F);
                    if (value != null)
                        Converter.DecodeSupportedPids((int)value, 0x0160, list);
                    value = (int?)GetValue(PIDCode.PidsSupported9_01_1F);
                    if (value != null)
                        Converter.DecodeSupportedPids((int)value, 0x0900, list);

                    _pidsSupported = list.ToArray();

                    if (Logger != null)
                    {
                        var pidsSupported = string.Join(", ", _pidsSupported.Select(b => "0x0" + b.ToString("X2:4")));
                        Logger?.WriteLine($"Supported PIDs: {pidsSupported}");
                    }
                }
                return _pidsSupported;
            }
        }

        public string MalfunctionIndicatorLightStatus
        {
            get
            {
                return GetValue(PIDCode.MILStatus) as string;
            }
        }

        public string FuelSystemStatus
        {
            get
            {
                return GetValue(PIDCode.FuelSystemStatus) as string;
            }
        }

        public double? CalculatedEngineLoadValue
        {
            get
            {
                return GetValue(PIDCode.CalculatedEngineLoadValue) as double?;
            }
        }

        public int? CoolantTemperature
        {
            get
            {
                return GetValue(PIDCode.CoolantTemperature) as int?;
            }
        }

        public string FuelPressure
        {
            get
            {
                return GetValue(PIDCode.FuelPressure) as string;
            }
        }

        public string IntakeManifoldAbsolutePressure
        {
            get
            {
                return GetValue(PIDCode.IntakeManifoldAbsolutePressure) as string;
            }
        }
        public double? RPM
        {
            get
            {
                return GetValue(PIDCode.RPM) as double?;
            }
        }

        public int? Speed
        {
            get
            {
                return GetValue(PIDCode.Speed) as int?;
            }
        }
        public int? IntakeAirTemperature
        {
            get
            {
                return GetValue(PIDCode.IntakeAirTemperature) as int?;
            }
        }

        public double? MAFAirFlowRate
        {
            get
            {
                return GetValue(PIDCode.MAFAirFlowRate) as double?;
            }
        }

        public double? ThrottlePosition
        {
            get
            {
                return GetValue(PIDCode.ThrottlePosition) as double?;
            }
        }

        public double? O2SensorLambda1
        {
            get
            {
                return GetValue(PIDCode.O2SensorLambda1) as double?;
            }
        }
        public double? O2SensorLambda2
        {
            get
            {
                return GetValue(PIDCode.O2SensorLambda2) as double?;
            }
        }
        public double? O2SensorLambda3
        {
            get
            {
                return GetValue(PIDCode.O2SensorLambda3) as double?;
            }
        }
        public double? O2SensorLambda4
        {
            get
            {
                return GetValue(PIDCode.O2SensorLambda4) as double?;
            }
        }
        public double? O2SensorLambda5
        {
            get
            {
                return GetValue(PIDCode.O2SensorLambda5) as double?;
            }
        }

        public double? O2SensorLambda6
        {
            get
            {
                return GetValue(PIDCode.O2SensorLambda6) as double?;
            }
        }
        public double? O2SensorLambda7
        {
            get
            {
                return GetValue(PIDCode.O2SensorLambda7) as double?;
            }
        }
        public double? O2SensorLambda8
        {
            get
            {
                return GetValue(PIDCode.O2SensorLambda8) as double?;
            }
        }

        string _VIN;
        public string VIN
        {
            get
            {
                if(_VIN == null)
                    _VIN = GetValue(PIDCode.VIN) as string;

                return _VIN;
            }
        }

        string _CalibrationID;
        public string CalibrationID
        {
            get
            {
                if(_CalibrationID == null)
                    _CalibrationID = GetValue(PIDCode.CalibrationID) as string;

                return _CalibrationID;
            }
        }

        string _CVN;
        public string CVN
        {
            get
            {
                if(_CVN == null)
                    _CVN = GetValue(PIDCode.CVN) as string;

                return _CVN;
            }
        }

        /// ELM327 Commands 
        string _OBDVersionID;
        public string OBDVersionID
        {
            get
            {
                if(_OBDVersionID == null)
                    _OBDVersionID = Execute("ATI");

                return _OBDVersionID;
            }
        }

        string _OBDDeviceDescription;
        public string OBDDeviceDescription
        {
            get
            {
                if(_OBDDeviceDescription == null)
                    _OBDDeviceDescription = Execute("AT@1");

                return _OBDDeviceDescription;
            }
        }

        string _OBDDeviceID;
        public string OBDDeviceID
        {
            get
            {
                if(_OBDDeviceID == null)
                    _OBDDeviceID = Execute("AT@2");

                return _OBDDeviceID;
            }
        }

        string _OBDCompliance;
        public string OBDCompliance
        {
            get
            {
                if (_OBDCompliance == null)
                    _OBDCompliance = (string)GetValue(PIDCode.OBDCompliance);

                return _OBDCompliance;
            }
        }

        public string OBDVoltage
        {
            get
            {
                return Execute("ATRV");
            }
        }

        public bool OBDPrintingSpaces
        {
            set
            {
                int val = value == true ? 1 : 0;
                Execute($"ATS{val}");
            }
        }

        public bool OBDLinefeed
        {
            set
            {
                int val = value == true ? 1 : 0;
                Execute($"ATL{val}");
            }
        }

        public bool OBDMemory
        {
            set
            {
                int val = value == true ? 1 : 0;
                Execute($"ATM{val}");
            }
        }

        public bool OBDEcho
        {
            set
            {
                int val = value == true ? 1 : 0;
                Execute($"ATE{val}");
            }
        }

        public bool OBDHeaders
        {
            set
            {
                int val = value == true ? 1 : 0;
                Execute($"ATH{val}");
            }
        }

        public int OBDTimeoutInMSec
        {
            set
            {
                if (value < 0)
                    throw new ArgumentException("Only positive possible for ATST");

                Execute($"ATST{value / 4}");
            }
        }

        public enum Protocol
        {
            Auto = 0,
            SAE_J1850_PWM = 1,
            SAE_J1850_VPW = 2,
            ISO_9141_2 = 3,
            ISO_14230_4_KWP = 4,
            ISO_14230_4_KWP_FAST = 5,
            ISO_15765_4_CAN_A = 6,
            ISO_15765_4_CAN_B = 7,
            ISO_15765_4_CAN_C = 8,
            ISO_15765_4_CAN_D = 9,
            SAE_J1939_CAN = 10,
            USER1_CAN = 11,
            USER2_CAN = 12
        }

        public Protocol OBDProtocol
        {
            get
            {
                string result = Execute($"ATDPN").Remove(0, 1);

                return (Protocol)int.Parse(result, NumberStyles.HexNumber);
            }

            set
            {
                Execute($"ATSP{(int)value}");
            }
        }

        public string OBDProtocolDescription
        {
            get
            {
                return Execute($"ATDP");
            }
        }

        public string OBDReset()
        {
            return Execute("ATZ");
        }

        public string OBDWarmStart()
        {
            return Execute("ATWS");
        }

        public string OBDSetDefaults()
        {
            return Execute("ATD");
        }

        public string OBDSetLowPower()
        {
            return Execute("ATLP");
        }

        public string Execute(string command)
        {
            command += '\r';
            LastResponse = Connection.WriteAndRead(command);

            if (string.IsNullOrEmpty(LastResponse))
                return null;

            LastResponse = Regex.Replace(LastResponse, @"[\0\n\r>]", "");
            LastResponse = LastResponse.Trim();

            if (LastResponse.Contains("SEARCHING..."))
                LastResponse = LastResponse.Replace("SEARCHING...", "");

            if (LastResponse.Contains("UNABLE TO CONNECT"))
                throw new ObdException("Connection to OBD not possible. Check the ignition.");

            if (LastResponse.Contains("NO DATA") || LastResponse.Contains("STOPPED") || 
                LastResponse == "?" || LastResponse == "")
                return null;

            return LastResponse;
        }

        public object GetValue(PIDCode pidCode)
        {
            LastResponse = Connection.WriteAndRead(PID.ToPidString(pidCode) + '\r');

            if (LastResponse == null)
                throw new Exception("Connection to OBD adapter lost");

            LastResponse = LastResponse.ToUpper();

            var pid = PID.Parse(pidCode, this, LastResponse, CurrentEcu);

            Logger?.WriteLine($"{pidCode}: {pid.Value}");

            return pid.Value;
        }

        private IList<string> GetECUs()
        {
            var ecus = new List<string>();
            var response = Execute("0100");

            foreach (var ecu in response.Split(new[] { '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = ecu.Split(' ');
                if (parts.Length > 1)
                {
                    if (parts[0].Length == 3)
                    {
                        ecus.Add(parts[0]);
                    }
                    else
                    {
                        ecus.Add(parts[2]);
                    }
                }
            }

            foreach (var ecu in ecus.OrderBy(ecu => ecu))
            {
                CurrentEcu = ecu;
                if (Speed != null)
                    break;
            }
            return ecus;
        }
    }
}
