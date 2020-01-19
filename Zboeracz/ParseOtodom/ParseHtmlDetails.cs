using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbieracz
{
    public static class ParseHtmlDetailsOtodom
    {
        public static string ParseDetailName(HtmlNode node)
        {
            try
            {
                string s = node.InnerText.Trim();

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
                string content = node.SelectSingleNode(".//strong").InnerText.Trim();
                return content;
            }
            catch
            {
                return "";
            }
        }


    }
}
