using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterRepository.Repositories.Interfaces;
using BookRenterService.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookRenterService.Concrete
{
    public class UserClaimService : IUserClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public UserClaimService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<User> GetUserFromClaimAsync()
        {
            // Get the HttpContext
            var httpContext = _httpContextAccessor.HttpContext;

            // Check if user is authenticated
            if (httpContext.User.Identity?.IsAuthenticated ?? false)
            {
                // Get the claim containing the username
                var usernameClaim = httpContext.User.FindFirst(ClaimTypes.Name);
                if (usernameClaim != null)
                {
                    // Retrieve the user from the repository based on the username claim
                    var username = usernameClaim.Value;
                    var userRepository = _unitOfWork.UserRepository; // Get the UserRepository from UnitOfWork
                    var user = await userRepository.GetUserByUsernameAsync(username);
                    return user;
                }
            }

            return null; // User not found or not authenticated
        }
    }
}
