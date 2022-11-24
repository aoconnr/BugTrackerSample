using AireBugTracker.Data;
using AireBugTracker.Shared;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AireBugTrackerTests
{
    internal static class ContextSetup
    {

        /// <summary>
        /// Generates mock DBContext with simple test data
        /// </summary>
        /// <returns></returns>
        public static DatabaseContext GenerateMockContext()
        {
            var bugData = new List<Bug>
            {
                new Bug{Id = 1, Title = "Bug 1", Description =  "Sample Bug 1", OpenedDate = DateTime.Now },
                new Bug{Id = 2, Title = "Bug 2", Description =  "Sample Bug two", OpenedDate = DateTime.Now },
                new Bug{Id = 3, Title = "Close Bug", Description =  "closed bug description", OpenedDate = DateTime.Now, ClosedDate = DateTime.Now },
            }.AsQueryable();
            var bugSet = new Mock<DbSet<Bug>>();
            bugSet.As<IQueryable<Bug>>().Setup(m => m.Provider).Returns(bugData.Provider);
            bugSet.As<IQueryable<Bug>>().Setup(m => m.Expression).Returns(bugData.Expression);
            bugSet.As<IQueryable<Bug>>().Setup(m => m.ElementType).Returns(bugData.ElementType);
            bugSet.As<IQueryable<Bug>>().Setup(m => m.GetEnumerator()).Returns(() => bugData.GetEnumerator());
            bugSet.As<IQueryable<Bug>>().Setup(m => m.ElementType).Returns(bugData.ElementType);
            bugSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => bugData.FirstOrDefault(d => d.Id == (int)ids[0]));
            
            var userData = new List<User>
            {
                new User{Id = 1, Name = "Andrew"},
                new User{Id = 2, Name = "Dave"},
                new User{Id = 3, Name = "Paul"},
            }.AsQueryable();
            var userSet = new Mock<DbSet<User>>();
            userSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userData.Provider);
            userSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userData.Expression);
            userSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            userSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => userData.GetEnumerator());
            userSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => userData.FirstOrDefault(d => d.Id == (int)ids[0]));

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(c => c.Bugs).Returns(bugSet.Object);
            mockContext.Setup(c => c.Users).Returns(userSet.Object);

            return mockContext.Object;
        }

    }
}
