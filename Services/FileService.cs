﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Models;
using RestSharp;
using System.Text.Json;
using IO = System.IO;
using File_Processor.Controllers;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using File_Processor.Exceptions;

namespace File_Processor.Services
{
    internal class FileService
    {
        private CategoryMergedService categoryMergedService;

        public FileService()
        {
            categoryMergedService = new CategoryMergedService();
        }
        public List<CategoryMergedModel> categorizeFile(FileModel file)
        {
            List<CategoryMergedModel> categories = categoryMergedService.getCategoryAndClassifications();
            List<CategoryMergedModel> flaggedCategories = new List<CategoryMergedModel>();
            String fileContents = readWholeFile(file.filePath);
            foreach (CategoryMergedModel c in categories)
            {
                for (int i = 0; i < c.patterns.Count; i++)
                {
                    string pattern = c.patterns[i];
                    if (c.types[i] == ((int)PatternType.Keyword))
                    {
                        //Implement capability to handle pdfs, doc, videos and whatever you can think


                        //txt implementation only
                        if (fileContents.Contains(pattern)) { flaggedCategories.Add(c); break; }
                    }
                    else
                    {
                        Regex r = new Regex(pattern);
                        Match m = r.Match(fileContents);
                        //Implement capability to handle pdfs, doc, videos and whatever you can think


                        //txt implementation only
                        if (m.Success) { flaggedCategories.Add(c); break; }
                    }
                }
            }

            return flaggedCategories;
        }
        public void deduplicationFile(FileModel file)
        {
            //Remeber To handle video deduplication (fuzzy hashing) once default deduplication works (possible use of Hash Factory Design pattern for normal vs video)
            int ind = file.filePath.LastIndexOf('\\');
            DirectoryModel directory = new DirectoryModel(file.filePath.Substring(0, ind), "");
            var filesInDirectory = IO.Directory.GetFiles(directory.directoryPath).Select(f => new IO.FileInfo(f));

            bool exists = false;
            bool useName = Properties.Settings.Default.UseFileNameDeduplication;
            bool useContent = Properties.Settings.Default.UseFileContentDeduplication;

            foreach (IO.FileInfo fileInfo in filesInDirectory)
            {
                if (useName)
                {
                    if (file.fileName.Equals(fileInfo.Name))
                    {
                        exists = true;
                        break;
                    }
                }
                if (useContent)
                {
                    if (file.fileHash.Equals(FileToHash(fileInfo.FullName)))
                    {
                        exists = true;
                        break;
                    }
                }
            }

            if (exists)
            {
                IO.File.Delete(file.filePath);
            }
        }

        //public void secureFile(FileModel file)
        //{
        //    if (Properties.Settings.Default.FileEncryption) { encryptFile(file); }
        //    if (Properties.Settings.Default.MalwareAnalysis) { malwareAnalysisOfFile(file); }
        //}

        public void encryptFile(FileModel file)
        {

        }

        public async Task<bool> malwareAnalysisOfFile(FileModel file)
        {
            if (!connectedToInternet()) { throw new NoInternetConnectionException(); }
            var options = new RestClientOptions("https://www.virustotal.com/api/v3/files/" + file.fileHash);
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("x-apikey", Properties.Settings.Default.TotalVirusAPIKey);
            var response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);
            if (response.IsSuccessful)
            {
                var responseJson = JsonSerializer.Deserialize<JsonElement>(response.Content);
                var numberOfRedFlags = responseJson.GetProperty("data").GetProperty("attributes").GetProperty("last_analysis_stats").GetProperty("malicious").GetInt32();
                Console.WriteLine(numberOfRedFlags.ToString());
                return numberOfRedFlags > 0;
            }
            return false;
        }

        private bool connectedToInternet()
        {
            string host = "www.google.com";
            Ping ping = new Ping();
            try
            {
                PingReply reply = ping.Send(host);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception e) { return false; }
        }

        public FileModel PathToFileModel(string path)
        {
            string name = path.Split('\\').Last();
            string hash = FileToHash(path);
            DateTime first = IO.File.GetCreationTime(path);
            DateTime last = IO.File.GetLastWriteTime(path);
            FileModel file = new FileModel(path, hash, name, last, first);
            Console.WriteLine(file);
            return file;
        }

        private string FileToHash(string path)
        {
            using (var md5 = MD5.Create())
            using (var stream = IO.File.OpenRead(path))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        private IEnumerable<String> readFile(string path)
        {
            return IO.File.ReadLines(path);
        }

        private String readWholeFile(string path)
        {
            return IO.File.ReadAllText(path);
        }
    }
}
