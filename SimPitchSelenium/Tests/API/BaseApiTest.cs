using System;
using System.Net.Http;
using NUnit.Framework;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests.API
{
    public abstract class BaseApiTest
    {
        protected HttpClient Client;

        [SetUp]
        public void SetUp()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(ConfigReader.GetApiBaseUrl())
            };
        }

        [TearDown]
        public void TearDown()
        {
            Client?.Dispose();
        }
    }
}
