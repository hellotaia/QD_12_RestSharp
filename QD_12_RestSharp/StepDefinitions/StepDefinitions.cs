using NUnit.Framework;
using RestSharp;

namespace QD_12_RestSharp.StepDefinitions
{
    [Binding]
    public class StepDefinitions
    {
        public RestClient SetUrl(string baseUrl, string endpoint)
        {
            string url = Path.Combine(baseUrl, endpoint);
            RestClient restClient = new RestClient(url);
            return restClient;
        }

        private RestClient client;
        private RestRequest request;
        private RestResponse response;

        public StepDefinitions()
        {
            client = new RestClient("https://reqres.in/api");
        }

        [Given(@"a User navigates to page (.*)")]
        public void GivenAUserNavigatesToPage(string baseUrl)
        {
            client = new RestClient(baseUrl);
        }

        [When(@"a User executes a (?i)(get|post|put|delete|patch) call to (.*)")]
        public void WhenAUserExecutesAGETCallToUsersPage(string method, string url)
        {
            request = new RestRequest(url, GetMethod(method));
            response = client.Execute(request);

            Console.WriteLine($"//Log Request: {request.Method}" +
                $" - {client.BuildUri(request)}");
        }

        [When(@"a User executes a (?i)(get|post|put|delete|patch) call to (.*) using")]
        public void PostWithBody(string method, string url, Table table)
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

        [Then(@"the status code is (.*)")]
        public void ThenTheStatusCodeIs(int expectedCode)
        {
            int actualCode = (int)response.StatusCode;

            Console.WriteLine($"//Log: Response Content: {response.Content}");

            Assert.AreEqual(expectedCode, actualCode,
                $"Actual status: {actualCode}, you expected to get {expectedCode}");
        }

        [Then(@"the following fields and values are in the response")]
        public void VerifyFieldsAndValuesInResponse(Table table)
        {
            foreach (var row in table.Rows)
            {
                string key = row["key"];
                string expectedValue = row["value"];

                Assert.IsTrue(response.Content.Contains($"\"{key}\":\"{expectedValue}\""),
                    $"Expected key-value pair not found in the response. Key: {key}, Value: {expectedValue}");
            }
        }

        [Then(@"the (.*) is in the response")]
        public void ThenTheIsInTheResponse(string value)
        {
            Assert.AreEqual(value, response.Content, 
                $"The actual answer is {response.Content} and not a expected \"{value}\"");
        }

        //math step
        [When(@"a User performs (.*) with (.*)")]
        public void WhenAUserPerformsOpetationWithAnd(string operation, string expression)
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
    }
}