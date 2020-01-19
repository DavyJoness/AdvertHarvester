using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbieracz
{
    class Program
    {
        static void Main(string[] args)
        {
            //string url = @"https://www.olx.pl/nieruchomosci/mieszkania/wynajem/poznan/?search%5Bfilter_float_price%3Ato%5D=1400&search%5Bfilter_enum_rooms%5D%5B0%5D=one&search%5Bprivate_business%5D=private&search%5Bdist%5D=2&search%5Bdistrict_id%5D=713";
            //string url = @"https://www.olx.pl/oferta/kawalerka-na-wildzie-CID3-IDCRSk8.html#48a65522fe";
            //int advertId = 52;
            string url = @"https://www.otodom.pl/oferta/kawalerka-bezposrednio-w-centrum-na-kraszewskiego-ID2R7UH.html?";
            int advertId = 50;

            GetSimpleAdvertInfo(advertId, url);

            Console.ReadKey();
        }

        static void GetSimpleAdvertInfo(int advertId, string url)
        {
            ISimpleAdvert simpleAdvert;
            AdvertDescribe advert;

            Console.WriteLine("Rozpoczęto pobieranie danych");
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

            Console.WriteLine("Zakończono pobieranie danych");
        }

        public static string AddAdvertsToDatabase(List<Advert> adverts)
        {
            string message = "";
            foreach (Advert advert in adverts)
            {
                if (!SqlAdvert.IsAdvertExists(advert))
                {
                    message += SqlAdvert.InsertAdvert(advert) + Environment.NewLine;
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
