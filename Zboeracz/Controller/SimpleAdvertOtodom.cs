using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbieracz
{
    public class SimpleAdvertOtodom : ISimpleAdvert
    {
        HtmlNode RetrieveHtmlExposer(string url)
        {
            HtmlWeb hw = new HtmlWeb();
            hw.UserAgent = @"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
            HtmlDocument html = hw.Load(url);

            return html.DocumentNode.SelectNodes("//article//section").ElementAt(1).SelectNodes("//div").ElementAt(1).SelectNodes("//div").ElementAt(1);
        }

        HtmlNode RetrieveHtmlSingleAdvert(string url)
        {
            HtmlWeb hw = new HtmlWeb();
            hw.UserAgent = @"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
            HtmlDocument html = hw.Load(url);

            return html.DocumentNode.SelectSingleNode("//div[contains(@class, 'css-1qft5kt-Pe')]");
        }

        AdvertDescribe ISimpleAdvert.GetSingleAdvertInfo(string url, int advertId)
        {
            HtmlNode content = RetrieveHtmlSingleAdvert(url);
            HtmlNode exposer = RetrieveHtmlExposer(url);
            HtmlNodeCollection details = content.SelectSingleNode(".//div[contains(@class, 'css-1kgyoyz-Xt')]").SelectNodes(".//li");
            AdvertDescribe advert;

            if (content != null)
            {
                try
                {
                    advert = new AdvertDescribe();
                    advert.AdvertId = advertId;
                    advert.AdvertDetails = GetAdvertDetails(details);
                    advert.Describe = ParseHtmlSingleAdvertOtodom.ParseDescribe(content);
                    advert.AdvertExposer = ParseHtmlSingleAdvertOtodom.ParseAdvertExposer(exposer);
                    //advert.Tel = ParseHtmlSingleAdvertOtodom.ParseTel(exposer);

                    return advert;
                }
                catch
                {

                }
            }
            return null;
        }

        List<Details> GetAdvertDetails(HtmlNodeCollection details)
        {
            if (details != null)
            {
                List<Details> detailList = new List<Details>();
                Details detailPair;
                foreach (HtmlNode node in details)
                {
                    try
                    {
                        detailPair = new Details();
                        detailPair.DetailName = ParseHtmlDetailsOtodom.ParseDetailName(node);
                        detailPair.DetailValue = ParseHtmlDetailsOtodom.ParseDetailValue(node);

                        detailList.Add(detailPair);
                    }
                    catch
                    {

                    }
                }
                return detailList;
            }
            return null;
        }
    }
}
