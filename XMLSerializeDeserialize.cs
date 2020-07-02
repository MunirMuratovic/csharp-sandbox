using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dotnet_sandbox
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public async Task<ResponseModel> GetResponseAsync(RequestModel requestObject)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), "192.168.1.11");

            // XML Serialization
            using (StringWriter stringwriter = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RequestModel));
                serializer.Serialize(stringwriter, requestObject);
                string serializedXml = stringwriter.ToString();
                request.Content = new StringContent(serializedXml);
            }

            // Getting response
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/xml");
            string response = await client.SendAsync(request).Result.Content.ReadAsStringAsync();

            // XML Deserialization
            ResponseModel result;
            using (TextReader reader = new StringReader(response))
            {
                var deserializer = new XmlSerializer(typeof(ResponseModel));
                result = (ResponseModel)deserializer.Deserialize(reader);
            }

            return result;
        }
    }
}
