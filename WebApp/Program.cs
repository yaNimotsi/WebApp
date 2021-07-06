using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationLesson1
{
    class Program
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string FilePath = @"C:\temp\result.txt";
        static void Main()
        {
            CreateAndSendRequest();
            Console.WriteLine("i think the task was completed");
            Console.ReadLine();
        }

        private static async void CreateAndSendRequest()
        {
            for (var i = 4; i <= 13; i++)
            {
                try
                {
                    var httpRequest = new HttpRequestMessage(HttpMethod.Get,
                        $"https://jsonplaceholder.typicode.com/posts/{i}");

                    var response = await HttpClient.SendAsync(httpRequest);

                    response.EnsureSuccessStatusCode();

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (i < 13) responseString += "\r\r";

                    await WriteTextAsync(responseString);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        static async Task WriteTextAsync(string textToWriteInFile)
        {
            var encodedText = Encoding.UTF8.GetBytes(textToWriteInFile);

            await using (var sourceStream = new FileStream(FilePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            }
        }
    }
}