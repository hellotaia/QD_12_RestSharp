using NUnit.Framework;
using NUnit.Framework.Internal;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace QD_12_RestSharp.StepDefinitions
{
    public class ApiHelper
    {

        private RestClient client;
        private RestRequest request;
        private RestResponse response;

        public ApiHelper()
        {
            client = new RestClient();
        }

        private Method GetMethod(string method)
        {
            switch (method)
            {
                case "GET":
                    return Method.Get;
                case "POST":
                    return Method.Post;
                case "PUT":
                    return Method.Put;
                case "DELETE":
                    return Method.Delete;
                case "PATCH":
                    return Method.Patch;
                default:
                    throw new ArgumentException($"Invalid method: {method}");
            }
        }

        public void SetClient(RestClient restClient)
        {
            client = restClient;
        }
        public RestResponse GetResponse()
        {
            return response;
        }

        public ApiHelper CallWithoutData(string method, string url)
        {
            request = new RestRequest(url, GetMethod(method));
            response = client.Execute(request);

            Console.WriteLine($"//Log Request: {request.Method}" +
                $" - {client.BuildUri(request)}");
            return this;
        }

        public void CallWithData(string method, string url, Table table)
        {
            request = new RestRequest(url, GetMethod(method));
            foreach (var row in table.Rows)
            {
                string key = row["key"];
                string value = row["value"];
                request.AddParameter(key, value);
            }

            response = client.Execute(request);

            Console.WriteLine($"//Log Request: {request.Method}" +
                $" - {client.BuildUri(request)}");
        }

        public ApiHelper VerifyData (Table table)
        {
            foreach (var row in table.Rows)
            {
                string key = row["key"];
                string expectedValue = row["value"];

                Assert.IsTrue(response.Content.Contains($"\"{key}\":\"{expectedValue}\""),
                    $"Expected key-value pair not found in the response. Key: {key}, Value: {expectedValue}");
            }
            return this;
        }

        public ApiHelper Calculations (string operation, string expression)
        {
            request = new RestRequest();

            if (operation.Equals("SquareRoot"))
            {
                request.Method = Method.Get;
                request.Resource = $"?expr={expression}";
            }
            else
            {
                request.AddHeader("User-Agent", "Learning RestSharp");
                request.AddParameter("expr", expression);
            }

            response = client.Execute(request);
            return this;
        }
    }
}
