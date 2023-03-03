namespace DogApp.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public ICollection<Dog> Dogs { get; set; }
       // public object People { get; internal set; }
    }
}
