using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
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
        public void Index_Sends_A_List_Of_All_Students_To_The_View()
        {
            var students = new List<Student>
            {
                new Student { EnrollmentDate = DateTime.Now, FirstMidName = "Brent", LastName = "Sullivan"},
                new Student { EnrollmentDate = DateTime.Now, FirstMidName = "Dave", LastName = "Wallace"},
                new Student { EnrollmentDate = DateTime.Now, FirstMidName = "Shirley", LastName = "Lane"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Student>>();
            mockSet.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(students.Provider);
            mockSet.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(students.Expression);
            mockSet.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(students.ElementType);
            mockSet.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(students.GetEnumerator());

            var mockContext = new Mock<SchoolContext>();
            mockContext.Setup(c => c.Students).Returns(mockSet.Object);

            var controller = new StudentsController(mockContext.Object);
            var model = controller.Index() as ViewResult;

            Assert.That(model.Model, Is.EqualTo(students));
        }

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

            mockSet.Verify(m => m.Add(student), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
