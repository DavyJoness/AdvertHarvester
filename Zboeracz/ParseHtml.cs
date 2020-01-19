using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Zbieracz
{
    public static class ParseHtml
    {
        public static string ParseId(HtmlNode node)
        {
            try
            {
                return node.Attributes["data-id"].Value;
            }
            catch 
            {
                return "";
            }
            
        }
        public static string ParseName(HtmlNode node)
        { 
            try
            {
                return node.SelectSingleNode(".//a[@data-cy='listing-ad-title']/strong").InnerText.Trim();
            }
            catch
            {
                return "";
            }
        }

        public static string ParseUrl(HtmlNode node)
        {
            try
            {
                return node.SelectSingleNode(".//a[contains(@class, 'detailsLink')]").Attributes["href"].Value;
            }
            catch
            {
                return "";
            }
        }
        public static string ParseLocation(HtmlNode node)
        {
            try
            {
                string s = node.SelectNodes(".//tr").ElementAt(1).SelectNodes(".//td").ElementAt(0).SelectNodes(".//span").ElementAt(0).InnerText;
                s = s.Replace("\\n", "").Replace("\\t", "").Trim();
                return s;
            }
            catch
            {
                return "";
            }
        }

        public static DateTime ParseDate(HtmlNode node)
        {
            try
            {
                string s = node.SelectNodes(".//tr").ElementAt(1).SelectNodes(".//td").ElementAt(0).SelectNodes(".//span").ElementAt(1).InnerText;
                s = s.Replace("\\n", "").Replace("\\t", "").Trim();

                int hour=0;
                int minute=0;
                int day=0;
                int month=0;
                int year=0;

                if (s.Contains("wczoraj"))
                {
                    s = s.Replace("wczoraj ", "");
                    hour = Convert.ToInt32(s.Split(':')[0]);
                    minute = Convert.ToInt32(s.Split(':')[1]);

                    DateTime yesterday = DateTime.Now.AddDays(-1);
                    day = yesterday.Day;
                    month = yesterday.Month;
                    year = yesterday.Year;
                }
                else if (s.Contains("dzisiaj"))
                {
                    s = s.Replace("dzisiaj ", "");
                    hour = Convert.ToInt32(s.Split(':')[0]);
                    minute = Convert.ToInt32(s.Split(':')[1]);

                    DateTime today = DateTime.Now;
                    day = today.Day;
                    month = today.Month;
                    year = today.Year;
                }
                else
                {
                    s = s.Replace("  ", ":").Replace(" ", ":");
                    string monthName = s.Split(':')[1];
                    day = Convert.ToInt32(s.Split(':')[0]);
                    switch (monthName)
                    {
                        case "sty":
                            month = 1;
                            break;
                        case "lut":
                            month = 2;
                            break;
                        case "mar":
                            month = 3;
                            break;
                        case "kwi":
                            month = 4;
                            break;
                        case "maj":
                            month = 5;
                            break;
                    };

                    if (DateTime.Now.Month == 1 && month == 12)
                        year = DateTime.Now.Year - 1;
                    else
                        year = DateTime.Now.Year;
                }
                

                return new DateTime(year, month, day, hour, minute, 0);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static decimal ParsePrice(HtmlNode node)
        {
            try
            {
                //string s = node.SelectSingleNode(".//p[@class, 'price']/strong").InnerText;
                string s = node.SelectNodes(".//tr").ElementAt(0).SelectNodes(".//td").ElementAt(2).SelectNodes(".//strong").ElementAt(0).InnerText;
                s = s.Replace("zł", "").Replace(" ", "");

                return Convert.ToDecimal(s);
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public static bool ParseIsPromoted(HtmlNode node)
        {
            try
            {
                HtmlNode s = node.SelectSingleNode(".//td[contains(@class, 'promoted')]");

                return s != null;
            }
            catch
            {
                return false;
            }
        }

        public static string ParseCategory(HtmlNode node)
        {
            try
            {
                string s = node.SelectNodes(".//tr").ElementAt(0).SelectNodes(".//td").ElementAt(1).SelectSingleNode(".//small").InnerText.Trim();

                return s;
            }
            catch 
            {
                return "";
            }
        }
    }
}
