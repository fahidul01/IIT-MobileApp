using CoreEngine.APIHandlers;
using Mobile.Core.Engines.APIHandlers;
using Mobile.Core.Worker;
using MobileTest.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobileTest.Api
{
    public class CourseTest
    {
        private IMemberHandler member;
        private ICourseHandler courseHandler;

        [SetUp]
        public async Task SetUp()
        {
            var http = new HttpWorker(TestConstants.WebAddress);
            member = new MemberEngine(http);
            courseHandler = new CourseEngine(http);
            await member.Login("181909", "qbQ890ZC");
        }

        [Test]
        public async Task AddCourse()
        {
            var semesters = await courseHandler.GetCurrentSemester();
            Assert.IsTrue(semesters.Count > 0);
        }
    }
}
