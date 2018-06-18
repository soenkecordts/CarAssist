using Plugin.Geolocator.Abstractions;
using System;
using System.IO;

namespace CarAssist.ViewModels
{
    public class Trip
    {
        public string VIN { get; set; }
        public DateTime? Departure { get; set; }
        public Position DeparturePosition { get; set; }
        public string FileName { get; set; }

        public static Trip FromFileToTrip(string fileName)
        {
            if (!File.Exists(fileName))
                return new Trip { FileName = Path.GetFileNameWithoutExtension(fileName) };

            using (var reader = new StreamReader(File.OpenRead(fileName)))
            {
                var first = reader.ReadLine();
                var colsFirst = first.Split(',');

                var trip = new Trip
                {
                    Departure = string.IsNullOrEmpty(colsFirst[0]) ? null : (DateTime?)DateTime.Parse(colsFirst[0]),
                    DeparturePosition = string.IsNullOrEmpty(colsFirst[1]) ? null : new Position(double.Parse(colsFirst[1]), double.Parse(colsFirst[2])),
                    VIN = colsFirst[3],
                    FileName = Path.GetFileNameWithoutExtension(fileName),
                };

                return trip;
            }
        }
    }
}
