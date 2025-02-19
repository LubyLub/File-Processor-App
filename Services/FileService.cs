using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Models;
using RestSharp;
using System.Text.Json;

namespace File_Processor.Services
{
    internal class FileService
    {
        public void categorizeFile(FileModel file)
        {

        }
        public void deduplicationFile(FileModel file)
        {
            //Remeber To handle video deduplication (fuzzy hashing) once default deduplication works (possible use of Hash Factory Design pattern for normal vs video

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
    }
}
