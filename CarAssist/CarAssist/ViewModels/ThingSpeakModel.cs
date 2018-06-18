using System.Collections.Generic;

namespace CarAssist.ViewModels
{
    public class ThingSpeakModel
    {
        public string write_api_key { get; set; }
        public IList<ThingSpeakOBD> updates { get; set; }
    }

    public class ThingSpeakOBD
    {
        public string created_at { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        //public string lat { get; set; }           werden bei Bulk_Update.json nicht verwendet und führen zu Fehlermeldunh
        //public string lon { get; set; }
    }
}
