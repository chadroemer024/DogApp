using DogApp.Models;

namespace DogApp.Interface
{
    public interface IDogRepository
    {
        ICollection<Dog> GetDogs();

        Dog GetDog(int dogId);
        bool CreateDog( Dog dog);
      
        bool DeleteDog(Dog dog);
        bool DeleteDogs(List<Dog> dogs);

        bool DogExists(int id);
    }
}
