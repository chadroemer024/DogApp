using DogApp.Models;

namespace DogApp.Interface
{
    public interface IDogRepository
    {
        ICollection<Dog> GetDogs();

        Dog GetDog(int dogId);
        bool CreateDog( Dog dog);
        //bool UpdateDog( Dog dog);
        bool DeleteDog(Dog dog);
        bool DeleteDogs(List<Dog> dogs);

        // ICollection<Person> GetDogByOwner(int dogId);
        bool DogExists(int id);
    }
}
