namespace DogApp.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public int CurrentOwnerId { get; set; }
        public Person CurrentOwner { get; set; }



    }
}
