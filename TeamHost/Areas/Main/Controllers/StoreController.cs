using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.Text.Json;
using WebTeamHost.App.Features.Developers.Queries.GetDeveloperById;
using WebTeamHost.App.Features.Games.Queries.GetAllGames;
using WebTeamHost.App.Features.Games.Queries.GetGameById;
using WebTeamHost.Shared;
using static System.Net.Mime.MediaTypeNames;

namespace TeamHost.Areas.Main.Controllers;

[Area("Main")]
public class StoreController : Controller
{
    private readonly IMediator _mediator;

    public StoreController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //public async Task<ActionResult<Result<List<GetAllGamesDto>>>> Get()
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _mediator.Send(new GetAllGamesQuery());

        //Console.WriteLine(result.Data);
        return View(result.Data);
    }

    [HttpGet] // "/Main/Store/Card/{id}"
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken) // , CancellationToken cancellationToken
    {
        var game = await _mediator.Send(new GetGameByIdQuery(id), cancellationToken);
        var developer = await _mediator.Send(new GetDeveloperByIdQuery(game.Data.DeveloperId), cancellationToken);
        //List<string>? images = JsonSerializer.Deserialize<List<string>>(game.Data.Images);
        //Console.WriteLine(game.Data.Images);
        return View(
            new ViewModelDetails()
            {
                Game = game.Data,
                Developer = developer.Data,
                Images = new List<string>() { game.Data.Images }
            });
    }

    public record class ViewModelDetails()
    {
        public GetGameByIdDto Game;
        public GetDeveloperByIdDto Developer;
        public List<string> Images;
    }
}