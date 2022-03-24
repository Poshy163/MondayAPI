using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MondayAPI
{
    internal class Program
    {
        //Provided infomation
        public const string MondayApiKey = "";
        private static readonly string FolderId = "";
        private const string Boardname = "";


        private static readonly string CreateBoardQuery = @"{""query"": ""mutation {create_board(board_name: \""" + Boardname + @"\"", board_kind: public, folder_id: " + FolderId + @") {id}  }"" }";
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Open");
            MondayLib.MondayHelper helper = new MondayLib.MondayHelper();
            string json = await helper.QueryMondayApiV2(CreateBoardQuery);

            Console.WriteLine("   ");
            Console.WriteLine(CreateBoardQuery);
            Console.WriteLine(json + "");


            dynamic jsonFile = JsonConvert.DeserializeObject(json);
            string BoardId = jsonFile["data"]["create_board"]["id"];


            string[] Querys = new[]
            {
            @"{""query"": ""mutation {create_column(board_id:"+ BoardId +@",title:\""Budgeted\"",column_type: numbers) {id}  }"" }",
            @"{""query"": ""mutation {create_column(board_id:"+ BoardId +@",title:\""Hours\"",column_type: numbers) {id}  }"" }",
            @"{""query"": ""mutation {create_column(board_id:"+ BoardId +@",title:\""Date\"",column_type: date) {id}  }"" }",
            @"{""query"": ""mutation {create_column(board_id:"+ BoardId +@",title:\""Notes\"",column_type: text) {id}  }"" }",
            @"{""query"": ""mutation {create_column(board_id:"+ BoardId +@",title:\""Person\"",column_type: people) {id}  }"" }",
            @"{""query"": ""mutation {create_column(board_id:"+ BoardId +@",title:\""Status\"",column_type: status) {id}  }"" }",
            @"{""query"": ""mutation {create_column(board_id:"+ BoardId +@",title:\""Billed\"",column_type: checkbox) {id}  }"" }",
            @"{""query"": ""mutation {create_column(board_id:"+ BoardId +@",title:\""Pulse ID\"",column_type: text) {id}  }"" }",
            };


            foreach (string Query in Querys)
            {
                MondayLib.MondayHelper helper2 = new MondayLib.MondayHelper();
                string jsonresponse = await helper2.QueryMondayApiV2(Query);
                Console.WriteLine("   ");
                Console.WriteLine(jsonresponse);
            }

        }
    }
}


namespace MondayLib
{
    public class MondayHelper
    {
        private const string MondayApiUrl = "https://api.monday.com/v2/";
        public async Task<string> QueryMondayApiV2(string query)
        {
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(query);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(MondayApiUrl);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers.Add("Authorization", MondayAPI.Program.MondayApiKey);

            using (Stream requestBody = request.GetRequestStream())
            {
                await requestBody.WriteAsync(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
