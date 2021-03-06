﻿using System.ServiceModel;

namespace TIH
{
    [ServiceContract]
    public interface IHistoricalDataService
    {
        [OperationContract]
        string[][] GetHistoricalData(string month, string day, string type);
        [OperationContract]
        string[][] GetHistoricalDataFromWiki(string month, string day, string type);
    }
}
