using CarAssist.IO;
using CarAssist.Models;
using CarAssist.Obd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace CarAssist.ViewModels
{
    public class TripViewModel : BaseNotifyPropertyChanged
    {
        bool _isRecording = false;
        public bool IsRecording
        {
            get
            {
                return _isRecording;
            }
            set
            {
                OnPropertyChanged(ref _isRecording, value);
                OnPropertyChanged("IsNotRecording");
            }
        }
        public bool IsNotRecording
        {
            get
            {
                return !IsRecording;
            }
        }
        public ObservableCollection<OBDValue> OBDValues { get; } = new ObservableCollection<OBDValue>();
        public ObservableCollection<Trip> Trips { get; } = new ObservableCollection<Trip>();
        public string PersonalPath
        {
            get
            {
                return Path.Combine(Folder.Personal, "Trip");
            }
        }

        public string SupportedPids { get; set; }
        public int SupportedPidsCount { get; set; }
        public string OBDProtocolDescription { get; set; }
        public string OBDVoltage { get; set; }
        public string VIN { get; set; }
        public string FuelType { get; set; }
        public List<string> ECUs { get; set; } = new List<string>();

        public TripViewModel()
        {
            var pid = PID.GetPID(PIDCode.Speed);
            OBDValues.Add(new OBDValue(pid.Description, pid.PIDCode));

            pid = PID.GetPID(PIDCode.CalculatedEngineLoadValue);
            OBDValues.Add(new OBDValue(pid.Description, pid.PIDCode));

            pid = PID.GetPID(PIDCode.RPM);
            OBDValues.Add(new OBDValue(pid.Description, pid.PIDCode));

            pid = PID.GetPID(PIDCode.IntakeAirTemperature);
            OBDValues.Add(new OBDValue(pid.Description, pid.PIDCode));

            pid = PID.GetPID(PIDCode.MAFAirFlowRate);
            OBDValues.Add(new OBDValue(pid.Description, pid.PIDCode));

            pid = PID.GetPID(PIDCode.FuelTankLevel);
            OBDValues.Add(new OBDValue(pid.Description, pid.PIDCode));

            if (!Directory.Exists(PersonalPath))
            {
                Directory.CreateDirectory(PersonalPath);
            }

            foreach (var fileName in Directory.EnumerateFiles(PersonalPath))
            {
                Trips.Add(Trip.FromFileToTrip(fileName));
            }
            foreach (var fileName in Directory.EnumerateFiles(Folder.Personal))
            {
                Debug.WriteLine(File.ReadAllText(fileName));

                // delete log files after 7 days
                var created = File.GetLastWriteTime(fileName);
                var timeSpan = (DateTime.Now - created);

                if(timeSpan.Days > 7)
                    File.Delete(fileName);
            }
        }
    }
}
