using Microsoft.AspNetCore.Mvc;
using StudyCenterBusiness;
using StudyCenterDataAccess.DTOs.PersonDTOs;

namespace StudyCenterRESTfulAPI.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        [HttpGet("all", Name = "GetAllPeople")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PersonDto>> GetAllUsers()
        {
            List<PersonDto> users = clsPerson.All();

            if (users == null || users.Count == 0)
            {
                return NotFound("No people found!");
            }

            return Ok(users);
        }

        [HttpGet("{personId}", Name = "GetPersonById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<PersonDto> GetPersonById(int personId)
        {
            if (personId < 1)
            {
                return BadRequest($"Not accepted ID {personId}");
            }

            clsPerson? person = clsPerson.Find(personId);

            if (person == null)
            {
                return NotFound($"User with ID {personId} is not found.");
            }

            return Ok(person.ToPersonDto());
        }

        [HttpPost("", Name = "AddNewPerson")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDto> AddPerson(PersonCreationDto newPerson)
        {
            if (newPerson == null)
            {
                return BadRequest($"Not accepted data");
            }

            clsPerson person = new clsPerson(new PersonDto(null, newPerson.FirstName,
                newPerson.SecondName, newPerson.ThirdName, newPerson.LastName,
                newPerson.Gender, newPerson.DateOfBirth, newPerson.PhoneNumber, newPerson.Email, newPerson.Address));

            if (person.TryToSave(out bool isValidateError))
            {
                return CreatedAtRoute("GetPersonById", new { personId = person.PersonID }, person.ToPersonDto());
            }
            else
            {
                return (isValidateError)
                    ?
                    BadRequest($"Missing data!")
                    :
                    StatusCode(500, new { message = "Error adding person" });
            }
        }

        [HttpPut("{personId}", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDto> UpdatePerson(int personId, PersonDto updatedPerson)
        {
            if (updatedPerson == null || personId < 1)
            {
                return BadRequest($"Not accepted data");
            }

            clsPerson? person = clsPerson.Find(personId);

            if (person == null)
            {
                return NotFound($"Person with ID {personId} is not found.");
            }

            person.FirstName = updatedPerson.FirstName;
            person.SecondName = updatedPerson.SecondName;
            person.ThirdName = updatedPerson.ThirdName;
            person.LastName = updatedPerson.LastName;
            person.Gender = (clsPerson.enGender)updatedPerson.Gender;
            person.DateOfBirth = updatedPerson.DateOfBirth;
            person.PhoneNumber = updatedPerson.PhoneNumber;
            person.Email = updatedPerson.Email;
            person.Address = updatedPerson.Address;

            if (person.TryToSave(out bool isValidateError))
            {
                return Ok(person.ToPersonDto());
            }
            else
            {
                return (isValidateError)
                    ?
                    BadRequest($"Missing data!")
                    :
                    StatusCode(500, new { message = "Error updating person" });
            }
        }

        [HttpDelete("{personId}", Name = "DeletePerson")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePerson(int personId)
        {
            if (personId < 1)
            {
                return BadRequest($"Not accepted ID {personId}");
            }

            if (clsPerson.Exists(personId))
            {
                if (clsPerson.Delete(personId))
                {
                    return Ok($"Person with ID {personId} has been deleted.");
                }
                else
                {
                    return StatusCode(500, new { message = "Error deleting person" });
                }
            }
            else
            {
                return NotFound($"Person with ID {personId} not found. no rows deleted!");
            }
        }

        [HttpGet("exists/{personId}", Name = "ExistsPersonByPersonId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> ExistsPersonByPersonId(int personId)
        {
            if (personId < 1)
            {
                return BadRequest($"Not accepted ID {personId}");
            }

            if (clsPerson.Exists(personId))
            {
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }
    }
}
