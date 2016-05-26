using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TIH.Models;

namespace TIH
{
    [ServiceContract]
    public interface IHistoricalDataService
    {
        [OperationContract]
        List<HistoricalData> GetHistoricEvents(string month, string day);

        [OperationContract]
        List<HistoricalData> GetHistoricBirthdays(string month, string day);

        [OperationContract]
        List<HistoricalData> GetHistoricDeaths(string month, string day);
    }
}
