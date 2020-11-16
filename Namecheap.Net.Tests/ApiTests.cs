using NUnit.Framework;
using System;
using System.Net;

namespace Namecheap.Net.Tests
{

    [TestFixture]
    class ApiTests
    {
        private const string ApiKey = "ThisIsATestApiKey";
        private const string UserName = "TestUser";
        private readonly IPAddress ClientIp = new(new byte[] { 127, 0, 0, 1 });
        private const string ApiUserName = "ApiCustomUser";

        private const string StandardEndPoint = "https://api.namecheap.com/xml.response";
        private const string SandboxEndPoint = "https://api.sandbox.namecheap.com/xml.response";

        /// Test input => properties with everything set (not sandbox by default)
        [Test]
        public void AllInputs_NotSandboxByDefault_NoErrors()
        {
            NamecheapApi api = new(ApiKey, UserName, ClientIp, ApiUserName);

            Assert.AreEqual(api.ApiKey, ApiKey);
            Assert.AreEqual(api.UserName, UserName);
            Assert.AreEqual(api.ClientIpAddress, ClientIp);
            Assert.AreEqual(api.ApiUserName, ApiUserName);
            Assert.AreEqual(api.ApiEndPoint, StandardEndPoint);
        }

        /// Test input => properties with everything set (not sandbox explicit)
        [Test]
        public void AllInputs_NotSandboxExplicit_NoErros()
        {
            NamecheapApi api = new(ApiKey, UserName, ClientIp, ApiUserName, false);

            Assert.AreEqual(api.ApiKey, ApiKey);
            Assert.AreEqual(api.UserName, UserName);
            Assert.AreEqual(api.ClientIpAddress, ClientIp);
            Assert.AreEqual(api.ApiUserName, ApiUserName);
            Assert.AreEqual(api.ApiEndPoint, StandardEndPoint);
        }

        /// Test input => properties with evertything set (sandbox)
        [Test]
        public void AllInputs_Sandbox_NoErrors()
        {
            NamecheapApi api = new(ApiKey, UserName, ClientIp, ApiUserName, true);

            Assert.AreEqual(api.ApiKey, ApiKey);
            Assert.AreEqual(api.UserName, UserName);
            Assert.AreEqual(api.ClientIpAddress, ClientIp);
            Assert.AreEqual(api.ApiUserName, ApiUserName);
            Assert.AreEqual(api.ApiEndPoint, SandboxEndPoint);
        }

        /// Test ApiUserName comes from UserName when not provided
        [Test]
        public void ApiUserNameFallback_NotSandbox_NoErrors()
        {
            NamecheapApi api = new(ApiKey, UserName, ClientIp);

            Assert.AreEqual(api.ApiKey, ApiKey);
            Assert.AreEqual(api.UserName, UserName);
            Assert.AreEqual(api.ClientIpAddress, ClientIp);
            Assert.AreEqual(api.ApiUserName, UserName);
            Assert.AreEqual(api.ApiEndPoint, StandardEndPoint);
        }

        /// Test exception thrown when ApiKey not provided
        [Test]
        public void ApiKeyNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                NamecheapApi api = new(null, UserName, ClientIp);
            });
        }

        /// Test exception thrown when UserName not provided
        [Test]
        public void UserNameNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                NamecheapApi api = new(ApiKey, null, ClientIp);
            });
        }

        /// Test exception thrown when ClientIP not provided
        [Test]
        public void ClientIpNull_ExceptionThrown()
        {

            Assert.Throws<ArgumentNullException>(() =>
            {
                NamecheapApi api = new(ApiKey, UserName, null);
            });
        }

    }
}
