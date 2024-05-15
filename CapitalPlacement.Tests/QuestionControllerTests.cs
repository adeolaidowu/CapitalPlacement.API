using CapitalPlacement.API.Controllers;
using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.IServices;
using CapitalPlacement.Core.Models;
using CapitalPlacement.Core.Response;
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
    public class QuestionControllerTests
    {
        private readonly Mock<IQuestionService> _mockQuestionService;
        private readonly QuestionController _sut;

        public QuestionControllerTests()
        {
            _mockQuestionService = new Mock<IQuestionService>();
            _sut = new QuestionController(_mockQuestionService.Object);
        }

        [Fact]
        public async Task CreateQuestion_ReturnsCreatedAtAction_WhenSuccess()
        {
            // Arrange
            var questionDto = new QuestionDto { Text = "Where are you from?", Type = QuestionType.Paragraph };
            var response = new ApiResponse<Question>
            {
                Success = true,
                Data = new Question { Id = Guid.NewGuid().ToString() }
            };
            _mockQuestionService.Setup(service => service.CreateQuestionAsync(questionDto))
                .ReturnsAsync(response);

            // Act
            var result = await _sut.CreateQuestion(questionDto);

            // Assert
            var actionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            actionResult.ActionName.Should().Be(nameof(_sut.GetQuestion));
            actionResult.RouteValues["id"].Should().Be(response.Data.Id);
            actionResult.Value.Should().Be(response.Data);
        }


        [Fact]
        public async Task CreateQuestion_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            _sut.ModelState.AddModelError("Text", "Question text is required");

            // Act
            var result = await _sut.CreateQuestion(new QuestionDto());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateQuestion_ReturnsNoContent_WhenSuccess()
        {
            // Arrange
            var questionId = "abc-123";
            var questionDto = new QuestionDto { Text = "Tell me about yourself", Type = QuestionType.Paragraph };
            _mockQuestionService.Setup(service => service.UpdateQuestionAsync(questionId, questionDto))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.UpdateQuestion(questionId, questionDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateQuestion_ReturnsInternalServerError_WhenFailure()
        {
            // Arrange
            var questionId = "abc-123";
            var questionDto = new QuestionDto { Text = "Tell me about yourself", Type = QuestionType.Paragraph };
            _mockQuestionService.Setup(service => service.UpdateQuestionAsync(questionId, questionDto))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.UpdateQuestion(questionId, questionDto);

            // Assert
            var actionResult = result.Should().BeOfType<ObjectResult>().Subject;
            actionResult.StatusCode.Should().Be(500);
            actionResult.Value.Should().BeEquivalentTo(new { errors = new[] { "Something went wrong" } });
        }

        [Fact]
        public async Task GetQuestionsByType_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var questionType = QuestionType.MultipleChoice;
            var response = new ApiResponse<IEnumerable<GetQuestionDto>> { Success = true, Data = new List<GetQuestionDto>() };
            _mockQuestionService.Setup(service => service.GetQuestionsByTypeAsync(questionType))
                .ReturnsAsync(response);

            // Act
            var result = await _sut.GetQuestionsByType(questionType);

            // Assert
            var actionResult = result.Should().BeOfType<OkObjectResult>().Subject;
            actionResult.Value.Should().Be(response);
        }

        [Fact]
        public async Task GetQuestionsByType_ReturnsInternalServerError_WhenFailure()
        {
            // Arrange
            var questionType = QuestionType.MultipleChoice;
            var response = new ApiResponse<IEnumerable<GetQuestionDto>> { Success = false };
            _mockQuestionService.Setup(service => service.GetQuestionsByTypeAsync(questionType))
                .ReturnsAsync(response);

            // Act
            var result = await _sut.GetQuestionsByType(questionType);

            // Assert
            var actionResult = result.Should().BeOfType<ObjectResult>().Subject;
            actionResult.StatusCode.Should().Be(500);
            actionResult.Value.Should().BeEquivalentTo(new { message = "Internal Server Error!" });
        }
    }
}
