using NUnit.Framework;
using RestSharp;
using System.Net;

namespace QD_12_RestSharp.StepDefinitions
{
    [Binding]
    public class StepDefinitions
    {
        private RestClient client;
        private ApiHelper apiHelper;

        public StepDefinitions()
        {
            client = new RestClient();
            apiHelper = new ApiHelper();
            apiHelper.SetClient(client);
        }

        [Given(@"a base URL is (.*)")]
        public void GivenAUserNavigatesToPage(string baseUrl)
        {
            client = new RestClient(baseUrl);
            apiHelper.SetClient(client);
        }


        [When(@"(?i)(get|post|delete) call executed to (.*)")]
        public void APIcallWithoutData(string method, string url)
        {
            apiHelper.CallWithoutData(method, url);
        }

        [When(@"(.*) is executed to (.*) using")]
        public void APIcallWithData(string method, string url, Table table)
        {
            apiHelper.CallWithData(method, url, table);
        }

        [Then(@"the status code is (.*)")]
        public void ThenTheStatusCodeIs(int expectedCode)
        {
            int actualCode = (int)apiHelper.GetResponse().StatusCode;

            Console.WriteLine($"//Log: Response Content: {apiHelper.GetResponse().Content}");
            Assert.AreEqual(expectedCode, actualCode,
                $"Actual status: {actualCode}, you expected to get {expectedCode}");
        }

        [Then(@"the following fields and values are in the response")]
        public void VerifyFieldsAndValuesInResponse(Table table)
        {
            apiHelper.VerifyData(table);
        }

        [Then(@"the (.*) is in the response")]
        public void ThenTheIsInTheResponse(string value)
        {
            Assert.AreEqual(value, apiHelper.GetResponse().Content, 
                $"The actual answer is {apiHelper.GetResponse().Content} and not a expected \"{value}\"");
        }

        //math step
        [When(@"(.*) action is executed with (.*)")]
        public void WhenAUserPerformsOpetation(string operation, string expression)
        {
            apiHelper.Calculations(operation, expression);
        }
    }
}