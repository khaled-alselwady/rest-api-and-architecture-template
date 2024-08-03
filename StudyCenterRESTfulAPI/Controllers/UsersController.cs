using Microsoft.AspNetCore.Mvc;
using StudyCenterBusiness;
using StudyCenterDataAccess.DTOs.UserDTOs;
using StudyCenterSharedDTOs.UserDTOs;

namespace StudyCenterRESTfulAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("all", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<UserViewDto>> GetAllUsers()
        {
            List<UserViewDto> users = clsUser.All();

            if (users == null || users.Count == 0)
            {
                return NotFound("No users found!");
            }

            return Ok(users);
        }

        [HttpGet("{userId}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<UserDetailsDto> GetUserById(int userId)
        {
            if (userId < 1)
            {
                return BadRequest($"Not accepted ID {userId}");
            }

            clsUser? user = clsUser.FindBy(userId, clsUser.enFindBy.UserID);

            if (user == null)
            {
                return NotFound($"User with ID {userId} is not found.");
            }

            return Ok(user.ToUserDetailsDto());
        }

        [HttpGet("person/{personId}", Name = "GetUserByPersonId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<UserDetailsDto> GetUserByPersonId(int personId)
        {
            if (personId < 1)
            {
                return BadRequest($"Not accepted ID {personId}");
            }

            clsUser? user = clsUser.FindBy(personId, clsUser.enFindBy.PersonID);

            if (user == null)
            {
                return NotFound($"User with ID {personId} is not found.");
            }

            return Ok(user.ToUserDetailsDto());
        }

        [HttpGet("username", Name = "GetUserByUsername")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<UserDetailsDto> GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest($"Not accepted data");
            }

            clsUser? user = clsUser.FindBy(username, clsUser.enFindBy.Username);

            if (user == null)
            {
                return NotFound($"User with username {username} is not found.");
            }

            return Ok(user.ToUserDetailsDto());
        }

        [HttpGet("username-password", Name = "GetUserByUsernameAndPassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<UserDetailsDto> GetUserByUsernameAndPassword(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest($"Not accepted data");
            }

            clsUser? user = clsUser.FindBy(username, password);

            if (user == null)
            {
                return NotFound($"User with this username/password is not found.");
            }

            return Ok(user.ToUserDetailsDto());
        }

        [HttpPost("", Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDto> AddUser(UserCreationDto newUser)
        {
            if (newUser == null)
            {
                return BadRequest($"Not accepted data");
            }

            clsUser user = new clsUser(new UserDto(null, newUser.PersonID, newUser.Username, newUser.Password, newUser.Permissions, newUser.IsActive));

            if (user.TryToSave(out bool isValidationError))
            {
                return CreatedAtRoute("GetUserById", new { userId = user.UserID }, user.ToUserDto());
            }
            else
            {
                return (isValidationError)
                    ?
                    BadRequest($"Missing data!")
                    :
                    StatusCode(500, new { message = "Error adding user" });
            }

        }

        [HttpPut("{userId}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDto> UpdateUser(int userId, UserDto updatedUser)
        {
            if (updatedUser == null || userId < 1)
            {
                return BadRequest($"Not accepted data");
            }

            clsUser? user = clsUser.FindBy(userId, clsUser.enFindBy.UserID);

            if (user == null)
            {
                return NotFound($"User with ID {userId} is not found.");
            }

            user.PersonID = updatedUser.PersonID;
            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
            user.Permissions = updatedUser.Permissions;
            user.IsActive = updatedUser.IsActive;

            if (user.TryToSave(out bool isValidationError))
            {
                return Ok(user.ToUserDto());
            }
            else
            {
                return (isValidationError)
                    ?
                    BadRequest($"Missing data!")
                    :
                    StatusCode(500, new { message = "Error updating user" });
            }
        }

        [HttpDelete("{userId}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteUser(int userId)
        {
            if (userId < 1)
            {
                return BadRequest($"Not accepted ID {userId}");
            }

            if (clsUser.Exists(userId, clsUser.enFindBy.UserID))
            {
                if (clsUser.Delete(userId))
                {
                    return Ok($"User with ID {userId} has been deleted.");
                }
                else
                {
                    return StatusCode(500, new { message = "Error deleting user" });
                }
            }
            else
            {
                return NotFound($"Person with ID {userId} not found. no rows deleted!");
            }
        }

        [HttpGet("exists/user-id/{userId}", Name = "ExistsUserByUserId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> ExistsUserByUserId(int userId)
        {
            if (userId < 1)
            {
                return BadRequest($"Not accepted ID {userId}");
            }

            if (clsUser.Exists(userId, clsUser.enFindBy.UserID))
            {
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        [HttpGet("exists/person-id/{personId}", Name = "ExistsUserByPersonId")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> ExistsUserByPersonId(int personId)
        {
            if (personId < 1)
            {
                return BadRequest($"Not accepted ID {personId}");
            }

            if (clsUser.Exists(personId, clsUser.enFindBy.PersonID))
            {
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        [HttpGet("exists/username/{username}", Name = "ExistsUserByUsername")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> ExistsUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest($"Not accepted username {username}");
            }

            if (clsUser.Exists(username, clsUser.enFindBy.Username))
            {
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        [HttpGet("exists/username-password", Name = "ExistsUserByUsernameAndPassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> ExistsUserByUsernameAndPassword(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest($"Not accepted data");
            }

            if (clsUser.Exists(username, password))
            {
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        [HttpGet("count", Name = "CountAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> Count()
        {
            return Ok(clsUser.Count());
        }

        [HttpPut("change-password", Name = "ChangePassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> ChangePassword(int userId, string newPassword)
        {
            if (userId < 1 || string.IsNullOrWhiteSpace(newPassword))
            {
                return BadRequest($"Not accepted data");
            }

            if (clsUser.ChangePassword(userId, newPassword))
            {
                return Ok(true);
            }
            else
            {
                return StatusCode(500, new { message = "Error changing password" });
            }
        }

        [HttpGet("get-permissions-list", Name = "GetPermissionsText")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<string>> GetPermissionsText(int permissionUser)
        {
            List<string> permissions = clsUser.GetPermissionsText(permissionUser);

            if (permissions == null || permissions.Count == 0)
            {
                return NotFound("No permissions found!");
            }

            return Ok(permissions);
        }
    }
}
