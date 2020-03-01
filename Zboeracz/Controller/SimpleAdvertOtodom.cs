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
            try
            {
                HtmlWeb hw = new HtmlWeb();
                hw.UserAgent = @"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
                HtmlDocument html = hw.Load(url);

                return html.DocumentNode.SelectSingleNode(".//article").SelectNodes("section").ElementAt(1).SelectNodes("div").ElementAt(1).SelectNodes("div").ElementAt(1);
            }
            catch
            {
                return null;
            }
        }
        
        HtmlNode RetrieveHtmlSingleAdvert(string url)
        {
            HtmlWeb hw = new HtmlWeb();
            hw.UserAgent = @"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
            HtmlDocument html = hw.Load(url);

            return html.DocumentNode.SelectSingleNode(".//article").SelectNodes("div").ElementAt(2);
        }

        AdvertDescribe ISimpleAdvert.GetSingleAdvertInfo(string url, int advertId)
        {
            HtmlNode content = RetrieveHtmlSingleAdvert(url);
            HtmlNode exposer = RetrieveHtmlExposer(url);
            if (exposer != null)
            {
                HtmlNodeCollection details = content.SelectSingleNode(".//section/div").SelectNodes(".//li");
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
