using System;
using System.Net;
using TIH.Helpers;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using Supremes;

namespace TIH
{
    public class HistoricalDataService : IHistoricalDataService
    {
        string strProcedureName;
        DataTable dtResults;
        string[,] listOfResults, listOfWikiData;
        int wikiSection;
        private readonly string strWikiUrl = "https://en.wikipedia.org/w/api.php?action=parse&format=xml&page={0}&section={1}";

        public string[][] GetHistoricalData(string month, string day, string type)
        {
            return ArrayConverter.ToJaggedArray<string>(RetrieveHistoricalData(month, day, type));
        }

        public string[][] GetHistoricalDataFromWiki(string month, string day, string type)
        {
            return ArrayConverter.ToJaggedArray<string>(RetrieveHistoricalDataFromWiki(month, day, type));
        }

        private string [,] RetrieveHistoricalData(string historicMonth, string historicDay, string historicDataType)
        {
            switch (historicDataType)
            {
                case "Events":
                    strProcedureName = "GetEventsByDate";
                    break;
                case "Birthdays":
                    strProcedureName = "GetBirthdaysByDate";
                    break;
                case "WeddingsAndDivorces":
                    strProcedureName = "GetWeddingsAndDivorcesByDate";
                    break;
                case "Deaths":
                    strProcedureName = "GetDeathsByDate";
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
                dtResults = dbManager.ExecuteSelectQueryByProcedureName(strProcedureName, sqlQueryParameters);

                if (dtResults != null)
                {
                    listOfResults = new string[dtResults.Rows.Count, 2];

                    for (int i = 0; i < dtResults.Rows.Count; i++)
                    {
                        listOfResults[i, 0] = ((System.DateTime)(dtResults.Rows[i]).ItemArray[0]).ToString();
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

        private string [,] RetrieveHistoricalDataFromWiki(string historicMonth, string historicDay, string historicDataType)
        {
            switch(historicDataType)
            {
                case "Events":
                    wikiSection = 1;
                    break;
                case "Births":
                    wikiSection = 2;
                    break;
                case "Deaths":
                    wikiSection = 3;
                    break;
                case "Holidays":
                    wikiSection = 4;
                    break;
            }

            if(historicMonth == "january" && historicDay == "1")
            {
                wikiSection++;
            }

            try
            {
                string strFormattedWikiUrl = String.Format(strWikiUrl, historicMonth + "_" + historicDay, wikiSection);
                XmlDocument xmlDocWikiData = new XmlDocument();
                xmlDocWikiData.Load(strFormattedWikiUrl);

                var wikiData = Dcsoup.Parse(xmlDocWikiData.InnerText);
                var selectedData = wikiData.Select("li");

                listOfWikiData = new string[selectedData.Count, 2];

                if (historicDataType.Equals("Holidays"))
                {
                    for (int i = 0; i < selectedData.Count; i++)
                    {
                        listOfWikiData[i, 0] = String.Empty;
                        if (selectedData[i].Text.Contains(":"))
                        {
                            listOfWikiData[i, 1] = selectedData[i].Text.Substring(0, selectedData[i].Text.IndexOf(':') + 1);
                        }
                        else
                        {
                            listOfWikiData[i, 1] = selectedData[i].Text;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < selectedData.Count; i++)
                    {
                        listOfWikiData[i, 0] = selectedData[i].Text.Substring(0, selectedData[i].Text.IndexOf('–') - 1);
                        listOfWikiData[i, 1] = selectedData[i].Text.Substring(selectedData[i].Text.IndexOf('–') + 2);
                    }
                }
            }
            catch(WebException ex)
            {
                throw ex;
            }

            return listOfWikiData;
        }
    }
}