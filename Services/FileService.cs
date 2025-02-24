using System;
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
using File_Processor.Models;
using System.Security.Cryptography;

namespace File_Processor.Services
{
    internal class FileService
    {
        public void categorizeFile(FileModel file)
        {

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
            if (!connectedToInternet()) { /*Log no connection exists. Stop File Processing from occuring and rollback/reset/some protocal and warn user to either find connection or disable malware analysis*/ }
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
    }
}
