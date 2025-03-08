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
            FileProcessor fileProcessor = FileProcessorFactory.getProcessor(file.extension);
            return fileProcessor.categorizeFile(file);
        }
        public bool deduplicationFile(FileModel file)
        {
            FileProcessor fileProcessor = FileProcessorFactory.getProcessor(file.extension);
            return fileProcessor.deduplicationFile(file);
        }

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

        public FileModel PathToFileModel(string path, String extension)
        {
            string name = path.Split('\\').Last();
            string hash = (FileProcessorFactory.getProcessor(extension)).FileToHash(path);
            DateTime first = IO.File.GetCreationTime(path);
            DateTime last = IO.File.GetLastWriteTime(path);
            FileModel file = new FileModel(path, hash, name, extension, last, first);
            Console.WriteLine(file);
            return file;
        }
    }
}
