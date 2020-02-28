using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjectionDemo.Core;
using DependencyInjectionDemo.DataAccess;

namespace DependencyInjectionDemo.Client
{
    public class DataProcessor
    {
        private List<PersonModel> _people = new List<PersonModel>();
        private IDataAccess _db;

        public DataProcessor(IDataAccess db)
        {
            _db = db;
        }

        public List<PersonModel> People
        {

            get { return _people; }
        }

        public void GetPeopleList()
        {
            _people = _db.LoadData<PersonModel>("select * from Person");
        }


        public void AddPerson(string firstName, string lastName)
        {
            var person = new PersonModel() { FirstName = firstName, LastName = lastName };
            _db.SaveData<PersonModel>(person, "insert into Person(FirstName, LastName) values (@FirstName, @LastName)");

        }

        public void RemovePerson(PersonModel person)
        {
            _db.DeleteData<PersonModel>(person, "delete from Person where Id=@Id");
        }

        public void UpdatePerson(PersonModel person)
        {
            _db.UpdateData<PersonModel>(person, "update Person set FirstName=@FirstName, LastName=@LastName Where Id=@Id");
        }

    }

}