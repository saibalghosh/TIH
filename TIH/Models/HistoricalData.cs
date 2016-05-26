using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TIH.Models
{
    [DataContract]
    public class HistoricalData
    {
        string historicYear;
        string historicData;

        [DataMember]
        public string HistoricYear
        {
            get
            {
                return historicYear;
            }

            set
            {
                historicYear = value;
            }
        }

        [DataMember]
        public string HistoricData
        {
            get
            {
                return historicData;
            }

            set
            {
                historicData = value;
            }
        }

        public HistoricalData(string HistoricYear, string HistoricData)
        {
            this.historicYear = HistoricYear;
            this.historicData = HistoricData;
        }
    }
}
