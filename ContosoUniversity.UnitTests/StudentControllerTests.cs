using System;
using System.Data.Entity;
using ContosoUniversity.Controllers;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using Moq;
using NUnit.Framework;

namespace ContosoUniversity.UnitTests
{
    [TestFixture]
    class StudentControllerTests
    {
        [Test]
        public void Create_Saves_A_Student_Via_Context()
        {
            var student = new Student
            {
                EnrollmentDate = new DateTime(1986, 01, 23),
                FirstMidName = "Brent",
                LastName = "Sullivan"
            };

            var mockSet = new Mock<DbSet<Student>>();

            var mockContext = new Mock<SchoolContext>();
            mockContext.Setup(m => m.Students).Returns(mockSet.Object);

            var controller = new StudentsController(mockContext.Object);
            controller.Create(student);

            mockSet.Verify(m => m.Add(It.IsAny<Student>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
