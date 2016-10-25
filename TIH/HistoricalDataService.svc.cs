using System;
using System.Net;
using Supremes;
using TIH.Helpers;

namespace TIH
{
    public class HistoricalDataService : IHistoricalDataService
    {
        string eventsUrl = "http://www.brainyhistory.com/days/";
        string birthdaysUrl = "http://www.brainyhistory.com/daysbirth/birth_";
        string deathsUrl = "http://www.brainyhistory.com/daysdeath/death_";

        string sanitizedUrl = "";

        public string[][] GetHistoricalData(string month, string day, string type)
        {
            return ArrayConverter.ToJaggedArray<string>(RetrieveHistoricalData(month, day, type));
        }

        private string [,] RetrieveHistoricalData(string historicMonth, string historicDay, string historicDataType)
        {
            switch (historicDataType)
            {
                case "Events":
                    sanitizedUrl = eventsUrl + historicMonth + "_" + historicDay + ".html";
                    break;
                case "Birthdays":
                    sanitizedUrl = birthdaysUrl + historicMonth + "_" + historicDay + ".html";
                    break;
                case "Deaths":
                    sanitizedUrl = deathsUrl + historicMonth + "_" + historicDay + ".html";
                    break;
            }

            try
            {
                var doc = Dcsoup.Parse(new Uri(sanitizedUrl), 5000);
                var historySection = doc.Select("table[border=0]");
                var dataSection = ((historySection[1].Select("td")));

                string[,] listOfEvents = new string[dataSection.Count/2,2];

                int j = 0;

                for (int i = 0; i < dataSection.Count; i = i+2)
                {
                    listOfEvents[j,0] = dataSection[i].Text;
                    listOfEvents[j, 1] = dataSection[i + 1].Text;
                    j++;
                }

                return listOfEvents;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

    }
}
