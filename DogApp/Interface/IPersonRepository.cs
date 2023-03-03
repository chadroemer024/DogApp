using DogApp.Models;

namespace DogApp.Interface
{
    public interface IPersonRepository
    {
        ICollection<Person> GetPeople();

        Person GetPerson(int id);
        Person GetOwnerOfADog(int dogId);
        ICollection<Dog> GetAllDogsByOwner(int id);
        bool CreatePerson(Person person);
        bool UpdatePerson(Person person);
        bool DeletePerson(Person person);
        bool PersonExists(int id);
       
    }
}
