using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace SoupRssScrapper
{
    public class SoupRssScrapper : IRssScrapper
    {
        private const string ReqexImageUrlPattern = @"(https?:\/\/[^\s]+?).(jpe?g|png|[tg]iff?|svg|gif)";
        private const string RssPath = @"C:\Users\tnowakow\Documents\_Private\SoupRssScrapper\soup.rss";
        private const string ImagesDirectoryPath = @"SoupImages";

        public static void DownloadSoupImagesFromRss()
        {
            string content = ReadRssFileContent();

            Regex rg = new Regex(ReqexImageUrlPattern);
            MatchCollection matchedLinks = rg.Matches(content);

            System.Console.WriteLine("count: " + matchedLinks.Count);
            foreach (var imageLink in matchedLinks)
            {
                try
                {
                    DownloadImage(imageLink.ToString());
                }
                catch
                {
                    WriteErrorLog(imageLink.ToString());
                }
            }
        }

        private static string ReadRssFileContent()
        {
            using (StreamReader reader = new StreamReader(RssPath))
            {
                return reader.ReadToEnd();
            }
        }

        private static void WriteErrorLog(string msg)
        {
            using(StreamWriter writer = File.AppendText("rssscrapper-errors.log"))
            {
                writer.WriteLine(msg);
            }
        }

        private static void DownloadImage(string url)
        {
            string fileName = GenerateFullFileNameFromUrl(url);

            if (!File.Exists(fileName))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(url, fileName);
                }
            }
        }

        private static string GenerateFullFileNameFromUrl(string url)
        {
            var path = ImagesDirectoryPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Uri uri = new Uri(url);
            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            var filePath = Path.Combine(path, filename);
            return filePath;
        }
    }
}
