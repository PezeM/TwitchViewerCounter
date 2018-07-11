using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using TwitchViewerCounter.Core.Constans;
using TwitchViewerCounter.Core.Models;

namespace TwitchViewerCounter.Tests
{
    [TestClass]
    public class TMIApiTest
    {
        [TestMethod]
        public void GetChatterResponseTest()
        {
            var client = new RestClient(ReguestConstans.TMIApiUrl);
            var request = new RestRequest("lirik/chatters", Method.GET);

            var response = client.Execute<TMIRequestResponse>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Assert.IsNotNull(response.Content);
            }
            else
            {
                Assert.Fail(response.StatusDescription);
            }
        }
    }
}
