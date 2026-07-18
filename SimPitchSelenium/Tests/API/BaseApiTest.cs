using System;
using System.Net.Http;
using NUnit.Framework;
using SimPitchSelenium.Utils;

namespace SimPitchSelenium.Tests.API
{
    public abstract class BaseApiTest
    {
        protected HttpClient Client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(ConfigReader.GetApiBaseUrl())
            };
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Client?.Dispose();
        }
    }
}
