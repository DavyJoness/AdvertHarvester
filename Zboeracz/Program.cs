using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zbieracz
{
    class Program
    {
        static void Main(string[] args)
        {
            string type = "";
            int searchingId = 0;

            try
            {
                //type = args[0];
                //searchingId = Convert.ToInt32(args[1]);
                type = "detal";
                searchingId = 1;
            }
            catch
            {
                Console.WriteLine("Złe parametry");
                type = "";
            }

            string url = "";
            int latestAdvId = 0;

            switch (type)
            {
                case "lista":
                    Console.WriteLine("Rozpoczęto pobieranie listy ogłoszeń");
                    while (true)
                    {
                        url = SqlAdvert.GetSearchingUrl(searchingId);
                        List<Advert> adverts = GetAdverts(url);
                        Console.WriteLine(AddAdvertsToDatabase(adverts, searchingId));
                        Thread.Sleep(4 * 60 * 1000);
                    }


                    break;

                case "detal":
                    Console.WriteLine("Rozpoczęto pobieranie informacji o pojedynczych ogłoszeniach");
                    while (true)
                    {
                        latestAdvId = SqlAdvert.GetLatestAdvertId(searchingId);
                        if (latestAdvId > 0)
                        {
                            url = SqlAdvert.GetAdvertUrlById(latestAdvId);
                            AdvertDescribe advert = GetSimpleAdvertInfo(latestAdvId, url);
                            if (advert != null)
                                Console.WriteLine(AddAdvertDetailsToDatabase(advert));
                            else
                                Console.WriteLine(DeleteAdvert(latestAdvId));
                        }

                        Thread.Sleep(2 * 60 * 1000);
                    }
                    break;

                case "":

                    break;
            }
        }

        private static string DeleteAdvert(int latestAdvId)
        {
            return SqlAdvert.DeleteAdvert(latestAdvId);
        }

        static AdvertDescribe GetSimpleAdvertInfo(int advertId, string url)
        {
            ISimpleAdvert simpleAdvert;
            AdvertDescribe advert;

            //Console.WriteLine("Rozpoczęto pobieranie danych");
            if (url.Contains("www.otodom.pl"))
            {
                simpleAdvert = new SimpleAdvertOtodom();
                advert = simpleAdvert.GetSingleAdvertInfo(url, advertId);
            }
            else
            {
                simpleAdvert = new SimpleAdvertOlx();
                advert = simpleAdvert.GetSingleAdvertInfo(url, advertId);
            }

            //Console.Write(AddSimpleAdvertInfoToDatabase(adverts));

            //Console.WriteLine("Zakończono pobieranie danych");
            return advert;
        }

        private static string AddAdvertDetailsToDatabase(AdvertDescribe advert)
        {
            string message = "";

            message += SqlAdvert.InsertAdvertDescribe(advert) + Environment.NewLine;

            foreach (Details detail in advert.AdvertDetails)
            {
                SqlAdvert.InsertAdvertDetail(detail, advert.AdvertId);
            }

            return message;
        }

        public static string AddAdvertsToDatabase(List<Advert> adverts, int searchId)
        {
            string message = "";
            foreach (Advert advert in adverts)
            {
                if (!SqlAdvert.IsAdvertExists(advert))
                {
                    message += SqlAdvert.InsertAdvert(advert, searchId) + Environment.NewLine;
                }
            }
            return message;
        }

        public static List<Advert> GetAdverts(string url)
        {
            HtmlNodeCollection content = RetrieveHtml(url);
            List<Advert> adverts = new List<Advert>();
            Advert advert;

            if (content != null)
            {
                foreach (HtmlNode node in content)
                {
                    try
                    {
                        advert = new Advert();

                        advert.Id = ParseHtml.ParseId(node);
                        advert.Category = ParseHtml.ParseCategory(node);
                        advert.Name = ParseHtml.ParseName(node);
                        advert.Url = ParseHtml.ParseUrl(node);
                        advert.Location = ParseHtml.ParseLocation(node);
                        advert.Date = ParseHtml.ParseDate(node);
                        advert.Price = ParseHtml.ParsePrice(node);
                        advert.IsPromoted = ParseHtml.ParseIsPromoted(node);

                        adverts.Add(advert);
                    }
                    catch
                    { }

                }
            }


            return adverts.OrderBy(x => x.Id).ToList();
        }

        private static HtmlNodeCollection RetrieveHtml(string url)
        {
            HtmlWeb hw = new HtmlWeb();
            hw.UserAgent = @"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
            HtmlDocument html = hw.Load(url);

            return html.GetElementbyId("offers_table").SelectNodes("//table[@summary='Ogłoszenie']");
        }

    }
}
