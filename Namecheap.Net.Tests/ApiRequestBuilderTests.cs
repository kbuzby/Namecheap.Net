using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Namecheap.Net.Tests
{
    public class ApiRequestBuilderTests
    {
        NamecheapApi api;

        [SetUp]
        public void Setup()
        {
            api = new NamecheapApi(
                "testApiKey",
                "userName",
                new System.Net.IPAddress(new byte[4] { 127, 0, 0, 1 }),
                "apiUserName",
                false);
        }

        /// Test basic command param parsing
        [Test]
        public void TestBasicCommand()
        {
            TestCommandBasic command = new()
            {
                Prop = "Test",
                Prop1 = "Test2"
            };

            Uri requestUri = ApiRequestBuilder.BuildRequest(api, command);

            var queryCollection = QueryHelpers.ParseQuery(requestUri.Query);
            AssertCommonQueryParams(queryCollection);
            AssertQueryParam(queryCollection, "Prop", "Test");
            AssertQueryParam(queryCollection, "Prop1", "Test2");
        }

        /// Test query param key and value is URL encoded
        [Test]
        public void TestEncodedParam()
        {
            TestCommandBasic command = new()
            {
                Prop = "Encode This",
                Prop1 = "Also&This"
            };

            Uri requestUri = ApiRequestBuilder.BuildRequest(api, command);

            var queryCollection = QueryHelpers.ParseQuery(requestUri.Query);
            AssertCommonQueryParams(queryCollection);
            AssertQueryParam(queryCollection, "Prop", "Encode This");
            AssertQueryParam(queryCollection, "Prop1", "Also&This");
        }


        /// Test command with no params (only command name)
        [Test]
        public void TestNoParams()
        {
            TestCommandBasic command = new();

            Uri requestUri = ApiRequestBuilder.BuildRequest(api, command);

            var queryCollection = QueryHelpers.ParseQuery(requestUri.Query);
            AssertCommonQueryParams(queryCollection);
            Assert.False(queryCollection.ContainsKey("Prop"));
            Assert.False(queryCollection.ContainsKey("Prop1"));
        }

        /// Test arguement exception thrown for command without attribtue
        [Test]
        public void TestClassWithNoCommandAttr()
        {
            TestClassWithNoAttr badCommand = new();

            Assert.Throws<ArgumentException>(() =>
            {
                Uri requestUri = ApiRequestBuilder.BuildRequest(api, badCommand);
            });
        }
        private class TestClassWithNoAttr { }

        /// Test ArgumentNullException for api
        [Test]
        public void TestApiNull()
        {
            TestCommandBasic command = new();

            Assert.Throws<ArgumentNullException>(() =>
            {
                Uri requestUri = ApiRequestBuilder.BuildRequest(null, command);
            });
        }

        /// Test ArgumentNullException for command
        [Test]
        public void TestCommandNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Uri requestUri = ApiRequestBuilder.BuildRequest<object>(api, null);
            });
        }

        #region Helper 
        [NamecheapApiCommand("Test.Command")]
        private class TestCommandBasic
        {
            [QueryParam(Optional = true)]
            public string Prop { get; init; } 
            [QueryParam(Optional = true)]
            public string Prop1 { get; init; }
        }

        private static void AssertCommonQueryParams(Dictionary<string, StringValues> queryCollection) {
            AssertQueryParam(queryCollection, "ApiKey", "testApiKey");
            AssertQueryParam(queryCollection, "UserName", "userName");
            AssertQueryParam(queryCollection, "ApiUser", "apiUserName");
            AssertQueryParam(queryCollection, "ClientIp", "127.0.0.1");
            AssertQueryParam(queryCollection, "Command", "Test.Command");
        }

        private static void AssertQueryParam(Dictionary<string, StringValues> queryCollection, string key, string value)
        {
            if (queryCollection.TryGetValue(key, out StringValues actualValue))
            {
                Assert.AreEqual(value, actualValue);
            }
            else
            {
                Assert.Fail();
            }
        }
        #endregion Helper 

    }
}