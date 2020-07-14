using System;
using System.IO;

namespace SoupRssScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SoupRssScrapper");
            
            (string rssPath, string outputDir) = GetPropertiesFromArgs(args);
            SoupRssScrapper.DownloadSoupImagesFromRss(rssPath, outputDir);
        }

        private static (string rssPath, string outputDir) GetPropertiesFromArgs(string[] args)
        {
            (string rssPath, string outputDir) = (null, null);

            if (args.Length > 0)
            {
                rssPath = File.Exists(args[0]) ? args[0] : throw new ArgumentException();
            }
            if (args.Length > 1)
            {
                outputDir = Directory.Exists(args[2]) ? args[2] : throw new ArgumentException();
            }

            return (rssPath, outputDir);
        }
    }
}
