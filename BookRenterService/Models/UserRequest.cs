using BookRenterData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRenterService.Models
{
    public class UserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public static implicit operator User(UserRequest userRequest)
        {
            return new User
            {
                Username = userRequest.Username,
                PasswordHash = userRequest.Password,
                Role = userRequest.Role
            };
        }
    }
}
