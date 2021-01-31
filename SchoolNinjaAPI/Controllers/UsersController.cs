using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolNinjaAPI.Data;
using SchoolNinjaAPI.DTO;
using SchoolNinjaAPI.Models;
using SchoolNinjaAPI.Utils;

namespace SchoolNinjaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SchoolNinjaAPIContext _context;
        private readonly IUserProvider _userProvider;

        public UsersController(SchoolNinjaAPIContext context, IUserProvider userProvider)
        {
            _context = context;
            _userProvider = userProvider;
        }

        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok(_userProvider.GetUser());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpPost("register")]
        public ActionResult Register(UserRequestDTO newUserRequest)
        {
            User user = (from u in _context.User where u.Email == newUserRequest.Email select u).FirstOrDefault();
            if (user == null)
            {
                var salt = SecretUtils.GenerateSalt();
                var password = SecretUtils.EncodePassword(newUserRequest.Password, salt);
                User newUser = new User();
                newUser.Firstname = newUserRequest.Firstname;
                newUser.Lastname = newUserRequest.Lastname;
                newUser.Email = newUserRequest.Email;
                newUser.Type = newUserRequest.Type;
                if (newUser.Type.Equals("student"))
                {
                    newUser.Grade = newUserRequest.Grade;
                }
                newUser.Password = password;
                newUser.Salt = salt;
                newUser.CreatedAt = DateTime.Now;
                _context.User.Add(newUser);
                _context.SaveChanges();
                return Ok();
            }
            return Conflict();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
