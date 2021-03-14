using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;
using TicketingSystem.RazorWebsite.Models;

namespace TicketingSystem.RazorWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IMediator _mediator;

        public AccountController(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            IMediator mediator)
        {
            _signInManager = signInManager;
            _logger = logger;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var model = new LoginModel();

            if (!string.IsNullOrEmpty(model.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
            }
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ReturnUrl = returnUrl ?? Url.Content("~/Dashboard/Index");
            
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model) 
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Input.Username, model.Input.Password, model.Input.RememberMe, lockoutOnFailure: true);
                await _mediator.Send(new CreateUserLoginAttemptCommand { Date = DateTime.Now, Username = model.Input.Username, Success = result.Succeeded });

                if (result.Succeeded)
                {
                    _logger.LogInformation("User ingelogd.");
                    return LocalRedirect(model.ReturnUrl ?? Url.Content("~/Dashboard/Index"));
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = model.ReturnUrl, RememberMe = model.Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account is gelocked.");
                    ModelState.AddModelError(string.Empty, "User account is gelocked.");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Login mislukt!");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
