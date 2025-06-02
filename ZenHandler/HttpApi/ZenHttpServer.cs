using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace ZenHandler.HttpApi
{
    public class ZenHttpServer
    {
        static string recipeFolder = @"D:\Recipes"; // 실제 경로로 바꾸세요
        public ZenHttpServer()
        {

        }
        public static void Start()
        {
            if (!Directory.Exists(recipeFolder))
            {
                Directory.CreateDirectory(recipeFolder);
            }

            var listener = new HttpListener();
            listener.Prefixes.Add("http://+:8080/recipes/");
            listener.Start();
            Console.WriteLine("레시피 HTTP 서버 시작됨: http://localhost:8080/recipes/");

            while (true)
            {
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                string name = WebUtility.UrlDecode(request.RawUrl.Replace("/recipes/", ""));
                string filePath = Path.Combine(recipeFolder, name);

                if (request.HttpMethod == "GET")
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        // 레시피 목록 전송
                        var files = Directory.GetFiles(recipeFolder, "*.yml")
                            .Select(Path.GetFileName);
                        //string listJson = System.Text.Json.JsonSerializer.Serialize(files);
                        string listJson = JsonConvert.SerializeObject(files);
                        byte[] buffer = Encoding.UTF8.GetBytes(listJson);
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                    else if (File.Exists(filePath))
                    {
                        string content = File.ReadAllText(filePath);
                        byte[] buffer = Encoding.UTF8.GetBytes(content);
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                    else
                    {
                        response.StatusCode = 404;
                    }
                }
                else if (request.HttpMethod == "POST")
                {
                    //using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string content = reader.ReadToEnd();
                        File.WriteAllText(filePath, content);
                        response.StatusCode = 200;
                    }
                        
                }

                response.Close();
            }
        }

    }
}
