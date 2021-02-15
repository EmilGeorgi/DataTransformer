using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ApiTester
{
    public class Field
    {
        public string FieldName;
        public string FieldType;
    }
    class Program
    {
        private static string urlParameters = "";
        static void Main(string[] args)
        {
            while (true)
            {
            Console.WriteLine("Enter API Url");
            string URL = Console.ReadLine();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                List<Field> fields = new List<Field>();
                List<object> dataObjects = new List<object>();
                dataObjects.Add(response.Content.ReadAsAsync<object>().Result);
                //Make sure to add a reference to System.Net.Http.Formatting.dll
                foreach (var item in dataObjects)
                {
                    if (item.GetType() == typeof(JArray))
                    {
                        for
                    }
                    var json = item.ToString();
                    var obj = JObject.Parse(json);
                    var result = obj.Descendants()
                        .OfType<JProperty>()
                        .Select(p => new KeyValuePair<string, object>(p.Path,
                            p.Value.Type == JTokenType.Array || p.Value.Type == JTokenType.Object
                                ? null : p.Value));

                    foreach (var kvp in result)
                    { 
                        if (kvp.Value != null)
                        {
                            fields.Add(new Field() { FieldName = kvp.Key, FieldType = kvp.Value.ToString() });
                        }
                    }
                }
                var myType = ClassBuilder.CreateNewObject(fields);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            // Make any other calls using HttpClient here.

            // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
        }
    }
    }
}