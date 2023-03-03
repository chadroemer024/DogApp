using DogApp.Data;
using DogApp.Interface;
using DogApp.Models;

namespace DogApp.Repository
{
    public class DogRepository : IDogRepository
    {
        private readonly DataContext _context;

        public DogRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateDog( Dog dog)
        {
            /*ar personEntity = _context.People.Where(p => p.Id == personId).FirstOrDefault();
            var dogOwner = new Person()
            {
                Dogs = dog,

            };*/
            //_context.Add(dogOwner);

            _context.Add(dog);
            return Save();
        }

        public bool DeleteDog(Dog dog)
        {
            _context.Remove(dog);
            return Save();
        }

        public bool DeleteDogs(List<Dog> dogs)
        {
            _context.RemoveRange(dogs);
            return Save();
        }

        public bool DogExists(int dogId)
        {
            return _context.Dogs.Any(d => d.Id == dogId);
        }

        public Dog GetDog(int dogId)
        {
            return _context.Dogs.Where(d => d.Id == dogId).FirstOrDefault();
        }

        /*public ICollection<Person> GetDogByOwner(int personId)
        {
            return _context.Dogs.Where(o => o.CurrentOwnerId == personId).Select(c => c.CurrentOwner).ToList();
        }*/

        public ICollection<Dog> GetDogs() //simple list of all dogs, display ToList
        {
            return _context.Dogs.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); //savechanges = entityframework
            return saved > 0 ? true : false;
        }

        /*public bool UpdateDog( Dog dog)
        {
            _context.Update(dog);
            return Save();
        }*/
    }
}
