using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamHost.Areas.Attributes;
using WebTeamHost.App.Features.Messages.Queries.GetMessagesByGroup;
using WebTeamHost.App.Features.UserGroups.Queries.GetGroupsByUser;
using WebTeamHost.Domain.Entities;

namespace TeamHost.Areas.Account.Controllers;

[AuthorizeAndRedirect]
[Area("Account")]
public class ChatsController : Controller
{
    private readonly IMediator _mediator;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    
    public ChatsController(IMediator mediator, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _mediator = mediator;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int number)
    {
        var resultGroups = _mediator.Send(new GetGroupsByUserQuery(_userManager.GetUserId(User)));
        var idGroup = resultGroups.Result.Data[number].Id;
        var resultMessages = _mediator.Send(new GetMessagesByGroupQuery(idGroup));
        if (resultMessages.Result.Succeeded)
        {
            return View(new ViewModelChats
            {
                UserId = _userManager.GetUserId(User),
                Username = _userManager.GetUserName(User),
                CurrentGroupId = idGroup,
                Groups = resultGroups.Result.Data,
                MessagesByCurrentGroup = resultMessages.Result.Data
            });
        }

        return View(new ViewModelChats
        {
            UserId = _userManager.GetUserId(User),
            Username = _userManager.GetUserName(User),
            CurrentGroupId = idGroup,
            Groups = resultGroups.Result.Data,
            MessagesByCurrentGroup = new List<GetMessagesByGroupDto>()
        });
    }

    public class ViewModelChats
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int CurrentGroupId { get; set; }
        public List<GetGroupsByUserDto> Groups { get; set; }
        public List<GetMessagesByGroupDto> MessagesByCurrentGroup { get; set; }
    }
}