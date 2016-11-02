using System;
using System.Net;
using TIH.Helpers;
using System.Data;
using System.Data.SqlClient;

namespace TIH
{
    public class HistoricalDataService : IHistoricalDataService
    {
        string strSqlQuery;
        DataTable dtResults;
        string[,] listOfResults;

        public string[][] GetHistoricalData(string month, string day, string type)
        {
            return ArrayConverter.ToJaggedArray<string>(RetrieveHistoricalData(month, day, type));
        }

        private string [,] RetrieveHistoricalData(string historicMonth, string historicDay, string historicDataType)
        {
            switch (historicDataType)
            {
                case "Events":
                    strSqlQuery = "SELECT Epoch, Event FROM Events WHERE DATEPART(MM, Epoch) = @month AND DATEPART(dd, Epoch) =@day";
                    break;
                case "Birthdays":
                    strSqlQuery = "SELECT Epoch, Birthday FROM Birthdays WHERE DATEPART(MM, Epoch) = @month AND DATEPART(dd, Epoch) =@day";
                    break;
                case "WeddingsAndDivorces":
                    strSqlQuery = "SELECT Epoch, WeddingOrDivorce FROM WeddingsAndDivorces WHERE DATEPART(MM, Epoch) = @month AND DATEPART(dd, Epoch) =@day";
                    break;
                case "Deaths":
                    strSqlQuery = "SELECT Epoch, Death FROM Deaths WHERE DATEPART(MM, Epoch) = @month AND DATEPART(dd, Epoch) =@day";
                    break;
            }

            try
            {
                SqlParameter[]  sqlQueryParameters =
                                {
                                    new SqlParameter("@month", historicMonth),
                                    new SqlParameter("@day", historicDay)
                                };

                DatabaseManager dbManager = new DatabaseManager();
                dtResults = dbManager.ExecuteSelectQuery(strSqlQuery, sqlQueryParameters);

                if (dtResults != null)
                {
                    listOfResults = new string[dtResults.Rows.Count, 2];

                    for (int i = 0; i < dtResults.Rows.Count; i++)
                    {
                        listOfResults[i, 0] = ((System.DateTime)(dtResults.Rows[i]).ItemArray[0]).Date.Year.ToString();
                        listOfResults[i, 1] = (String)(dtResults.Rows[i]).ItemArray[1];
                    }
                }
                else
                {
                    throw new Exception("Error retrieving data from the database.");
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            return listOfResults;
        }
    }
}