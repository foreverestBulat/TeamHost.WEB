using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamHost.Areas.Attributes;
using WebTeamHost.App.Features.Groups.Commands.CreateGroup;
using WebTeamHost.Domain.Entities;
//using WebTeamHost.Application.Features.UserGroups.Queries.HasGroupUserWithUserByOne;

namespace TeamHost.Areas.Account.Controllers;

[Area("Account")]
public class FriendsController : Controller
{
    private readonly IMediator _mediator;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public FriendsController(IMediator mediator, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _mediator = mediator;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [AuthorizeAndRedirect]
    [HttpGet]
    public IActionResult Index()
    {
        return View(_userManager.Users.Where(user => user.Id != _userManager.GetUserId(User)));
    }


    [HttpGet]
    public async Task<IActionResult> Write(string id)
    {
        var result = await _mediator.Send(new CreateGroupCommand()
        {
            AddUserId = id,
            OwnerUser = await _userManager.GetUserAsync(User)
        });
        return RedirectToAction("Index", "Chats", new { area = "Account" });
    }

    //Console.WriteLine($"{id} ------------USERID");
    //Console.WriteLine("Õ¿œ»—¿“‹");
    //var has = await _mediator.Send(new HasGroupUserWithUserByOneQuery()
    //{
    //    UserId1 = _userManager.GetUserId(User),
    //    UserId2 = id
    //});
    //if (!has.Succeeded)
    //{
    //    //var result = await _mediator.Send(new CreateGroupCommand()
    //    //{
    //    //    AddUserId = id,
    //    //    OwnerUser = await _userManager.GetUserAsync(User)
    //    //});
    //    //Console.WriteLine(result.Data);
    //    Console.WriteLine("HAS GROUP");
    //}
}