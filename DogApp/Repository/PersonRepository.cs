using DogApp.Data;
using DogApp.Interface;
using DogApp.Models;

namespace DogApp.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DataContext _context;

        public PersonRepository(DataContext context) //constructor parm my datacontext
        {
            _context = context;
        }

        public Person GetPerson(int id)
        {
            return _context.People.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Person> GetPeople()
        {
            return _context.People.OrderBy(p => p.Id).ToList();
        }

        public bool PersonExists(int personId)
        {
            return _context.People.Any(p => p.Id == personId);
        }

        public Person GetOwnerOfADog(int dogId)
        {
            return _context.Dogs.Where(d => d.Id == dogId).Select(c => c.CurrentOwner).FirstOrDefault();
        }

        public bool CreatePerson(Person person)
        {
            _context.Add(person);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); //savechanges = entityframework
            return saved > 0 ? true : false;
        }

        public bool UpdatePerson(Person person)
        {
            _context.Update(person);
            return Save();
        }

        public bool DeletePerson(Person person)
        {
            _context.Remove(person);
            return Save();
        }

        public ICollection<Dog> GetAllDogsByOwner(int id)
        {
            return _context.Dogs.Where(p => p.CurrentOwnerId == id).ToList();
        }
    }
}
