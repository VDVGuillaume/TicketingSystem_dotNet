using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;
using TicketingSystem.RazorWebsite.Controllers;
using TicketingSystem.RazorWebsite.Models;
using Xunit;

namespace TicketingSystem.Xunit.Tests
{
    #region MockObjects
    public class FakeSignInManager : SignInManager<IdentityUser>
    {
        public FakeSignInManager()
                : base(new FakeUserManager(),
                     new Mock<IHttpContextAccessor>().Object,
                     new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                     new Mock<IOptions<IdentityOptions>>().Object,
                     new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                     new Mock<IAuthenticationSchemeProvider>().Object,
                     new Mock<IUserConfirmation<IdentityUser>>().Object)
        { }
    }

    public class FakeUserManager : UserManager<IdentityUser>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<IdentityUser>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<IdentityUser>>().Object,
              new IUserValidator<IdentityUser>[0],
              new IPasswordValidator<IdentityUser>[0],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              new Mock<ILogger<UserManager<IdentityUser>>>().Object)
        { }

        public override Task<IdentityResult> CreateAsync(IdentityUser user, string password)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<string> GenerateEmailConfirmationTokenAsync(IdentityUser user)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }
    }
    #endregion MockObjects

    public class LoginUnitTests
    {
        [Theory]
        [InlineData("driesco","P@ssword1", 1, true)]
        [InlineData("driesco","P@ssword1", 2, false)]
        [InlineData("driesco","P@ssword1", 3, false)]
        public async void Login_Tests_Should_Be_Expected_Result(string expectedUsername, string expectedPassword, int signInResultInput, bool expectedSucceeded) 
        {
            //arrange
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<LoginModel>>();
            var signInManagerMock = new Mock<FakeSignInManager>();
            
            SignInResult signInResult = SignInResult.Success;
            string expectedModelStateErrorMessage = string.Empty;
            switch (signInResultInput) 
            {
                case 1:
                    signInResult = SignInResult.Success;
                    expectedModelStateErrorMessage = string.Empty;
                    break;
                case 2:
                    signInResult = SignInResult.Failed;
                    expectedModelStateErrorMessage = "Invalid login attempt.";
                    break;
                case 3:
                    signInResult = SignInResult.LockedOut;
                    expectedModelStateErrorMessage = "User account locked out.";
                    break;
                default:
                    throw new NotImplementedException();
            }

            CreateUserLoginAttemptCommand cmdSaved = null;

            mediatorMock.Setup(x =>
            x.Send(It.IsAny<CreateUserLoginAttemptCommand>(),
            It.IsAny<CancellationToken>()))
                .Callback<IRequest<UserLoginAttempt>, CancellationToken>((cmd, token) => cmdSaved = cmd as CreateUserLoginAttemptCommand);;

            signInManagerMock.Setup(x =>
                x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), true))
                .Returns(Task.FromResult(signInResult));

            var accountController = new AccountController(signInManagerMock.Object, loggerMock.Object, mediatorMock.Object);

            var loginModel = new LoginModel();
            loginModel.ReturnUrl = "~/";
            loginModel.Input = new LoginModel.InputModel
            {
                Username = expectedUsername,
                Password = expectedPassword
            };

            //act
            var actionResult = await accountController.Login(loginModel);

            //assert
            Assert.Equal(accountController.ModelState.IsValid, expectedSucceeded);
            Assert.Equal(cmdSaved.Success, expectedSucceeded);
            if (!expectedSucceeded)
                Assert.Contains(accountController.ModelState.Values, x => x.Errors.Any(x => x.ErrorMessage == expectedModelStateErrorMessage));
            mediatorMock.Verify(x => x.Send(It.IsAny<CreateUserLoginAttemptCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            signInManagerMock.Verify(x => x.PasswordSignInAsync(expectedUsername, expectedPassword, It.IsAny<bool>(), true), Times.Once);
            Assert.Equal(expectedUsername, cmdSaved.Username);
        }
    }
}
