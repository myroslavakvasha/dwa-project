using BL.DTOs.Auth;
using BL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class AuthService
    {
        private readonly GrillPizzaOrdersContext _context;

        public AuthService(GrillPizzaOrdersContext context)
        {
            _context = context;
        }

        public void Register(UserRegisterDto userDto)
        {
            var trimmedUsername = userDto.Username.Trim();
            if (_context.Users.Any(x => x.Username.Equals(trimmedUsername)))
                throw new Exception("Username already exists");

            Role? role = _context.Roles.FirstOrDefault(x => x.RoleTitle.Equals("User"));
            if(role == null)
                throw new InvalidOperationException("User role doesn't exist in the DB");
            var roleId = role.Id;

            var b64salt = PasswordHashProvider.GetSalt();
            var b64hash = PasswordHashProvider.GetHash(userDto.Password, b64salt);

            var user = new User
            {
                Username = userDto.Username,
                RoleId = roleId,
                PwdHash = b64hash,
                PwdSalt = b64salt,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Phone = userDto.Phone,
            };

            _context.Add(user);
            _context.SaveChanges();
        }

        public UserResponseDto? ValidateUser(string username, string password)
        {
            User? user = _context.Users.Include(x => x.Role).FirstOrDefault(x => x.Username.Equals(username.Trim()));
            if (user == null)
                return null; // catching nulls and showing generic message

            var b64hash = PasswordHashProvider.GetHash(password.Trim(), user.PwdSalt);
            if (b64hash != user.PwdHash)
                return null;

            return new UserResponseDto
            {
                Username = user.Username,
                RoleTitle = user.Role.RoleTitle,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone
            };
        }

        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Username.Equals(username.Trim()));
            if (user == null)
                throw new Exception("Username doesn't exist");

            var b64hash = PasswordHashProvider.GetHash(oldPassword.Trim(), user.PwdSalt);
            if (b64hash != user.PwdHash)
                throw new Exception("Old password incorrect");

            var b64saltNew = PasswordHashProvider.GetSalt();
            var b64hashNew = PasswordHashProvider.GetHash(newPassword.Trim(), b64saltNew);
            user.PwdSalt = b64saltNew;
            user.PwdHash = b64hashNew;

            _context.SaveChanges();
        }

        public UserResponseDto UpdateProfile(UserUpdateDto newUser)
        {
            User? user = _context.Users.Include(x => x.Role).FirstOrDefault(x => x.Username.Equals(newUser.Username));
            if (user == null)
                throw new Exception("Username doesn't exist");

            user.Username = newUser.Username;
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.Email = newUser.Email;
            user.Phone = newUser.Phone;

            _context.SaveChanges();

            return new UserResponseDto
            {
                Username = user.Username,
                RoleTitle = user.Role.RoleTitle,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone
            };
        }

        public List<UserResponseDto> GetAllUsers() => 
            _context.Users
            .Include(x => x.Role)
            .Where(x => x.Role.RoleTitle.ToLower().Equals("user"))
            .Select(x => new UserResponseDto
            {
                Username = x.Username,
                RoleTitle = x.Role.RoleTitle,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Phone = x.Phone
            })
            .ToList();

        public UserResponseDto GetByUsername(string username)
        {
            User? user = _context.Users.Include(x => x.Role).FirstOrDefault(x => x.Username.Equals(username));
            if (user == null)
                throw new Exception("Username doesn't exist");

            return new UserResponseDto
            {
                Username = user.Username,
                RoleTitle = user.Role.RoleTitle,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone
            };
        }

    }
}
