using CoreEngine.APIHandlers;
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
            member = new MemberEngine(http);
        }

        [Test]
        public async Task Login()
        {
            var res = await member.Login("admin", "admin");
            Assert.IsTrue(res.Succeeded);
        }
    }
}