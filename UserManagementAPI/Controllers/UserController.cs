using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // Dictionary for storing users with thread-safe access
        private static ConcurrentDictionary<int, User> users = new ConcurrentDictionary<int, User>
        {
            [1] = new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
            [2] = new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" }
        };

        private readonly InputValidationService _validationService;

        public UserController(InputValidationService validationService)
        {
            _validationService = validationService;
        }

        // GET: api/User
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers([FromQuery] int limit = 10)
        {
            var limitedUsers = users.Values.Take(limit).ToList();
            return Ok(limitedUsers);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            if (users.TryGetValue(id, out var user))
            {
                return Ok(user);
            }
            return NotFound();
        }

        // POST: api/User
        [HttpPost]
        public IActionResult AddUser([FromBody] User newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate and clean user input
            var cleanedName = _validationService.ValidateAndCleanName(newUser.Name);
            var cleanedEmail = _validationService.ValidateAndCleanEmail(newUser.Email);

            if (cleanedName == null || cleanedEmail == null)
            {
                return BadRequest(new { error = "Invalid name or email." });
            }

            // Use the cleaned data
            newUser.Name = cleanedName;
            newUser.Email = cleanedEmail;

            // Generate a new unique ID
            int newId = users.Keys.DefaultIfEmpty(0).Max() + 1;
            newUser.Id = newId;

            if (users.TryAdd(newId, newUser))
            {
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
            }

            return BadRequest("Failed to add user.");
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (users.TryGetValue(id, out var existingUser))
            {
                existingUser.Name = updatedUser.Name;
                existingUser.Email = updatedUser.Email;
                return NoContent();
            }

            return NotFound();
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            if (users.TryRemove(id, out _))
            {
                return NoContent();
            }

            return NotFound();
        }
    }

    // User model with validation attributes
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public required string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }
    }
}