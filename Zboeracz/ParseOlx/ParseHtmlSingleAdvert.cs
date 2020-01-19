using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbieracz
{
    public static class ParseHtmlSingleAdvertOlx
    {
        public static string ParseDescribe(HtmlNode node)
        {
            try
            {
                string s = node.SelectSingleNode(".//div[contains(@id, 'textContent')]").InnerText.Replace("\\n", System.Environment.NewLine).Trim();

                return s;
            }
            catch
            {
                return "";
            }
        }

        public static string ParseTel(HtmlNode node)
        {
            try
            {
                string s = node.SelectNodes(".//ul[contains(@id, 'contact_methods')]//li").ElementAt(1).SelectSingleNode(".//div[contains(@data-rel, 'phone')]").Attributes["class"].Value;
                int pFrom = s.IndexOf("'id_raw': '") + "'id_raw': '".Length;
                int pTo = s.LastIndexOf("'}");

                string result = s.Substring(pFrom, pTo - pFrom);

                return result;
            }
            catch
            {
                return "";
            }
        }

        internal static string ParseAdvertExposer(HtmlNode node)
        {
            try
            {
                string s = node.SelectSingleNode(".//div[contains(@class, 'offer-user__details ')]//h4").InnerText.Replace("\\n","").Trim();

                return s;
            }
            catch
            {
                return "";
            }
        }
    }
}
