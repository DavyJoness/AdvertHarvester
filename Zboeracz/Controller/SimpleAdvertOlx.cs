using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbieracz
{
    public class SimpleAdvertOlx : ISimpleAdvert
    {
        HtmlNode RetrieveHtmlExposer(string url)
        {
            HtmlWeb hw = new HtmlWeb();
            hw.UserAgent = @"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
            HtmlDocument html = hw.Load(url);

            return html.DocumentNode.SelectSingleNode("//div[contains(@id, 'offeractions')]");
        }

        HtmlNode RetrieveHtmlSingleAdvert(string url)
        {
            HtmlWeb hw = new HtmlWeb();
            hw.UserAgent = @"Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
            HtmlDocument html = hw.Load(url);

            return html.DocumentNode.SelectSingleNode("//div[contains(@id, 'offerdescription')]");
        }

        AdvertDescribe ISimpleAdvert.GetSingleAdvertInfo(string url, int advertId)
        {
            HtmlNode content = RetrieveHtmlSingleAdvert(url);
            if (content != null)
            {
                HtmlNode exposer = RetrieveHtmlExposer(url);
                HtmlNodeCollection details = content.SelectSingleNode(".//table[contains(@class, 'details fixed marginbott20 margintop5 full')]").SelectNodes(".//table[contains(@class, 'item')]");
                AdvertDescribe advert;

                if (content != null)
                {
                    try
                    {
                        advert = new AdvertDescribe();
                        advert.AdvertId = advertId;
                        advert.AdvertDetails = GetAdvertDetails(details);
                        advert.Describe = ParseHtmlSingleAdvertOlx.ParseDescribe(content);
                        advert.AdvertExposer = ParseHtmlSingleAdvertOlx.ParseAdvertExposer(exposer);
                        advert.Tel = ParseHtmlSingleAdvertOlx.ParseTel(exposer);

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
                        detailPair.DetailName = ParseHtmlDetailsOlx.ParseDetailName(node);
                        detailPair.DetailValue = ParseHtmlDetailsOlx.ParseDetailValue(node);

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
