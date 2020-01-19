using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbieracz
{
    public static class ParseHtmlDetailsOlx
    {
        public static string ParseDetailName(HtmlNode node)
        {
            try
            {
                string s = node.SelectNodes(".//th").ElementAt(0).InnerText.Trim();

                return s;
            }
            catch
            {
                return "";
            }
        }

        public static string ParseDetailValue(HtmlNode node)
        {
            try
            {
                HtmlNode content = node.SelectNodes(".//strong").ElementAt(0);

                if (content.SelectSingleNode(".//a") != null)
                {
                    return content.SelectSingleNode(".//a").InnerText.Trim();
                }
                else
                {
                    return content.InnerText.Trim();
                }
            }
            catch
            {
                return "";
            }
        }


    }
}
