﻿using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.IServices;
using CapitalPlacement.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapitalPlacement.API.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateQuestion(QuestionDto question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _questionService.CreateQuestionAsync(question);
            
            if (res.Success)
            {
                return CreatedAtAction(nameof(GetQuestion), new { res.Data.Id }, res.Data);
            }
            return BadRequest(res);

        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateQuestion(string id, QuestionDto question)
        {
            var res = await _questionService.UpdateQuestionAsync(id, question);
            if (res == true)
            {
                return NoContent();
            }
            return StatusCode(500, new { errors = new[] { "Something went wrong" } });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestion(string id)
        {
            var response = await _questionService.GetQuestionByIdAsync(id);
            if (response.Success) return Ok(response);
            if (!response.Success && response.Message is "Not Found") return NotFound();
            return StatusCode(500, new { message = "Internal Server Error!" });
        }

        [HttpGet()]
        public async Task<IActionResult> GetQuestionsByType(QuestionType question)
        {
            var response = await _questionService.GetQuestionsByTypeAsync(question);
            if (response.Success) return Ok(response);
            return StatusCode(500, new { message = "Internal Server Error!" });
        }
    }
}
