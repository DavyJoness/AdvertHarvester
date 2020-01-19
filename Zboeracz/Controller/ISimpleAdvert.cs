using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbieracz
{
    interface ISimpleAdvert
    {
        //HtmlNode RetrieveHtmlSingleAdvert(string url);
        AdvertDescribe GetSingleAdvertInfo(string url, int advertId);
        //List<Details> GetAdvertDetails(HtmlNodeCollection details);
    }
}
