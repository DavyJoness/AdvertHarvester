using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using Logger;

namespace Zbieracz
{
    class Writer
    {
        public static void Write(string line)
        {
            Console.WriteLine(line);
            Log.WriteLog(line);
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            string type = "";
            int searchingId = 0;

            try
            {
#if (!DEBUG)
                type = args[0];
                searchingId = Convert.ToInt32(args[1]);
#else
                type = "detal";
                searchingId = 1;
#endif
            }
            catch
            {
                Writer.Write("Złe parametry");
                type = "";
            }

            string url;
            int latestAdvId = 0;

            switch (type)
            {
                case "lista":
                    Writer.Write("Rozpoczęto pobieranie listy ogłoszeń");

                    url = SqlAdvert.GetSearchingUrl(searchingId);
                    List<Advert> adverts = GetAdverts(url);
                    AddAdvertsToDatabase(adverts, searchingId);

                    Writer.Write("Zakończono pobieranie listy ogłoszeń");
                    break;

                case "detal":
                    Writer.Write("Rozpoczęto pobieranie informacji o pojedynczych ogłoszeniach");

                    latestAdvId = SqlAdvert.GetLatestAdvertId(searchingId);
                    if (latestAdvId > 0)
                    {
                        url = SqlAdvert.GetAdvertUrlById(latestAdvId);
                        AdvertDescribe advert = GetSimpleAdvertInfo(latestAdvId, url);
                        if (advert != null)
                            AddAdvertDetailsToDatabase(advert);
                        else
                            DeleteAdvert(latestAdvId);
                    }
                    else
                    {
                        Writer.Write("Zakończono pobieranie inforacji o ogłoszeniu: brak nowych ogłoszeń");
                    }
                    Writer.Write("Zakończono pobieranie informacji o pojedynczych ogłoszeniach");

                    break;
            }
        }

        private static void DeleteAdvert(int latestAdvId)
        {
            Log.WriteLog(SqlAdvert.DeleteAdvert(latestAdvId));
        }

        static AdvertDescribe GetSimpleAdvertInfo(int advertId, string url)
        {
            ISimpleAdvert simpleAdvert;
            AdvertDescribe advert;

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

            return advert;
        }

        private static void AddAdvertDetailsToDatabase(AdvertDescribe advert)
        {
            Log.WriteLog(SqlAdvert.InsertAdvertDescribe(advert));

            foreach (Details detail in advert.AdvertDetails)
            {
                SqlAdvert.InsertAdvertDetail(detail, advert.AdvertId);
            }

        }

        public static void AddAdvertsToDatabase(List<Advert> adverts, int searchId)
        {
            foreach (Advert advert in adverts)
            {
                if (!SqlAdvert.IsAdvertExists(advert))
                {
                    Log.WriteLog(SqlAdvert.InsertAdvert(advert, searchId));
                }
            }
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
