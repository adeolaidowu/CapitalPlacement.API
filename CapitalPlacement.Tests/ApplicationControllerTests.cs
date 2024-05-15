using CapitalPlacement.API.Controllers;
using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.IServices;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Tests
{
    public class ApplicationControllerTests
    {
        private readonly Mock<IApplicationService> _mockApplicationService;
        private readonly ApplicationController _sut;

        public ApplicationControllerTests()
        {
            _mockApplicationService = new Mock<IApplicationService>();
            _sut = new ApplicationController(_mockApplicationService.Object);
        }

        [Fact]
        public async Task SubmitApplication_WithValidInput_ReturnsOk()
        {
            // Arrange
            var candidate = new CandidateApplicationDto
            {
                FirstName = "Zaraki",
                LastName = "Kenpachi",
                Email = "zaraki@email.com",
                Phone = "0932345671",
                Nationality = "Nigerian",
                DateOfBirth = new DateTime(1981,12,12),
                CurrentResidence = "4 Privet Drive",
                DateMovedToUk = new DateTime(2023, 4, 25),
                IdNumber = "user123",
                Gender = "male",
                YearOfGraduation = 2008,
                YearsOfExperience = 7,
                AboutYourself = "I am the captain of 10th squad",
                Skills = new List<string> {"C#", "React" },
                UkEmbassyRejection = false
            };
            _mockApplicationService.Setup(service => service.SubmitApplicationAsync(candidate))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.SubmitApplication(candidate);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be("Application submitted successfully.");
        }
    }
}
