using Hangfire;
using HotelApp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IMailService _mailService;
        public DemoController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetActionAsync([FromQuery] string message)
        {
            await Task.CompletedTask;
            return Ok(message);
        }

        [HttpGet("hangfire")]
        public async Task<IActionResult> TestHangfire()
        {
            await _mailService.SendEmailAsync(new Core.Models.MailRequest
            {
                Body = "Hello",
                Subject = "Den",
                ToEmail = "kotdeniss2@yandex.ru"
            }, default);
            return Ok("sent");
        }
    }
}
