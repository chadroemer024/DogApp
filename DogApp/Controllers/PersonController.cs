using AutoMapper;
using DogApp.DTO;
using DogApp.Interface;
using DogApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace DogApp.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller //inherit controller
    {
        private readonly IPersonRepository _personRepository;
        private readonly IDogRepository _dogRepository;
        private readonly IMapper _mapper; //rename variables inside constructor, format standard

        public PersonController(IPersonRepository personRepository,
            IDogRepository dogRepository,
            IMapper mapper) //constructor created
        {
            _personRepository = personRepository;
            _dogRepository = dogRepository;
            _mapper = mapper;
        }

        [HttpGet ("Show all people listed")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Person>))] //type what kind of GET you want

        public IActionResult GetPeople() //this method, when called, will simply display the list of people in my Person table. No parm needed.
        {
            var people = _mapper.Map<List<PersonDTO>>(_personRepository.GetPeople()); //create variable to automap all of the fields I want to see (i made personDTO to limit the data shown).

            if(!ModelState.IsValid) //if badrequest, please tell me :)
                return BadRequest(ModelState);
            return Ok(people); //else everything is fine, return the var
        }

        [HttpGet("Look up a specific person")] //{personId}
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(400)]
        public IActionResult GetPerson(int personId) //this method, when called, will display the specific personID entered.
        {
            if (!_personRepository.PersonExists(personId)) //if no data found in personId after action, not found
                return NotFound();

            var person = _mapper.Map<PersonDTO>(_personRepository.GetPerson(personId)); //variable to autoMap data from the found ID to display DTO fields

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(person);
        }

        [HttpGet("Get Owner by Dog ID")] ///dogs/{dogId}
        [ProducesResponseType(200, Type = typeof(Person))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerOfADog(int dogId) //this method, when called, will display the specific personID entered.
        {
            var dog = _mapper.Map<PersonDTO>(_personRepository.GetOwnerOfADog(dogId)); //variable to autoMap data from the found ID to display DTO fields

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(dog);
        }

        [HttpGet("Get All Dogs by Owner")] ///dogs/{dogId}
        [ProducesResponseType(200, Type = typeof(IEnumerable<Dog>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllDogsByOwner(int personId) //this method, when called, will display the specific personID entered.
        {
            if (!_personRepository.PersonExists(personId)) //if no data found in personId after action, not found
               return NotFound();

            var dog = _mapper.Map<List<DogDTO>>(_personRepository.GetAllDogsByOwner(personId)); //variable to autoMap data from the found ID to display DTO fields

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(dog);
        }

        [HttpPost("Add a dog owner")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)] //FromBody = takes the entire CategoryDTO JSON encapsulation of data
        public IActionResult CreatePerson([FromBody] PersonDTO personCreate)
        {
            if (personCreate == null)
                return BadRequest(ModelState);

            var people = _personRepository.GetPeople()
                 .Where(c => c.LastName.Trim().ToUpper() == personCreate.LastName.TrimEnd().ToUpper())
                 .FirstOrDefault();

            if (people != null)
            {
                ModelState.AddModelError("", "reviewer already exists");
                return StatusCode(422, ModelState); //Handles if we have an existing record 

            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personMap = _mapper.Map<Person>(personCreate);
           

            if (!_personRepository.CreatePerson(personMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving data.");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully added the dog owner.");
        }

        [HttpPut("Update Person Information")] //{ownerId}
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)] //for update, have a not found response

        public IActionResult UpdatePerson(int personId, [FromBody] PersonDTO updatedPerson)
        {
            if (updatedPerson == null)
                return BadRequest(ModelState);

            if (personId != updatedPerson.Id)
                return BadRequest(ModelState);

            if (!_personRepository.PersonExists(personId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();


            var personMap = _mapper.Map<Person>(updatedPerson);

            if (!_personRepository.UpdatePerson(personMap))
            {
                ModelState.AddModelError("", "Something went wrong updating person info....");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("Delete an Person's record")] //{ownerId}
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult DeletePerson(int personId)
        {
            if (!_personRepository.PersonExists(personId))
            {
                return NotFound();
            }

            var dogsToDelete = _personRepository.GetAllDogsByOwner(personId);
            var personToDelete = _personRepository.GetPerson(personId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_dogRepository.DeleteDogs(dogsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong deleting this pokemon.");
            }

            if (!_personRepository.DeletePerson(personToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting this persons record.");
            }

            return NoContent();
        }

    }
}
