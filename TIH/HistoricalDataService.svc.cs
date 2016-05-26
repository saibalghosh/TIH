using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net;
using TIH.Models;
using Supremes;

namespace TIH
{
    public class HistoricalDataService : IHistoricalDataService
    {
        string eventsUrl = "http://www.brainyhistory.com/days/";
        string birthdaysUrl = "http://www.brainyhistory.com/daysbirth/birth_";
        string deathsUrl = "http://www.brainyhistory.com/daysdeath/death_";

        public List<HistoricalData> GetHistoricEvents(string month, string day)
        {
            return RetrieveHistoricalData(month, day, "Events");
        }

        public List<HistoricalData> GetHistoricBirthdays(string month, string day)
        {
            return RetrieveHistoricalData(month, day, "Birthdays");
        }

        public List<HistoricalData> GetHistoricDeaths(string month, string day)
        {
            return RetrieveHistoricalData(month, day, "Deaths");
        }

        private List<HistoricalData> RetrieveHistoricalData(string historicMonth, string historicDay, string dataType)
        {
            var sanitizedUrl = "";
            switch (dataType)
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
                    //case default:
                    //    throw new NotImplementedException("No such method");
            }

            try
            {
                var doc = Dcsoup.Parse(new Uri(sanitizedUrl), 5000);
                var historySection = doc.Select("table[border=0]");
                var dataSection = ((historySection[1].Select("td")));

                List<HistoricalData> listOfEvents = new List<HistoricalData>();

                for (int i = 0; i < dataSection.Count; i = i + 2)
                {
                    listOfEvents.Add(new HistoricalData(dataSection[i].Text, ((Supremes.Nodes.Element)(dataSection[i].SiblingNodes[1])).Text));
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
