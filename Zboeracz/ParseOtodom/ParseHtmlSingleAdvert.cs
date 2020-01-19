using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbieracz
{
    public static class ParseHtmlSingleAdvertOtodom
    {
        public static string ParseDescribe(HtmlNode node)
        {
            try
            {
                HtmlNodeCollection s = node.SelectNodes(".//section[contains(@class, 'section-description')]//div//p");
                string result = "";
                foreach (HtmlNode htmlNode in s)
                    result += htmlNode.InnerText + Environment.NewLine;

                return result;
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
                //string s = node.SelectNodes(".//ul[contains(@id, 'contact_methods')]//li").ElementAt(1).SelectSingleNode(".//div[contains(@data-rel, 'phone')]").Attributes["class"].Value;
                //int pFrom = s.IndexOf("'id_raw': '") + "'id_raw': '".Length;
                //int pTo = s.LastIndexOf("'}");

                string result = "";

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
                string s = node.InnerText.Trim();

                return s;
            }
            catch
            {
                return "";
            }
        }
    }
}
