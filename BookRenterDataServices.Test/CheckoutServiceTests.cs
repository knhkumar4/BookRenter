using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookRenter.Services;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Interfaces;
using BookRenterData.Entities;

namespace BookRenterDataServices.Test
{
    public class CheckoutServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserClaimService> _mockUserClaimService;
        private readonly CheckoutService _checkoutService;

        public CheckoutServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserClaimService = new Mock<IUserClaimService>();
            _checkoutService = new CheckoutService(_mockUnitOfWork.Object, _mockUserClaimService.Object);
        }

        

    }
}
