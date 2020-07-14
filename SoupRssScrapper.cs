using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace SoupRssScrapper
{
    public class SoupRssScrapper
    {
        private const string ReqexImageUrlPattern = @"(https?:\/\/[^\s]+?).(jpe?g|png|[tg]iff?|svg|gif)";
        private const string DefaultImagesDirectoryPath = @"SoupImages";

        public static void DownloadSoupImagesFromRss(string rssPath, string outputDir = DefaultImagesDirectoryPath)
        {
            string content = ReadRssFileContent(rssPath);

            Regex rg = new Regex(ReqexImageUrlPattern);
            MatchCollection mc = rg.Matches(content);                        
            var matchedLinks = mc.Distinct().ToList();

            int count = matchedLinks.Count();
            LogStep($"Found {count} links.");

            for(int i = 0; i < count; i++)
            {                                
                try
                {
                    LogStep($"Processing element {i} out of {count}...");
                    DownloadImage(matchedLinks[i].ToString(), outputDir);
                }
                catch
                {
                    WriteErrorLog(matchedLinks[i].ToString());
                }
            }
        }

        private static string ReadRssFileContent(string rssPath)
        {
            using (StreamReader reader = new StreamReader(rssPath))
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

        private static void DownloadImage(string url, string outputDir)
        {
            string fileName = GenerateFullFileNameFromUrl(url, outputDir);

            if (!File.Exists(fileName))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(url, fileName);
                }
            }
        }

        private static string GenerateFullFileNameFromUrl(string url, string outputDir)
        {
            var path = outputDir ?? DefaultImagesDirectoryPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Uri uri = new Uri(url);
            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            var filePath = Path.Combine(path, filename);
            return filePath;
        }

        private static void LogStep(string msg)
        {
            System.Console.WriteLine($"[] {msg}");
        }
    }
}
