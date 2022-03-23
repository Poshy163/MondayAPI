using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MondayAPI
{
    internal class Program
    {

		static string BoardId = "";
		static string FolderId = "7350320"; // GET GIVIEN 

		static string query = @"{""query"": ""mutation {create_board(board_name: \""Testboard\"", board_kind: public, folder_id: " + FolderId + @") {id}  }"" }";
		
	
		

        static async Task Main(string[] args)
        {
            Console.WriteLine("Open");

			var helper = new Monday.lib.MondayHelper();
			string json = await helper.QueryMondayApiV2(query);
			Console.WriteLine("   ");
			Console.WriteLine(BoardId);
			Console.WriteLine(query);
			Console.WriteLine(json + "");


			dynamic jsonFile = JsonConvert.DeserializeObject(json);
			BoardId = jsonFile["data"]["create_board"]["id"];

			Thread.Sleep(3000);

			var Querys = new[]
			  {
			@"{""query"": ""mutation {create_item(board_id:"+BoardId+@",item_name: \""adding works\"", ) {id}  }"" }",
			@"{""query"": ""mutation {create_item(board_id:"+BoardId+@",item_name: \""Ahhhhh\"") {id}  }"" }"
			};

			foreach (var Query in Querys)
			{
				var helper2 = new Monday.lib.MondayHelper();
				string json2 = await helper2.QueryMondayApiV2(Query);
				Console.WriteLine("   ");
				Console.WriteLine(json2 + "");
			}


		}


		

	}

		
}


namespace Monday.lib
{
	public class MondayHelper
	{
		private const string MondayApiKey = "eyJhbGciOiJIUzI1NiJ9.eyJ0aWQiOjE1MTA1MTQ0MiwidWlkIjoyODkzMzM3MiwiaWFkIjoiMjAyMi0wMy0xNlQxMDowMTozMS40MDlaIiwicGVyIjoibWU6d3JpdGUiLCJhY3RpZCI6MTE1NjkyNzcsInJnbiI6InVzZTEifQ.O06o90tglIW4a7HxFl8o8vhY_WFplOGtmFIMcNhKqks";
		private const string MondayApiUrl = "https://api.monday.com/v2/";


		/// <summary>
		/// Get a JSON response from the Monday.com V2 API.
		/// </summary>
		/// <param name="query">GraphQL Query to apply to the Monday.com production instance for Grange.</param>
		/// <returns>JSON response of query results.</returns>
		/// <remarks>
		/// Query must be in JSON,
		///		e.g. = "{\"query\": \"{boards(ids: 1234) {id name}}\"}"
		/// </remarks>
		public async Task<string> QueryMondayApiV2(string query)
		{
			byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(query);

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(MondayApiUrl);
			request.ContentType = "application/json";
			request.Method = "POST";
			request.Headers.Add("Authorization", MondayApiKey);

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
