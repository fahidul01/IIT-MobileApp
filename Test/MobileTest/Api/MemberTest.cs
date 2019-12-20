﻿using CoreEngine.APIHandlers;
using Mobile.Core.Engines.APIHandlers;
using Mobile.Core.Worker;
using MobileTest.Core;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MobileTest.Api
{

    public class MemberTest
    {
        private IMemberHandler member;

        [SetUp]
        public void SetUp()
        {
            var http = new HttpWorker(TestConstants.WebAddress);
            //member = new MemberEngine(http);
        }

        [Test]
        public async Task Login()
        {
            var res = await member.Login("181909", "12345678");
            Assert.IsTrue(res.Success);

            var response = await member.TouchLogin();
            Assert.NotNull(response);

            member.Logout();
            await Task.Delay(250);
            response = await member.TouchLogin();
            Assert.Null(response);
        }
    }
}