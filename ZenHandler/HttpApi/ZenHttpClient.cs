using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace ZenHandler.HttpApi
{
    public class ZenHttpClient
    {
        private static readonly HttpClient client = new HttpClient();
        static string server = "http://PC1_IP:8080/recipes"; // PC1의 IP로 수정

        public static async Task<List<string>> GetRecipeList()
        {
            var response = await client.GetAsync(server + "/");
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(json);
        }

        public static async Task<string> GetRecipeContent(string name)
        {
            var response = await client.GetAsync($"{server}/{name}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            return null;
        }

        public static async Task<bool> SaveRecipeContent(string name, string content)
        {
            var response = await client.PostAsync(
                $"{server}/{name}",
                new StringContent(content, Encoding.UTF8, "text/plain"));
            return response.IsSuccessStatusCode;
        }
    }
}
