using System;
using NUnit.Framework;
using RestSharp;

namespace MultiTenantWebApi.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestDefault()
        {
            var client = new RestClient("http://localhost:49965");

            var request = new RestRequest("api/hello", Method.GET);

            // Act
            var response = client.Execute(request);
            var content = response.Content;

            // Assert
            Assert.AreEqual("\"SharedLib says Hi\"", content);
        }

        [Test]
        public void TestA()
        {
            var client = new RestClient("http://localhost:49965");

            var request = new RestRequest("api/hello", Method.GET);
            request.AddQueryParameter("tenant", "A");

            // Act
            var response = client.Execute(request);
            var content = response.Content;

            // Assert
            var expected = "\"PlugInA says Hi\"";
            Assert.AreEqual(expected, content);
        }

        [Test]
        public void TestB()
        {
            var client = new RestClient("http://localhost:49965");

            var request = new RestRequest("api/hello", Method.GET);
            request.AddQueryParameter("tenant", "B");

            // Act
            var response = client.Execute(request);
            var content = response.Content;

            // Assert
            Assert.AreEqual("\"PlugInB says Hi\"", content);
        }
    }
}
