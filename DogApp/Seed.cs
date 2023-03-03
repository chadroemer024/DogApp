


using DogApp.Data;
using DogApp.Models;

namespace DogApp
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (dataContext.People.Any()) return;

            var person = new List<Person>
            {
                new Person {
                    FirstName = "Chad",
                    LastName = "Roemer",
                    Age = 27,
                    Dogs = new List<Dog>
                    {
                        new Dog {
                            Name = "Patches",
                            Type = "Springer Spaniel"
                        },
                        new Dog {
                            Name = "Spot",
                            Type = "Springer Spaniel"
                        }
                    }
                },
                new Person {
                    FirstName = "Anna",
                    LastName = "Demore",
                    Age = 25,
                    Dogs = new List<Dog>
                    {
                        new Dog {
                            Name = "Archie",
                            Type = "German Shepard"
                        },
                        new Dog {
                            Name = "ShadowFax",
                            Type = "Golden Doodle"
                        }
                    }
                },
                new Person {
                    FirstName = "Bill",
                    LastName = "Gates",
                    Age = 75,
                    Dogs = new List<Dog>
                    {
                        new Dog {
                            Name = "Soft",
                            Type = "Golden Retriever"
                        },
                        new Dog {
                            Name = "Ware",
                            Type = "Bulldog"
                        }
                    }
                }
            };

             dataContext.People.AddRange(person);
            dataContext.SaveChanges();
        }
    }
}

        /*private static async Task SeedDogs(DataContext context)
        {
            if (context.Dogs.Any()) return;

            var dogs = new List<Dog> {
                new Dog {
                    Name = "Patches",
                    Type = "Springer Spaniel",

                },
                new Dog {
                    Name = "Spot",
                    Type = "Springer Spaniel",
                },
                new City {
                    Name = "Berlin",
                    Code = "BER"
                }
            };*/