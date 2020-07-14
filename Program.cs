using System;
using System.Text;
using System.Xml;
using Microsoft.Toolkit.Parsers.Rss;

namespace SoupRssScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SoupRssScrapper");
            SoupRssScrapper.DownloadSoupImagesFromRss();
        }
    }
}
