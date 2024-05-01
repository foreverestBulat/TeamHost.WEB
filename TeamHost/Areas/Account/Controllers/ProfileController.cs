using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebTeamHost.Shared;
using WebTeamHost.App.Features.Users.Commands.Register;
using WebTeamHost.App.Features.Users.Commands.Login;
using WebTeamHost.App.Features.UsersInfo.Queries.GetUserInfoById;
using WebTeamHost.App.Features.UsersInfo.Commands.CreateUserInfo;
using WebTeamHost.App.Features.UsersInfo.Commands.UpdateUserInfo;
using WebTeamHost.Domain.Entities;
using TeamHost.Areas.Attributes;

namespace TeamHost.Areas.Account.Controllers;

[Area("Account")]
public class ProfileController : Controller
{
    private readonly IMediator _mediator;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public ProfileController(IMediator mediator, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _mediator = mediator;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpGet]
    public IActionResult Login() => View();


    [AuthorizeAndRedirect]
    [HttpGet]
    public IActionResult Index()
    {
        return View(new ViewModelDetails()
        {
            UserName = _userManager.GetUserName(User),
            //About = _mediator.Send(new GetUserInfoByUserQuery(_userManager.GetUserId(User))).Result.Data.About
        });
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<Result<int>>> Register(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine(command.Username);
        var user = new User
        {
            UserName = command.Email,
            Email = command.Email
        };
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Profile", new { area = "Account" });
        }
        foreach (var error in result.Errors)
        {
            Console.WriteLine(error.Description);
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home", new { area = "Home" });
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(command.Email, command.Password, false, false);
        if (result.Succeeded)
            return RedirectToAction("Index", "Profile", new { area = "Account" });
        return View();
    }

    [AuthorizeAndRedirect]
    [HttpGet]
    public IActionResult UserInfo(CancellationToken cancellationToken)
    {
        var result = _mediator.Send(new GetUserInfoByUserQuery(_userManager.GetUserId(User)), cancellationToken).Result;
        if (result.Succeeded)
            return View(result.Data);
        return View(new GetUserInfoByUserDto());
    }


    [AuthorizeAndRedirect]
    [HttpPost]
    public async Task<IActionResult> Update(GetUserInfoByUserDto getUserInfoByUserDto, CancellationToken cancellationToken)
    {
        if (!_mediator.Send(new UpdateUserInfoCommand(getUserInfoByUserDto)
            { UserId = _userManager.GetUserId(User) }).Result.Succeeded)
        {
            _mediator?.Send(new CreateUserInfoCommand(getUserInfoByUserDto) { UserId = _userManager.GetUserId(User) });
        }
        return RedirectToAction("Index", "Profile", new { area = "Account" });
    }

    public record class ViewModelDetails
    {
        public bool IsAuth { get; set; }
        public string UserName { get; set; }

        public string About { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}