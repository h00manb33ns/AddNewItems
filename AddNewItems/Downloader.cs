using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AddNewItems
{
    class Downloader
    {
        public string DownloadXmlHappyGifts(string url)
        {

            string xml;
            using (var webClient = new WebClient())
            {
                // webClient.Encoding = Encoding.UTF8;
                xml = webClient.DownloadString(url);
            }
            return xml;

        }
    }
}
