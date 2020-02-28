using System.Collections.Generic;
using DependencyInjectionDemo.Core;
using DependencyInjectionDemo.Client;
using DependencyInjectionDemo.DataAccess;
using NUnit.Framework;
using Moq;

namespace DependencyInjectionDemo.UnitTests
{
    public class DataProcessorTests
    {
        private DataProcessor _consoleDataProcessor;
        private Mock<IDataAccess> _dataAccess;

        [SetUp]
        public void Setup()
        {
            _dataAccess = new Mock<IDataAccess>();
            _consoleDataProcessor = new DataProcessor(_dataAccess.Object);
        }

        [Test]
        public void GetPeopleList_WhenCalled_ReturnPeopleList()
        {


            _dataAccess.Setup(db => db.LoadData<PersonModel>("select * from Person")).Returns(new List<PersonModel>() {
                                    new PersonModel {FirstName = "Manolo", LastName = "Rivera", Id  = 1},
                                    new PersonModel {FirstName = "Jeff", LastName = "Goldberg", Id  = 2}
                                    });

            _consoleDataProcessor.GetPeopleList();

            Assert.That(_consoleDataProcessor.People.Count == 2);
            Assert.That(_consoleDataProcessor.People[0].FirstName == "Manolo");

        }

        [Test]
        public void AddPerson_WhenCalled_SavePersonInDB()
        {
            string fName = "";
            string lName = "";
            _dataAccess.Setup(db => db.SaveData<PersonModel>(It.IsAny<PersonModel>(), It.IsAny<string>())
                                         ).Callback((PersonModel person, string sql) =>
                                         {
                                             fName = person.FirstName;
                                             lName = person.LastName;
                                         });

            _consoleDataProcessor.AddPerson("John", "Doe");

            Assert.That(fName == "John" && lName == "Doe");

        }

        [Test]
        public void RemovePerson_WhenCalled_DeletePersonFromDB()
        {

            var fakePerson = new PersonModel() { FirstName = "John", LastName = "Doe", Id = 5 };
            _consoleDataProcessor.RemovePerson(fakePerson);

            _dataAccess.Verify(db => db.DeleteData(fakePerson, It.IsAny<string>()), Times.Once());

        }

        [Test]
        public void UpdatePerson_WhenCalled_UpdatePersonInDB()
        {

            var fakePerson = new PersonModel() { FirstName = "John", LastName = "Doe", Id = 5 };
            _consoleDataProcessor.UpdatePerson(fakePerson);

            _dataAccess.Verify(db => db.UpdateData(fakePerson, It.IsAny<string>()), Times.Once());

        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _consoleDataProcessor = null;
        }
    }
}