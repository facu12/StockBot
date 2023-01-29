using Entities;
using StockBot.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace StockBot.Services.Implementations
{
    public class StockService : IStockService
    {
        public HttpClient Client { get; }

        public StockService(HttpClient client)
        {
            Client = client;
        }

        public Stock GetStock(string code)
        {
            using (HttpResponseMessage response = Client.GetAsync($"https://stooq.com/q/l/?s={code}&f=sd2t2ohlcv&h&e=csv").Result)
            using (HttpContent content = response.Content)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpResponseException((int)HttpStatusCode.BadGateway, "Something went wrong with the API call");

                var callResponse = content.ReadAsStringAsync().Result;

                var data = callResponse.Substring(callResponse.IndexOf(Environment.NewLine, StringComparison.Ordinal) + 2);
                var processedArray = data.Split(',');
                return new Stock()
                {
                    Symbol = processedArray[0],
                    Date = !processedArray[1].Contains("N/D") ? Convert.ToDateTime(processedArray[1]) : default,
                    Time = !processedArray[2].Contains("N/D") ? Convert.ToDateTime(processedArray[2]).TimeOfDay : default,
                    Open = !processedArray[3].Contains("N/D") ? Convert.ToDouble(processedArray[3]) : default,
                    High = !processedArray[4].Contains("N/D") ? Convert.ToDouble(processedArray[4]) : default,
                    Low = !processedArray[5].Contains("N/D") ? Convert.ToDouble(processedArray[5]) : default,
                    Close = !processedArray[6].Contains("N/D") ? Convert.ToDouble(processedArray[6]) : default,
                    Volume = !processedArray[7].Contains("N/D") ? Convert.ToDouble(processedArray[7]) : default,
                };
            }
        }

    }
}
