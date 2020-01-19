using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbieracz
{
    public class AdvertDescribe
    {
        public int AdvertId { get; set; }
        public List<Details> AdvertDetails { get; set; }
        public string Describe { get; set; }
        public string AdvertExposer { get; set; }
        public string Tel { get; set; }

    }
}
