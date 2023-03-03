using AutoMapper;
using DogApp.DTO;
using DogApp.Interface;
using DogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogController : Controller
    {
        private readonly IDogRepository _dogRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public DogController(IDogRepository dogRepository,
                             IPersonRepository personRepository,
                             IMapper mapper)
        {
            _dogRepository = dogRepository;
            _personRepository = personRepository;
            _mapper = mapper;
        }

        [HttpGet("Show All the Dogs we got!")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Dog>))] //type what kind of GET you want

        public IActionResult GetDogs() //this method, when called, will simply display the list of people in my Person table. No parm needed.
        {
            var dogs = _mapper.Map<List<DogDTO>>(_dogRepository.GetDogs()); //create variable to automap all of the fields I want to see (i made personDTO to limit the data shown).

            if (!ModelState.IsValid) //if badrequest, please tell me :)
                return BadRequest(ModelState);
            return Ok(dogs); //else everything is fine, return the var
        }

        [HttpGet("Look up a specific doggo")] //{dogId}
        [ProducesResponseType(200, Type = typeof(Dog))]
        [ProducesResponseType(400)]
        public IActionResult GetPerson(int dogId) //this method, when called, will display the specific personID entered.
        {
            if (!_dogRepository.DogExists(dogId)) //if no data found in personId after action, not found
                return NotFound();

            var dog = _mapper.Map<DogDTO>(_dogRepository.GetDog(dogId)); //variable to autoMap data from the found ID to display DTO fields

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(dog);
        }

        [HttpPost("Add a dog")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)] //FromBody = takes the entire CategoryDTO JSON encapsulation of data
        public IActionResult CreateDog([FromQuery] int personId, [FromBody] DogDTO dogCreate)
        {
            if (dogCreate == null)
                return BadRequest(ModelState); //nothing or unknown entry

            

            var dogs = _dogRepository.GetDogs()  //checking if a dog with that name is already entered (
                 .Where(c => c.Name.Trim().ToUpper() == dogCreate.Name.TrimEnd().ToUpper()).Where(i => i.CurrentOwnerId == personId)
                 .FirstOrDefault();

            

            if (dogs != null) //no dogowner would own 2 dogs with the same name hehe
            {
                ModelState.AddModelError("", "a dog with that name already exists for that Owner");
                return StatusCode(422, ModelState); //Handles if we have an existing record 

            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dogMap = _mapper.Map<Dog>(dogCreate); //autoMap object dog to var
            dogMap.CurrentOwner = _personRepository.GetPerson(personId); //assign current owner to the input owner ID

            if (!_dogRepository.CreateDog(dogMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving data.");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully Created.");
        }

        /*[HttpPut("Update Dog Information")] //{reviewerId}
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)] //for update, have a not found response

        public IActionResult UpdateDog(int dogId,
             [FromBody] DogDTO updatedDog)
        {
            if (updatedDog == null)
                return BadRequest(ModelState);

            if (dogId != updatedDog.Id)
                return BadRequest(ModelState);

            if (!_dogRepository.DogExists(dogId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

          //  var dogFK = dogId.CurrentOwnerId;
            var dogMap = _mapper.Map<Dog>(updatedDog);

            if (!_dogRepository.UpdateDog(dogMap))
            {
                ModelState.AddModelError("", "Something went wrong updating dog info....");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }*/

        [HttpDelete("Delete dog from table")] //{reviewerId}
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult DeleteDog(int dogId)
        {
            if (!_dogRepository.DogExists(dogId))
            {
                return NotFound();
            }

            var dogToDelete = _dogRepository.GetDog(dogId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_dogRepository.DeleteDog(dogToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting this doggo.");
            }

            return NoContent();
        }

    }
}
